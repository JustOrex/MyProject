using System.ComponentModel.DataAnnotations;

namespace MyOpinion.Domain.Entities
{
    public class Article : EntityBase
    {
        [Required(ErrorMessage = "Заполните название статьи")]
        [Display(Name = "Название статьи")]
        public override string Title { get; set; }

        [Display(Name = "Дисциплины")]
        public override string? Categories { get; set; }

        [Display(Name = "Содержание статьи")]
        public override string Text { get; set; }

        public string? AdditionalTitleImagesPath { get; set; }

        [Display(Name = "Предмет")]
        public string Subject { get; set; }
        
        public Guid AuthorsId {get; set;}

        [Display(Name = "Дополнительные картинки")]
        public string[] AdditImagPathes = new string[10];

        public string ImToDel = null;
    }
}
