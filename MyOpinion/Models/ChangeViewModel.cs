using System.ComponentModel.DataAnnotations;
using MyOpinion.Services;

namespace MyOpinion.Models
{
    public class ChangeViewModel
    {

        
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        
        [UIHint("password")]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [UIHint("password")]
        [Display(Name = "Новый пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен иметь длину больше 6 символов")]
        public string NewPassword { get; set; }

        [UIHint("password")]
        [Display(Name = "Подтвердите Пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "Введите почту")]
        //public string Email { get; set; }

        
    }
}
