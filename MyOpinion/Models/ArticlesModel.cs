using Microsoft.AspNetCore.Mvc;
using MyOpinion.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyOpinion.Models
{
    public class ArticlesModel
    {
        
        public string? text { get; set; }

        [Display(Name = "Предмет")]
        public string? Subject { get; set; }


        [HiddenInput(DisplayValue = false)]
        public IQueryable<Article> articles { get; set; }

        public bool ShowAuthor = false;
    }
}
