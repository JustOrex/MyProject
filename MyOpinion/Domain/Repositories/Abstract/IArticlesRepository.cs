using System;
using System.Linq;
using MyOpinion.Domain.Entities;

namespace MyOpinion.Domain.Repositories.Abstract
{
    public interface IArticlesRepository
    {
        IQueryable<Article> GetArticles();
        IQueryable<Article> GetArticlesByAuthorsId(Guid id);
        Article GetArticleById(Guid id);
        void SaveArticle(Article entity);
        void DeleteArticle(Guid id);

    }
}
