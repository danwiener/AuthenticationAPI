using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Authentication.Data;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("user_name")]
        public string Username { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        [JsonIgnore]
        public string Password { get; set; } = default!;

        public List<User_League> User_Leagues { get; set; }


        //public List<Team> TeamsOwned { get; set; } = default;

    }
}
