using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyOpinion.Domain.Entities;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain.Repositories.EntityFramework
{
    public class EFArticlesRepository : IArticlesRepository
    {
        private readonly AppDbContext context;
        public EFArticlesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Article> GetArticles()
        {
            return context.Articles;
        }

        public Article GetArticleById(Guid id)
        {
            return context.Articles.FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<Article> GetArticlesByAuthorsId(Guid id)
        {

            return context.Articles.Where(x => x.AuthorsId == id);
        
        }

        public void SaveArticle(Article entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteArticle(Guid id)
        {
            context.Articles.Remove(new Article() { Id = id });
            context.SaveChanges();
        }
    }
}
