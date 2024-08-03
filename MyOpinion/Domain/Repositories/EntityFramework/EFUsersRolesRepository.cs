using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain.Repositories.EntityFramework
{
    public class EFUsersRolesRepository : IUsersRolesRepository
    {
        private readonly AppDbContext context;
        public EFUsersRolesRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IdentityUserRole<string> GetUserRoleByUserId(string userId)
        {
            return context.UserRoles.FirstOrDefault(x => x.UserId == userId);
        }

        public IdentityUserRole<string> GetUserRoleByRoleId(string roleId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IdentityUserRole<string>> GetUsersRoles()
        {
            throw new NotImplementedException();
        }

        public void SaveUserRole(IdentityUserRole<string> entity)
        {
            throw new NotImplementedException();
        }
    }
}
