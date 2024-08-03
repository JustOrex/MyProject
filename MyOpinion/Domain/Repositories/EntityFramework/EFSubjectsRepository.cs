using MyOpinion.Domain.Entities;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain.Repositories.EntityFramework
{
    public class EFSubjectsRepository : ISubjectsRepository
    {
        private readonly AppDbContext context;

        public EFSubjectsRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Subject> GetSubjects()
        {
            return context.Subjects;
        }
        public Subject GetSubjectById(string id)
        {
            return context.Subjects.FirstOrDefault(x=> x.Id==id);
        }
    }
}
