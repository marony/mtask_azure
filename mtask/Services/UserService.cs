using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using mtask.Models;
using mtask.Models.DomainModel;
using mtask.Models.Repository;
using mtask.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;

namespace mtask.Services
{
    public class UserService : IUserService
    {
        protected readonly IUserRepository UserRepository = null;

        public UserService()
        {
            this.UserRepository = new UserRepository();
        }

        [InjectionConstructor]
        public UserService(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        public User GetCurrentUser(IPrincipal user)
        {
            var userId = user.Identity.GetUserId();
            if (userId == null)
                return null;
            return UserRepository.GetUser(userId);
        }

        public void UpdateUser(User user)
        {
            UserRepository.UpdateUser(user);
        }

        public async System.Threading.Tasks.Task<SignInStatus> Logon(SignInManager<User, string> signInManager, ModelStateDictionary modelState, TempDataDictionary tempDate, LogonViewModel model)
        {
            Logoff(signInManager);

            var status = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (status)
            {
            default:
                throw new ArgumentException($"UserService.Login: status = {status}");
            case SignInStatus.Success:
                break;
            //case SignInStatus.LockedOut:
            //    break;
            //case SignInStatus.RequiresVerification:
            //    break;
            case SignInStatus.Failure:
                modelState.AddModelError("", "電子メールかパスワードが間違っています。");
                break;
            }

            return status;
        }

        public void Logoff(SignInManager<User, string> signInManager)
        {
            signInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}