using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class GetLeagueIdBelongedTo
	{
		[JsonPropertyName("leaguesbelongedto")]
		public int[] LeaguesBelongedTo { get; set; }

		public GetLeagueIdBelongedTo(int[] leagueids)
		{
			this.LeaguesBelongedTo= leagueids;
		}
	}
}
