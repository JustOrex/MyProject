using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain.Repositories.EntityFramework
{
    public class EFUsersRepository : IUsersRepository
    {
        private readonly AppDbContext context;

        public EFUsersRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void SaveUser(IdentityUser entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
        }

        public IdentityUser GetUserById(string id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }

        public IdentityUser GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.Email == email);
        }

        public IdentityUser GetUserByName(string name)
        {
            return context.Users.FirstOrDefault(x => x.UserName == name);
        }

        public IQueryable<IdentityUser> GetUsers()
        {
            return context.Users;
        }

        
    }
}
