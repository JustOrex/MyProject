using Microsoft.AspNetCore.Identity;
using MyOpinion.Domain.Entities;

namespace MyOpinion.Domain.Repositories.Abstract
{
    public interface IUsersRepository
    {
        IQueryable<IdentityUser>GetUsers();

        IdentityUser GetUserByName(string name);

        IdentityUser GetUserByEmail(string email);

        IdentityUser GetUserById(string id);

        void SaveUser(IdentityUser entity);

        
    }
}
