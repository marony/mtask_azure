using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using mtask.Models.DomainModel;
using mtask.Models.ViewModel;
using Microsoft.AspNet.Identity.Owin;

namespace mtask.Services
{
    public interface IUserService : IService
    {
        User GetCurrentUser(IPrincipal user);
        void UpdateUser(User user);
        System.Threading.Tasks.Task<SignInStatus> Logon(SignInManager<User, string> signInManager, ModelStateDictionary modelState, TempDataDictionary tempDate, LogonViewModel model);
        void Logoff(SignInManager<User, string> signInManager);
    }
}