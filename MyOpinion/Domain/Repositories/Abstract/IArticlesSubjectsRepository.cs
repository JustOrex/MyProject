using Microsoft.AspNetCore.Identity;
using MyOpinion.Domain.Entities;

namespace MyOpinion.Domain.Repositories.Abstract
{
    
        public interface IArticlesSubjectsRepository
        {
            IQueryable<IdentityUserRole<string>> GetSubjects();

            IQueryable<Article> GetArticlesBySubjects(string subjectId);

            //IdentityUserRole<string> GetUserRoleByRoleId(string userId);

            void SaveArticleSubject(IdentityUserRole<string> entity);
        }
    
}
