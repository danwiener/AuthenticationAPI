using Authentication.DTO;
using Microsoft.AspNetCore.Mvc;
using Authentication.Models;
using Authentication.Data;
using Authentication.Services;
using Google.Authenticator;
using System.Text;
using System.Reflection;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private FFContextDb db;
        private IConfiguration configuration;

        public AuthController(IConfiguration configuration, FFContextDb db)
        {
            this.configuration = configuration;
            this.db = db;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                return Unauthorized("Passwords do not match!");
            }
            User user = new User()
            {
                Username = dto.Username,
                Name = dto.Name,
                Email = dto.Email,
                Password = HashService.HashPassword(dto.Password)
            };

            db.Users.Add(user);
            db.SaveChanges();

            return Ok(user);
        } // End method

        // Generates access/refresh tokens and cookie which will be stored in the browser for 7 days
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            User? user = db.Users.Where(u => u.Email == dto.Email).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid credentials!");
            }
            if (HashService.HashPassword(dto.Password) != user.Password)
            {
                return Unauthorized("Invalid credentials!");
            }

			string accessToken = TokenService.CreateAccessToken(user.UserId, configuration.GetSection("JWT:AccessKey").Value);
			string refreshToken = TokenService.CreateRefreshToken(user.UserId, configuration.GetSection("JWT:RefreshKey").Value);

			CookieOptions cookieOptions = new();
			cookieOptions.HttpOnly = true;
			Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);

			UserToken token = new()
			{
				UserId = user.UserId,
				Token = refreshToken,
				ExpiredAt = DateTime.Now.AddDays(7)
			};

			db.UserTokens.Add(token);
			db.SaveChanges();

			return Ok(new
			{
				token = accessToken
			});


		} // End method


        [HttpGet("user")]
        public new IActionResult User()
        {
            string? authorizationHeader = Request.Headers["Authorization"];

            if (authorizationHeader is null || authorizationHeader.Length <= 8)
            {
                return Unauthorized("Unauthenticated");
            }

            string accessToken = authorizationHeader[7..];

            int id = TokenService.DecodeToken(accessToken, out bool hasTokenExpired);

            if (hasTokenExpired)
            {
                return Unauthorized("Unauthenticated");
            }

            User? user = db.Users.Where(u => u.UserId== id).FirstOrDefault();

            if (user is null)
            {
                return Unauthorized("Unauthenticated!");
            }

            return Ok(user);
        } // End method

        // Return user to the home page to get the user name and email
		[HttpGet("getuser")]
        public IActionResult UsernameEmail()
        {
            string? usernameEmailHeader = Request.Headers["UsernameEmail"];

			if (usernameEmailHeader is null || usernameEmailHeader.Length < 8)
			{
				return Unauthorized("Unauthenticated");
			}

            string stringid = usernameEmailHeader[7..];
            int.TryParse(stringid, out int id);

            User? user = db.Users.Where(u => u.UserId== id).FirstOrDefault();

			if (user is null)
			{
				return Unauthorized("Unauthenticated!");
			}

			return Ok(user);
		} // End method

        [HttpGet("getleaguesbelongedto")]
        public IActionResult GetLeaguesBelongedTo()
        {
            string? leaguesbelongedToHeader = Request.Headers["LeaguesBelongedToHeader"];
            if (leaguesbelongedToHeader is null || leaguesbelongedToHeader.Length < 1)
            {
                return Unauthorized("Unauthenticated");
            }

            string userid = leaguesbelongedToHeader[0..];
            int.TryParse(userid, out int id);

            int[]? leagueids = db.UserLeagues.Where(u => u.UserId == id).Select(u => u.LeagueId).ToArray();
            GetLeagueIdBelongedTo gl = new GetLeagueIdBelongedTo(leagueids);

			return Ok(gl);
        }

		[HttpGet("getteamsbelongedto")]
		public IActionResult GetTeamsBelongedTo()
		{
			string? teamsbelongedToHeader = Request.Headers["TeamsBelongedToHeader"];
			if (teamsbelongedToHeader is null || teamsbelongedToHeader.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

			string userid = teamsbelongedToHeader[0..];
			int.TryParse(userid, out int id);

			int[]? teamids = db.LeagueTeams.Where(l => l.UserId == id).Select(l => l.TeamId).ToArray();
			GetTeamIdBelongedTo gt = new GetTeamIdBelongedTo(teamids);

			return Ok(gt);
		}

		[HttpGet("getleagues")]
        public IActionResult GetLeagues()
        {
			string? leagues = Request.Headers["LeagueIdHeader"];
			if (leagues is null || leagues.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

            string leagueid = leagues[0..];
            int.TryParse(leagueid, out int leagueId);

            League? league = db.Leagues.Where(l => l.LeagueId == leagueId).FirstOrDefault(); 

            return Ok(league);
		}

		[HttpGet("getteams")]
		public IActionResult GetTeams()
		{
			string? teams = Request.Headers["TeamIdHeader"];
			if (teams is null || teams.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

			string teamidstr = teams[0..];
			int.TryParse(teamidstr, out int teamid);

			Team? team = db.Teams.Where(t => t.TeamId == teamid).FirstOrDefault();

			return Ok(team);
		}

		[HttpGet("getleaguerules")]
		public IActionResult GetLeagueRules()
		{
			string? leagueruleleagueid = Request.Headers["LeagueIdForRulesHeader"];
			if (leagueruleleagueid is null || leagueruleleagueid.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

			string leagueidstr = leagueruleleagueid[0..];
			int.TryParse(leagueidstr, out int leagueid);

			LeagueRules? leaguerules = db.LeagueRules.Where(lr => lr.LeagueId == leagueid).FirstOrDefault();

			return Ok(leaguerules);
		}

		[HttpPost("postleaguerules")]
		public IActionResult GetLeagueRules(UpdateRulesDTO dto)
		{

            db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().MaxTeams = dto.MaxTeams;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().MaxPlayers = dto.MaxPlayers;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().QbCount = dto.QbCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().RbCount = dto.RbCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().WrCount = dto.WrCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().TeCount = dto.TeCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().DCount = dto.DCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().KCount = dto.KCount;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PassingTDPoints = dto.PassingTDPoints;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PPC = dto.PPC;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PPI = dto.PPI;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PPTwentyFiveYdsPass = dto.PPTwentyFiveYdsPass;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FortyYardPassBonus = dto.FortyYardPassBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().SixtyYardPassBonus = dto.SixtyYardPassBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().ThreeHundredYardPassBonus = dto.ThreeHundredYardPassBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FiveHundredYardPassBonus = dto.FiveHundredYardPassBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().RushingTDPoints = dto.RushingTDPoints;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().ReceivingTDPoints = dto.ReceivingTDPoints;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PPTenRush = dto.PPTenRush;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FortyYardRushReceivingBonus = dto.FortyYardRushReceivingBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().SixtyYardRushReceivingBonus = dto.SixtyYardRushReceivingBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().OneHundredYardRushReceivingBonus = dto.OneHundredYardRushReceivingBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().TwoHundredYardRushReceivingBonus = dto.TwoHundredYardRushReceivingBonus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().PPR = dto.PPR;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().TwoPointConversion = dto.TwoPointConversion;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().InterceptionOffense = dto.InterceptionOffense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FumbleOffense = dto.FumbleOffense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().SafetyOffense = dto.SafetyOffense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().SackDefense = dto.SackDefense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().TackleDefense = dto.TackleDefense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgPuntBlock = dto.FgPuntBlock;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().InterceptionDefense = dto.InterceptionDefense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FumbleDefense = dto.FumbleDefense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().SafetyDefense = dto.SafetyDefense;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().IntTd = dto.IntTd;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FumbleTd = dto.FumbleTd;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().ReturnTd = dto.ReturnTd;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgTenToTwenty = dto.FgTenToTwenty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedTen = dto.FgMissedTen;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgTwentyToThirty = dto.FgTwentyToThirty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedTwenty = dto.FgMissedTwenty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgThirtyToForty = dto.FgThirtyToForty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedThirty = dto.FgMissedThirty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgFortyToFifty = dto.FgFortyToFifty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedforty = dto.FgMissedforty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgFiftyToSixty = dto.FgFiftyToSixty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedFifty = dto.FgMissedFifty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgSixtyPlus = dto.FgSixtyPlus;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().FgMissedSixty = dto.FgMissedSixty;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().XpMade = dto.XpMade;
			db.LeagueRules.Where(lr => lr.LeagueId == dto.LeagueId).FirstOrDefault().XpMissed = dto.XpMissed;


			db.SaveChanges();
			return Ok();
		}

		[HttpGet("getgloballeagueids")]
		public IActionResult GetGlobalLeagues()
		{
            int[]? leagueids = db.Leagues.Where(l => l.LeagueId >= 1).Select(l => l.LeagueId).ToArray();
            GetLeagueIdBelongedTo gl = new GetLeagueIdBelongedTo(leagueids);

			return Ok(gl);
		}

		[HttpPost("createleague")]
        public IActionResult CreateLeague(CreateLeagueDTO dto)
        {
            if (db.Leagues.Where(l => l.LeagueName == dto.LeagueName).Any())
            {
                return Unauthorized("League name already exists, must be unique");
            }
            if (dto.MaxTeams > 16)
            {
                return Unauthorized("League cannot contain more than 16 teams");
            }
            League league = new League();
            league.LeagueName = dto.LeagueName;
            league.MaxTeams = dto.MaxTeams;
            league.Creator = dto.Creator;
            db.Leagues.Add(league);
            db.SaveChanges();

            LeagueRules leagueRules = new LeagueRules(league.LeagueId);
            leagueRules.MaxTeams = league.MaxTeams;
            db.LeagueRules.Add(leagueRules);
            db.SaveChanges();

            User_League ul = new User_League();
			ul.UserId = dto.Creator;
			ul.User = db.Users.Where(u => u.UserId == dto.Creator).FirstOrDefault();
            ul.LeagueId = league.LeagueId;
            ul.League = league;

            db.UserLeagues.Add(ul);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost("deleteleague")]
        public IActionResult DeleteLeague(DeleteLeagueDTO dto)
        {
            League? league = db.Leagues.Where(l => l.LeagueId == dto.LeagueId).FirstOrDefault();
            User_League? ul = db.UserLeagues.Where(ul => ul.LeagueId == dto.LeagueId).FirstOrDefault();

            db.Leagues.Remove(league);
            db.UserLeagues.Remove(ul);
            db.SaveChanges();
            return Ok();
        }


		// Successfully returned authenticated user, but access token only lives for 30 seconds. Generate new access token using refresh token
		[HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (Request.Cookies["refresh_token"] is null)
            {
                return Unauthorized("Unauthenticated!");
            }

            string? refreshToken = Request.Cookies["refresh_token"];

            int id = TokenService.DecodeToken(refreshToken, out bool hasTokenExpired);

            if (!db.UserTokens.Where(u => u.UserId == id && u.Token == refreshToken && u.ExpiredAt > DateTime.Now).Any())
            {
                return Unauthorized("Unauthenticated!");
            }

            if (hasTokenExpired)
            {
                return Unauthorized("Unauthenticated!");
            }
            string accessToken = TokenService.CreateAccessToken(id, configuration.GetSection("JWT:AccessKey").Value);

            return Ok(new
            {
                token = accessToken
            });
        } // End method

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string? refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken is null)
            {
                return Ok("Already logged out!");
            }
            db.UserTokens.Remove(db.UserTokens.Where(u => u.Token == refreshToken).First());
            db.SaveChanges();

            Response.Cookies.Delete("refresh_token");

            return Ok("Loged out successfully!");
        }
    } // End class
} // End namespace
