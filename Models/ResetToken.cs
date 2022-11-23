using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class ResetToken
    {
        [Key()]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
