using Microsoft.AspNetCore.Identity;
using MyOpinion.Domain.Entities;

namespace MyOpinion.Domain.Repositories.Abstract
{
    public interface ISubjectsRepository
    {
        IQueryable<Subject> GetSubjects();
        public Subject GetSubjectById(string id);
        
    }
}
