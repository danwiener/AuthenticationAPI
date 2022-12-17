using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class GetPlayerIdDTO
	{
		[JsonPropertyName("playeridinleague")]
		public int[] PlayerIdsInLeague { get; set; }

		public GetPlayerIdDTO(int[] playerids)
		{
			this.PlayerIdsInLeague = playerids;
		}
	}
}
