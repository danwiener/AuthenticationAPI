using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class NumberTeamsDTO
	{
		[JsonPropertyName("numberteams")]
		public int? NumberTeams { get; set; }

		public NumberTeamsDTO(int numberteams)
		{
			this.NumberTeams = numberteams;
		}
	}
}
