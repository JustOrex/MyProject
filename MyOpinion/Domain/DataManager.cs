using Microsoft.Extensions.Caching.Memory;
using MyOpinion.Domain.Repositories.Abstract;

namespace MyOpinion.Domain
{
    public class DataManager
    {
        public ITextFieldsRepository TextFields { get; set; }
        public IArticlesRepository Articles { get; set; }
        public IUsersRepository Users { get; set; }
        public IUsersRolesRepository UsersRoles { get; set; }

        public ISubjectsRepository Subjects { get; set; }
        public IRolesRepository Roles { get; set; }

        public IMemoryCache memoryCache { get;set; }

        public DataManager(ITextFieldsRepository textFieldsRepository, IArticlesRepository serviceItemsRepository, IUsersRepository usersRepository, IUsersRolesRepository usersRoles, IRolesRepository roles, ISubjectsRepository subjects)
        {
            TextFields = textFieldsRepository;
            Articles = serviceItemsRepository;
            Users = usersRepository;
            UsersRoles = usersRoles;
            Roles = roles;
            Subjects = subjects;
        }
    }
}
