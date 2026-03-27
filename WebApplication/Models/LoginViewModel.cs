using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // "Beni Hatırlat" seçeneği (Identity bunu hazır destekler)
        public bool RememberMe { get; set; }
    }
}
