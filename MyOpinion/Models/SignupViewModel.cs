using System.ComponentModel.DataAnnotations;

namespace MyOpinion.Models
{
    public class SignupViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string UserName { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину больше 6 символов")]
        public string Password { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[Display(Name = "Введите почту")]
        //public string Email { get; set; }
    }
}
