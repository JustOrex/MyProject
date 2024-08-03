using Microsoft.AspNetCore.Identity;
using MyOpinion.Domain.Entities;

namespace MyOpinion.Domain.Repositories.Abstract
{
    public interface IRolesRepository
    {
        public Task<IdentityRole> GetRoleByName(string roleName);
    }
}
