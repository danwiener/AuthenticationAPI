using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class GetTeamIdBelongedTo
	{
		[JsonPropertyName("teamsbelongedto")]
		public int[] TeamsBelongedTo { get; set; }

		public GetTeamIdBelongedTo(int[] teamids)
		{
			this.TeamsBelongedTo = teamids;
		}
	}
}
