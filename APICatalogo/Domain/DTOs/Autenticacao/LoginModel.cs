using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Domain.DTOs.Autenticacao
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo UserName é obrigatório.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string? Password { get; set; }
    }
}
