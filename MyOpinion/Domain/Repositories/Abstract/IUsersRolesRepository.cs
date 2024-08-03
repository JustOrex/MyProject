using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyOpinion.Domain.Repositories.Abstract
{
    public interface IUsersRolesRepository
    {
        IQueryable<IdentityUserRole<string>> GetUsersRoles();

        IdentityUserRole<string> GetUserRoleByUserId(string roleId);

        IdentityUserRole<string> GetUserRoleByRoleId(string userId);

        void SaveUserRole(IdentityUserRole<string> entity);
    }
}
