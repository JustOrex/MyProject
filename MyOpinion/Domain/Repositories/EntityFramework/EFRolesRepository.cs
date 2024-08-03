using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain.Repositories.EntityFramework
{
    public class EFRolesRepository : IRolesRepository
    {
        private readonly AppDbContext context;

        public EFRolesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task<IdentityRole> GetRoleByName(string roleName)
        {
           return context.Roles.FirstOrDefaultAsync(x=> x.Name == roleName);
        }
    }
}
