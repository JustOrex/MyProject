using System.ComponentModel.DataAnnotations;

namespace MyOpinion.Models
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [Display(Name = "Введите код отправленный вам на почту")]
        public string Code { get; set; }
    }
}
