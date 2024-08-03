using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using CKSource.CKFinder.Connector.Core.Authentication;
using CKSource.CKFinder.Connector.Core;

namespace MyOpinion.Services
{
    public class UserAreaAuthorization : IControllerModelConvention, IAuthenticator
    {
        private readonly string area;
        private readonly string policy;

        public UserAreaAuthorization(string area, string policy)
        {
            this.area = area;
            this.policy = policy;
        }
        public void Apply(ControllerModel controller)
        {
            if (controller.Attributes.Any(a =>
                     a is AreaAttribute && (a as AreaAttribute).RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase))
                 || controller.RouteValues.Any(r =>
                     r.Key.Equals("area", StringComparison.OrdinalIgnoreCase) && r.Value.Equals(area, StringComparison.OrdinalIgnoreCase)))
            {
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }

        public Task<IUser> AuthenticateAsync(ICommandRequest commandRequest, CancellationToken cancellationToken)
        {
            var user = new User(true, null);
            return Task.FromResult((IUser)user);
        }
    }
}
