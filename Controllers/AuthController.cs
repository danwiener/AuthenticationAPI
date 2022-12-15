using Authentication.DTO;
using Microsoft.AspNetCore.Mvc;
using Authentication.Models;
using Authentication.Data;
using Authentication.Services;
using Google.Authenticator;
using System.Text;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

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

		[HttpGet("getteamsinleague")]
		public IActionResult GetTeamsInLeague()
		{
			string? teamsbelongedToHeader = Request.Headers["TeamsInLeagueHeader"];
			if (teamsbelongedToHeader is null || teamsbelongedToHeader.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

			string leagueid = teamsbelongedToHeader[0..];
			int.TryParse(leagueid, out int id);

			int[]? teamids = db.LeagueTeams.Where(l => l.LeagueId == id).Select(l => l.TeamId).ToArray();
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

		[HttpGet("getcurrentnumberteams")]
		public IActionResult GetNumberTeams()
		{
			string? leagues = Request.Headers["NumberTeamsHeader"];
			if (leagues is null || leagues.Length < 1)
			{
				return Unauthorized("Unauthenticated");
			}

			string leagueidstr = leagues[0..];
			int.TryParse(leagueidstr, out int leagueId);

			int numberteams = db.Teams.Where(t => t.League == leagueId).Count();
			NumberTeamsDTO dto = new NumberTeamsDTO(numberteams);

			return Ok(dto);
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

			if (db.Players.IsNullOrEmpty())
			{
				// Create players associated with league
				string[] firstNames = new string[] {"Liam", "Noah", "Oliver", "Elijah",
				"William", "James", "Benjamin", "Lucas", "Henry", "Alexander", "Mason", "Michael", "Ethan", "Daniel",
				"Jacob", "Logan", "Jackson", "Levi", "Sebastian", "Mateo", "Jack", "Owen", "Theodore", "Aiden",
				"Samuel", "Joseph", "John", "David", "Wyatt", "Matthew", "Luke", "Asher", "Carter", "Julian",
				"Grayson", "Leo", "Jayden", "Gabriel", "Isaac", "Lincoln", "Anthony", "Hudson", "Dylan",
				"Ezra", "Thomas", "Charles", "Christopher", "Jaxon", "Maverick", "Josiah", "Isaiah", "Andrew",
				"Elias", "Joshua", "Nathan", "Caleb", "Ryan", "Adrian", "Miles", "Eli", "Nolan", "Christian",
				"Aaron", "Cameron", "Ezekiel", "Colton", "Luca", "Landon", "Hunter", "Jonathan", "Santiago",
				"Axel", "Easton", "Cooper", "Jeremiah", "Angel", "Roman", "Connor", "Jameson", "Robert",
				"Greyson", "Jordan", "Ian", "Carson", "Jaxson", "Leonardo", "Nicholas", "Dominic", "Austin",
				"Everett", "Brooks", "Xavier", "Kai", "Jose", "Parker", "Adam", "Jace", "Wesley", "Kayden",
				"Silas", "Amiri", "Eliam", "Colter", "Ozzy", "Loyal", "Khai", "Evander", "Camilo", "Mac",
				"Jiraiya", "Banks", "Gian", "Wylder", "Elio", "Kylian", "Cillian", "Bridger", "Onyx",
				"Zyair", "Koen", "Aaran", "Aaren", "Aarez", "Aarman", "Aaron", "Aaron-James", "Aarron",
				"Aaryan", "Aaryn", "Aayan", "Aazaan", "Abaan", "Abbas", "Abdallah", "Abdalroof", "Abdihakim",
				"Abdirahman", "Abdisalam", "Abdul", "Abdul-Aziz", "Abdulbasir", "Abdulkadir", "Abdulkarem", "Abdulkhader",
				"Abdullah", "Abdul-Majeed", "Abdulmalik", "Abdul-Rehman", "Abdur", "Abdurraheem",
				"Abdur-Rahman", "Abdur-Rehmaan", "Abel", "Abhinav", "Abhisumant", "Abid", "Abir",
				"Abraham", "Abu", "Abubakar", "Ace", "Adain", "Adam", "Adam-James", "Addison", "Addisson",
				"Adegbola", "Adegbolahan", "Aden", "Adenn", "Adie", "Adil", "Aditya", "Adnan", "Adrian", "Adrien",
				"Aedan", "Aedin", "Aedyn", "Aeron", "Afonso", "Ahmad", "Ahmed", "Ahmed-Aziz", "Ahoua",
				"Ahtasham", "Aiadan", "Aidan", "Aiden", "Aiden-Jack", "Aiden-Vee", "Aidian", "Aidy", "Ailin", "Aiman", "Ainsley", "Ainslie", "Airen", "Airidas", "Airlie", "AJ", "Ajay", "A-Jay", "Ajayraj", "Akan", "Akram", "Al", "Ala", "Alan", "Alanas", "Alasdair", "Alastair", "Alber", "Albert", "Albie", "Aldred", "Alec", "Aled", "Aleem", "Aleksandar", "Aleksander", "Aleksandr", "Aleksandrs", "Alekzander", "Alessandro", "Alessio", "Alex", "Alexander", "Alexei", "Alexx", "Alexzander", "Alf", "Alfee", "Alfie", "Alfred", "Alfy", "Alhaji", "Al-Hassan", "Ali", "Aliekber", "Alieu", "Alihaider", "Alisdair", "Alishan", "Alistair", "Alistar", "Alister", "Aliyaan", "Allan", "Allan-Laiton", "Allen", "Allesandro", "Allister", "Ally", "Alphonse", "Altyiab", "Alum", "Alvern", "Alvin", "Alyas", "Amaan", "Aman", "Amani", "Ambanimoh", "Ameer", "Amgad", "Ami", "Amin", "Amir", "Ammaar", "Ammar", "Ammer", "Amolpreet", "Amos", "Amrinder", "Amrit", "Amro", "Anay", "Andrea", "Andreas", "Andrei", "Andrejs", "Andrew", "Andy", "Anees", "Anesu", "Angel", "Angelo", "Angus", "Anir", "Anis", "Anish", "Anmolpreet", "Annan", "Anndra", "Anselm", "Anthony", "Anthony-John", "Antoine", "Anton", "Antoni", "Antonio", "Antony", "Antonyo", "Anubhav", "Aodhan", "Aon", "Aonghus", "Apisai", "Arafat", "Aran", "Arandeep", "Arann", "Aray", "Arayan", "Archibald", "Archie", "Arda", "Ardal", "Ardeshir", "Areeb", "Areez", "Aref", "Arfin", "Argyle", "Argyll", "Ari", "Aria", "Arian", "Arihant", "Aristomenis", "Aristotelis", "Arjuna", "Arlo", "Armaan", "Arman", "Armen", "Arnab", "Arnav", "Arnold", "Aron", "Aronas", "Arran", "Arrham", "Arron", "Arryn", "Arsalan", "Artem", "Arthur", "Artur", "Arturo", "Arun", "Arunas", "Arved", "Arya", "Aryan", "Aryankhan", "Aryian", "Aryn", "Asa", "Asfhan", "Ash", "Ashlee-jay", "Ashley", "Ashton", "Ashton-Lloyd", "Ashtyn", "Ashwin", "Asif", "Asim", "Aslam", "Asrar", "Ata", "Atal", "Atapattu", "Ateeq", "Athol", "Athon", "Athos-Carlos", "Atli", "Atom", "Attila", "Aulay", "Aun", "Austen", "Austin", "Avani", "Averon", "Avi", "Avinash", "Avraham", "Awais", "Awwal", "Axel", "Ayaan", "Ayan", "Aydan", "Ayden", "Aydin", "Aydon", "Ayman", "Ayomide", "Ayren", "Ayrton", "Aytug", "Ayub", "Ayyub", "Azaan", "Azedine", "Azeem", "Azim", "Aziz", "Azlan", "Azzam", "Azzedine", "Babatunmise", "Babur", "Bader", "Badr", "Badsha", "Bailee", "Bailey", "Bailie", "Bailley", "Baillie", "Baley", "Balian", "Banan", "Barath", "Barkley", "Barney", "Baron", "Barrie", "Barry", "Bartlomiej", "Bartosz", "Basher", "Basile", "Baxter", "Baye", "Bayley", "Beau", "Beinn", "Bekim", "Believe", "Ben", "Bendeguz", "Benedict", "Benjamin", "Benjamyn", "Benji", "Benn", "Bennett", "Benny", "Benoit", "Bentley", "Berkay", "Bernard", "Bertie", "Bevin", "Bezalel", "Bhaaldeen", "Bharath", "Bilal", "Bill", "Billy", "Binod", "Bjorn", "Blaike", "Blaine", "Blair", "Blaire", "Blake", "Blazej", "Blazey", "Blessing", "Blue", "Blyth", "Bo", "Boab", "Bob", "Bobby", "Bobby-Lee", "Bodhan", "Boedyn", "Bogdan", "Bohbi", "Bony", "Bowen", "Bowie", "Boyd", "Bracken", "Brad", "Bradan", "Braden", "Bradley", "Bradlie", "Bradly", "Brady", "Bradyn", "Braeden", "Braiden", "Brajan", "Brandan", "Branden", "Brandon", "Brandonlee", "Brandon-Lee", "Brandyn", "Brannan", "Brayden", "Braydon", "Braydyn", "Breandan", "Brehme", "Brendan", "Brendon", "Brendyn", "Breogan", "Bret", "Brett", "Briaddon", "Brian", "Brodi", "Brodie", "Brody", "Brogan", "Broghan", "Brooke", "Brooklin", "Brooklyn", "Bruce", "Bruin", "Bruno", "Brunon", "Bryan", "Bryce", "Bryden", "Brydon", "Brydon-Craig", "Bryn", "Brynmor", "Bryson", "Buddy", "Bully", "Burak", "Burhan", "Butali", "Butchi", "Byron", "Cabhan", "Cadan", "Cade", "Caden", "Cadon", "Cadyn", "Caedan", "Caedyn", "Cael", "Caelan", "Caelen", "Caethan", "Cahl", "Cahlum", "Cai", "Caidan", "Caiden", "Caiden-Paul", "Caidyn", "Caie", "Cailaen", "Cailean", "Caileb-John", "Cailin", "Cain", "Caine", "Cairn", "Cal", "Calan", "Calder", "Cale", "Calean", "Caleb", "Calen", "Caley", "Calib", "Calin", "Callahan", "Callan", "Callan-Adam", "Calley", "Callie", "Callin", "Callum", "Callun", "Callyn", "Calum", "Calum-James", "Calvin", "Cambell", "Camerin", "Cameron", "Campbel", "Campbell", "Camron", "Caolain", "Caolan", "Carl", "Carlo", "Carlos", "Carrich", "Carrick", "Carson", "Carter", "Carwyn", "Casey", "Casper", "Cassy", "Cathal", "Cator", "Cavan", "Cayden", "Cayden-Robert", "Cayden-Tiamo", "Ceejay", "Ceilan", "Ceiran", "Ceirin", "Ceiron", "Cejay", "Celik", "Cephas", "Cesar", "Cesare", "Chad", "Chaitanya", "Chang-Ha", "Charles", "Charley", "Charlie", "Charly", "Chase", "Che", "Chester", "Chevy", "Chi", "Chibudom", "Chidera", "Chimsom", "Chin", "Chintu", "Chiqal", "Chiron", "Chris", "Chris-Daniel", "Chrismedi", "Christian", "Christie", "Christoph", "Christopher", "Christopher-Lee", "Christy", "Chu", "Chukwuemeka", "Cian", "Ciann", "Ciar", "Ciaran", "Ciarian", "Cieran", "Cillian", "Cillin", "Cinar", "CJ", "C-Jay", "Clark", "Clarke", "Clayton", "Clement", "Clifford", "Clyde", "Cobain", "Coban", "Coben", "Cobi", "Cobie", "Coby", "Codey", "Codi", "Codie", "Cody", "Cody-Lee", "Coel", "Cohan", "Cohen", "Colby", "Cole", "Colin", "Coll", "Colm", "Colt", "Colton", "Colum", "Colvin", "Comghan", "Conal", "Conall", "Conan", "Conar", "Conghaile", "Conlan", "Conley", "Conli", "Conlin", "Conlly", "Conlon", "Conlyn", "Connal", "Connall", "Connan", "Connar", "Connel", "Connell", "Conner", "Connolly", "Connor", "Connor-David", "Conor", "Conrad", "Cooper", "Copeland", "Coray", "Corben", "Corbin", "Corey", "Corey-James", "Corey-Jay", "Cori", "Corie", "Corin", "Cormac", "Cormack", "Cormak", "Corran", "Corrie", "Cory", "Cosmo", "Coupar", "Craig", "Craig-James", "Crawford", "Creag", "Crispin", "Cristian", "Crombie", "Cruiz", "Cruz", "Cuillin", "Cullen", "Cullin", "Curtis", "Cyrus", "Daanyaal", "Daegan", "Daegyu", "Dafydd", "Dagon", "Dailey", "Daimhin", "Daithi", "Dakota", "Daksh", "Dale", "Dalong", "Dalton", "Damian", "Damien", "Damon", "Dan", "Danar", "Dane", "Danial", "Daniel", "Daniele", "Daniel-James", "Daniels", "Daniil", "Danish", "Daniyal", "Danniel", "Danny", "Dante", "Danyal", "Danyil", "Danys", "Daood", "Dara", "Darach", "Daragh", "Darcy", "D'arcy", "Dareh", "Daren", "Darien", "Darius", "Darl", "Darn", "Darrach", "Darragh", "Darrel", "Darrell", "Darren", "Darrie", "Darrius", "Darroch", "Darryl", "Darryn", "Darwyn", "Daryl", "Daryn", "Daud", "Daumantas", "Davi", "David", "David-Jay", "David-Lee", "Davie", "Davis", "Davy", "Dawid", "Dawson", "Dawud", "Dayem", "Daymian", "Deacon", "Deagan", "Dean", "Deano", "Decklan", "Declain", "Declan", "Declyan", "Declyn", "Dedeniseoluwa", "Deecan", "Deegan", "Deelan", "Deklain-Jaimes", "Del", "Demetrius", "Denis", "Deniss", "Dennan", "Dennin", "Dennis", "Denny", "Dennys", "Denon", "Denton", "Denver", "Denzel", "Deon", "Derek", "Derick", "Derin", "Dermot", "Derren", "Derrie", "Derrin", "Derron", "Derry", "Derryn", "Deryn", "Deshawn", "Desmond", "Dev", "Devan", "Devin", "Devlin", "Devlyn", "Devon", "Devrin", "Devyn", "Dex", "Dexter", "Dhani", "Dharam", "Dhavid", "Dhyia", "Diarmaid", "Diarmid", "Diarmuid", "Didier", "Diego", "Diesel", "Diesil", "Digby", "Dilan", "Dilano", "Dillan", "Dillon", "Dilraj", "Dimitri", "Dinaras", "Dion", "Dissanayake", "Dmitri", "Doire", "Dolan", "Domanic", "Domenico", "Domhnall", "Dominic", "Dominick", "Dominik", "Donald", "Donnacha", "Donnie", "Dorian", "Dougal", "Douglas", "Dougray", "Drakeo", "Dre", "Dregan", "Drew", "Dugald", "Duncan", "Duriel", "Dustin", "Dylan", "Dylan-Jack", "Dylan-James", "Dylan-John", "Dylan-Patrick", "Dylin", "Dyllan", "Dyllan-James", "Dyllon", "Eadie", "Eagann", "Eamon", "Eamonn", "Eason", "Eassan", "Easton", "Ebow", "Ed", "Eddie", "Eden", "Ediomi", "Edison", "Eduardo", "Eduards", "Edward", "Edwin", "Edwyn", "Eesa", "Efan", "Efe", "Ege", "Ehsan", "Ehsen", "Eiddon", "Eidhan", "Eihli", "Eimantas", "Eisa", "Eli", "Elias", "Elijah", "Eliot", "Elisau", "Eljay", "Eljon", "Elliot", "Elliott", "Ellis", "Ellisandro", "Elshan", "Elvin", "Elyan", "Emanuel", "Emerson", "Emil", "Emile", "Emir", "Emlyn", "Emmanuel", "Emmet", "Eng", "Eniola", "Enis", "Ennis", "Enrico", "Enrique", "Enzo", "Eoghain", "Eoghan", "Eoin", "Eonan", "Erdehan", "Eren", "Erencem", "Eric", "Ericlee", "Erik", "Eriz", "Ernie-Jacks", "Eroni", "Eryk", "Eshan", "Essa", "Esteban", "Ethan", "Etienne", "Etinosa", "Euan", "Eugene", "Evan", "Evann", "Ewan", "Ewen", "Ewing", "Exodi", "Ezekiel", "Ezra", "Fabian", "Fahad", "Faheem", "Faisal", "Faizaan", "Famara", "Fares", "Farhaan", "Farhan", "Farren", "Farzad", "Fauzaan", "Favour", "Fawaz", "Fawkes", "Faysal", "Fearghus", "Feden", "Felix", "Fergal", "Fergie", "Fergus", "Ferre", "Fezaan", "Fiachra", "Fikret", "Filip", "Filippo", "Finan", "Findlay", "Findlay-James", "Findlie", "Finlay", "Finley", "Finn", "Finnan", "Finnean", "Finnen", "Finnlay", "Finnley", "Fintan", "Fionn", "Firaaz", "Fletcher", "Flint", "Florin", "Flyn", "Flynn", "Fodeba", "Folarinwa", "Forbes", "Forgan", "Forrest", "Fox", "Francesco", "Francis", "Francisco", "Franciszek", "Franco", "Frank", "Frankie", "Franklin", "Franko", "Fraser", "Frazer", "Fred", "Freddie", "Frederick", "Fruin", "Fyfe", "Fyn", "Fynlay", "Fynn", "Gabriel", "Gallagher", "Gareth", "Garren", "Garrett", "Garry", "Gary", "Gavin", "Gavin-Lee", "Gene", "Geoff", "Geoffrey", "Geomer", "Geordan", "Geordie", "George", "Georgia", "Georgy", "Gerard", "Ghyll", "Giacomo", "Gian", "Giancarlo", "Gianluca", "Gianmarco", "Gideon", "Gil", "Gio", "Girijan", "Girius", "Gjan", "Glascott", "Glen", "Glenn", "Gordon", "Grady", "Graeme", "Graham", "Grahame", "Grant", "Grayson", "Greg", "Gregor", "Gregory", "Greig", "Griffin", "Griffyn", "Grzegorz", "Guang", "Guerin", "Guillaume", "Gurardass", "Gurdeep", "Gursees", "Gurthar", "Gurveer", "Gurwinder", "Gus", "Gustav", "Guthrie", "Guy", "Gytis", "Habeeb", "Hadji", "Hadyn", "Hagun", "Haiden", "Haider", "Hamad", "Hamid", "Hamish", "Hamza", "Hamzah", "Han", "Hansen", "Hao", "Hareem", "Hari", "Harikrishna", "Haris", "Harish", "Harjeevan", "Harjyot", "Harlee", "Harleigh", "Harley", "Harman", "Harnek", "Harold", "Haroon", "Harper", "Harri", "Harrington", "Harris", "Harrison", "Harry", "Harvey", "Harvie", "Harvinder", "Hasan", "Haseeb", "Hashem", "Hashim", "Hassan", "Hassanali", "Hately", "Havila", "Hayden", "Haydn", "Haydon", "Haydyn", "Hcen", "Hector", "Heddle", "Heidar", "Heini", "Hendri", "Henri", "Henry", "Herbert", "Heyden", "Hiro", "Hirvaansh", "Hishaam", "Hogan", "Honey", "Hong", "Hope", "Hopkin", "Hosea", "Howard", "Howie", "Hristomir", "Hubert", "Hugh", "Hugo", "Humza", "Hunter", "Husnain", "Hussain", "Hussan", "Hussnain", "Hussnan", "Hyden", "I", "Iagan", "Iain", "Ian", "Ibraheem", "Ibrahim", "Idahosa", "Idrees", "Idris", "Iestyn", "Ieuan", "Igor", "Ihtisham", "Ijay", "Ikechukwu", "Ikemsinachukwu", "Ilyaas", "Ilyas", "Iman", "Immanuel", "Inan", "Indy", "Ines", "Innes", "Ioannis", "Ireayomide", "Ireoluwa", "Irvin", "Irvine", "Isa", "Isaa", "Isaac", "Isaiah", "Isak", "Isher", "Ishwar", "Isimeli", "Isira", "Ismaeel", "Ismail", "Israel", "Issiaka", "Ivan", "Ivar", "Izaak", "J", "Jaay", "Jac", "Jace", "Jack", "Jacki", "Jackie", "Jack-James", "Jackson", "Jacky", "Jacob", "Jacques", "Jad", "Jaden", "Jadon", "Jadyn", "Jae", "Jagat", "Jago", "Jaheim", "Jahid", "Jahy", "Jai", "Jaida", "Jaiden", "Jaidyn", "Jaii", "Jaime", "Jai-Rajaram", "Jaise", "Jak", "Jake", "Jakey", "Jakob", "Jaksyn", "Jakub", "Jamaal", "Jamal", "Jameel", "Jameil", "James", "James-Paul", "Jamey", "Jamie", "Jan", "Jaosha", "Jardine", "Jared", "Jarell", "Jarl", "Jarno", "Jarred", "Jarvi", "Jasey-Jay", "Jasim", "Jaskaran", "Jason", "Jasper", "Jaxon", "Jaxson", "Jay", "Jaydan", "Jayden", "Jayden-James", "Jayden-Lee", "Jayden-Paul", "Jayden-Thomas", "Jaydn", "Jaydon", "Jaydyn", "Jayhan", "Jay-Jay", "Jayke", "Jaymie", "Jayse", "Jayson", "Jaz", "Jazeb", "Jazib", "Jazz", "Jean", "Jean-Lewis", "Jean-Pierre", "Jebadiah", "Jed", "Jedd", "Jedidiah", "Jeemie", "Jeevan", "Jeffrey", "Jensen", "Jenson", "Jensyn", "Jeremy", "Jerome", "Jeronimo", "Jerrick", "Jerry", "Jesse", "Jesuseun", "Jeswin", "Jevan", "Jeyun", "Jez", "Jia", "Jian", "Jiao", "Jimmy", "Jincheng", "JJ", "Joaquin", "Joash", "Jock", "Jody", "Joe", "Joeddy", "Joel", "Joey", "Joey-Jack", "Johann", "Johannes", "Johansson", "John", "Johnathan", "Johndean", "Johnjay", "John-Michael", "Johnnie", "Johnny", "Johnpaul", "John-Paul", "John-Scott", "Johnson", "Jole", "Jomuel", "Jon", "Jonah", "Jonatan", "Jonathan", "Jonathon", "Jonny", "Jonothan", "Jon-Paul", "Jonson", "Joojo", "Jordan", "Jordi", "Jordon", "Jordy", "Jordyn", "Jorge", "Joris", "Jorryn", "Josan", "Josef", "Joseph", "Josese", "Josh", "Joshiah", "Joshua", "Josiah", "Joss", "Jostelle", "Joynul", "Juan", "Jubin", "Judah", "Jude", "Jules", "Julian", "Julien", "Jun", "Junior", "Jura", "Justan", "Justin", "Justinas", "Kaan", "Kabeer", "Kabir", "Kacey", "Kacper", "Kade", "Kaden", "Kadin", "Kadyn", "Kaeden", "Kael", "Kaelan", "Kaelin", "Kaelum", "Kai", "Kaid", "Kaidan", "Kaiden", "Kaidinn", "Kaidyn", "Kaileb", "Kailin", "Kain", "Kaine", "Kainin", "Kainui", "Kairn", "Kaison", "Kaiwen", "Kajally", "Kajetan", "Kalani", "Kale", "Kaleb", "Kaleem", "Kal-el", "Kalen", "Kalin", "Kallan", "Kallin", "Kalum", "Kalvin", "Kalvyn", "Kameron", "Kames", "Kamil", "Kamran", "Kamron", "Kane", "Karam", "Karamvir", "Karandeep", "Kareem", "Karim", "Karimas", "Karl", "Karol", "Karson", "Karsyn", "Karthikeya", "Kasey", "Kash", "Kashif", "Kasim", "Kasper", "Kasra", "Kavin", "Kayam", "Kaydan", "Kayden", "Kaydin", "Kaydn", "Kaydyn", "Kaydyne", "Kayleb", "Kaylem", "Kaylum", "Kayne", "Kaywan", "Kealan", "Kealon", "Kean", "Keane", "Kearney", "Keatin", "Keaton", "Keavan", "Keayn", "Kedrick", "Keegan", "Keelan", "Keelin", "Keeman", "Keenan"
				}; // 1560 first names

				string[] lastNames = new string[] {"Smith", "Johnson", "Williams", "Brown",
				"Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzales",
				"Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson",
				"White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
				"Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
				"Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter",
				"Roberts", "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards",
				"Collins", "Reyes", "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez",
				"Ortiz", "Morgan", "Cooper", "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos",
				"Kim", "Cox", "Ward", "Richardson", "Watson", "Brooks", "Chavez", "Wood", "James", "Bennet",
				"Gray", "Mendoza", "Ruiz", "Hughes", "Price", "Alvarez", "Castillo", "Sanders", "Patel",
				"Myers", "Long", "Ross", "Foster", "Jimenez", "Collins", "Stewart", "Sanchez", "Morris",
				"Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper",
				"Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray"}; // 120 last names

				List<string> teams = new List<string> {"Arizona", "Atlanta", "Baltimore",
				"Buffalo", "Carolina", "Chicago", "Cincinnati", "Cleveland", "Dallas", "Denver", "Detroit",
				"Houston", "Green Bay", "Indianapolis", "Los Angeles", "Jacksonville", "Minnesota", "Kansas City",
				"New Orleans", "Las Vegas", "New York", "San Diego", "Philadelphia", "Miami", "San Francisco",
				"New England", "Seattle", "New Jersey", "Tampa Bay", "Pittsburgh", "Washington", "Tennessee"}; // 32 teams


				List<string> playerNames = new List<string>();

				Random rand = new Random();
				// Create roster names for 32 teams x ~39 skill position players per team = 1248
				for (int i = 0; i < 1248; i++)
				{
					int firstNamesIndex = rand.Next(firstNames.Length);
					int lastNamesIndex = rand.Next(lastNames.Length);
					playerNames.Add(firstNames[firstNamesIndex] + " " + lastNames[lastNamesIndex]);

					firstNames = firstNames.OrderBy(x => rand.Next()).ToArray();
					lastNames = lastNames.OrderBy(x => rand.Next()).ToArray();
				}

				int numberQBs = 238; // Players generated per position
				int numberRBs = 291;
				int numberWRs = 436;
				int numberTEs = 184;
				int numberKs = 99;
				int numberDs = 32;

				// Generate 238 QBs
				for (int i = 0; i < 238; i++)
				{
					int playerNameIndex = 0;
					int teamIndex = 0;
					for (int j = 0; j < playerNames.Count(); j++)
					{
						playerNameIndex = rand.Next(playerNames.Count());
					}
					for (int k = 0; k < teams.Count(); k++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player qb = new Player();
					qb.LeagueId = league.LeagueId;
					qb.Position = "QB";
					qb.PlayerName = $"{playerNames[playerNameIndex]}";
					qb.Team = $"{teams[teamIndex]}";
					playerNames.RemoveAt(playerNameIndex);

					db.Players.Add(qb);
					db.SaveChanges();
				} // End method

				// Generate 291 RBs
				for (int i = 0; i < 291; i++)
				{
					int playerNameIndex = 0;
					int teamIndex = 0;
					for (int j = 0; j < playerNames.Count(); j++)
					{
						playerNameIndex = rand.Next(playerNames.Count());
					}
					for (int k = 0; k < teams.Count(); k++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player rb = new Player();
					rb.LeagueId = league.LeagueId;
					rb.Position = "RB";
					rb.PlayerName = $"{playerNames[playerNameIndex]}";
					rb.Team = $"{teams[teamIndex]}";
					playerNames.RemoveAt(playerNameIndex);

					db.Players.Add(rb);
					db.SaveChanges();
				} // End method

				// Generate 436 WRs
				for (int i = 0; i < 436; i++)
				{
					int playerNameIndex = 0;
					int teamIndex = 0;
					for (int j = 0; j < playerNames.Count(); j++)
					{
						playerNameIndex = rand.Next(playerNames.Count());
					}
					for (int k = 0; k < teams.Count(); k++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player wr = new Player();
					wr.LeagueId = league.LeagueId;
					wr.Position = "WR";
					wr.PlayerName = $"{playerNames[playerNameIndex]}";
					wr.Team = $"{teams[teamIndex]}";
					playerNames.RemoveAt(playerNameIndex);

					db.Players.Add(wr);
					db.SaveChanges();
				} // End method

				// Generate 184 TEs
				for (int i = 0; i < 184; i++)
				{
					int playerNameIndex = 0;
					int teamIndex = 0;
					for (int j = 0; j < playerNames.Count(); j++)
					{
						playerNameIndex = rand.Next(playerNames.Count());
					}
					for (int k = 0; k < teams.Count(); k++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player te = new Player();
					te.LeagueId = league.LeagueId;
					te.Position = "TE";
					te.PlayerName = $"{playerNames[playerNameIndex]}";
					te.Team = $"{teams[teamIndex]}";
					playerNames.RemoveAt(playerNameIndex);

					db.Players.Add(te);
					db.SaveChanges();
				} // End method

				// Generate 99 Ks
				for (int i = 0; i < 99; i++)
				{
					int playerNameIndex = 0;
					int teamIndex = 0;
					for (int j = 0; j < playerNames.Count(); j++)
					{
						playerNameIndex = rand.Next(playerNames.Count());
					}
					for (int o = 0; o < teams.Count(); o++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player k = new Player();
					k.LeagueId = league.LeagueId;
					k.Position = "K";
					k.PlayerName = $"{playerNames[playerNameIndex]}";
					k.Team = $"{teams[teamIndex]}";
					playerNames.RemoveAt(playerNameIndex);

					db.Players.Add(k);
					db.SaveChanges();
				} // End method

				// Generate 32 Ds
				for (int i = 0; i < 32; i++)
				{
					int teamIndex = 0;

					for (int k = 0; k < teams.Count(); k++)
					{
						teamIndex = rand.Next(teams.Count());
					}
					Player d = new Player();
					d.LeagueId = league.LeagueId;
					d.Position = "D";
					d.PlayerName = $"{teams[teamIndex]}";
					d.Team = $"{teams[teamIndex]}";
					teams.RemoveAt(teamIndex);

					db.Players.Add(d);
					db.SaveChanges();
				} // End method
			} // End if
			else
			{
				Player[] allPlayers = db.Players.Where(p => p.PlayerId <= 1280).ToArray();
				foreach (Player player in allPlayers)
				{
					Player newLeaguePlayer = new Player();
					newLeaguePlayer.LeagueId = league.LeagueId;
					newLeaguePlayer.Position = player.Position;
					newLeaguePlayer.PlayerName = player.PlayerName;
					newLeaguePlayer.Team = player.Team;
					db.Players.Add(newLeaguePlayer);
					db.SaveChanges();
				}
			}


			return Ok();
		} // End method

		[HttpPost("createteam")]
		public IActionResult CreateTeam(CreateTeamDTO dto)
		{
			if (db.Teams.Where(t => t.TeamName == dto.TeamName).Any())
			{
				return Unauthorized("League name already exists, must be unique");
			}

			Team team = new Team();
			team.TeamName = dto.TeamName;
			team.CreatedOnDate= dto.CreatedOnDate;
			team.Creator = dto.Creator;
			team.League = dto.League;
			db.Teams.Add(team);
			db.SaveChanges();


            League_Team league_Team = new League_Team();
            league_Team.UserId = dto.Creator;
            league_Team.User = db.Users.Where(u => u.UserId == dto.Creator).FirstOrDefault();
            league_Team.LeagueId = dto.League;
            league_Team.League = db.Leagues.Where(l => l.LeagueId == dto.League).FirstOrDefault();
            league_Team.TeamId = team.TeamId;
            league_Team.Team = team;

			db.LeagueTeams.Add(league_Team);
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
        } // End method

		[HttpPost("deleteteam")]
		public IActionResult DeleteTeam(DeleteTeamDTO dto)
		{
			Team? team = db.Teams.Where(t => t.TeamId == dto.TeamId).FirstOrDefault();

			db.Teams.Remove(team);
			db.SaveChanges();
			return Ok();
		} // End method


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
