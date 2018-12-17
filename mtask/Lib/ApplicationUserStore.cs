using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;
using mtask.Models.Repository;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace mtask {
    /// <summary>
    /// 認証用ユーザストア(Microsoft Identity)
    /// </summary>
    public class ApplicationUserStore
        : IUserStore<User>, IUserPasswordStore<User>, IUserLockoutStore<User, string>,
          IUserTwoFactorStore<User, string>, IUserEmailStore<User, string>
    {
        protected readonly IUserRepository userRepository = null;

        [InjectionConstructor]
        public ApplicationUserStore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public System.Threading.Tasks.Task CreateAsync(User user)
        {
            userRepository.UpdateUser(user);
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task UpdateAsync(User user)
        {
            userRepository.UpdateUser(user);
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task<User> FindByIdAsync(string userId)
        {
            var user = userRepository.GetUser(userId);
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public System.Threading.Tasks.Task<User> FindByNameAsync(string userName)
        {
            var user = userRepository.GetUserByName(userName);
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public System.Threading.Tasks.Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task<string> GetPasswordHashAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(user.PasswordHash);
        }

        public System.Threading.Tasks.Task<bool> HasPasswordAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public void Dispose()
        {
            Console.WriteLine("ApplicationUserStore.Dispose");
        }

        public System.Threading.Tasks.Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> IncrementAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task ResetAccessFailedCountAsync(User user)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<int> GetAccessFailedCountAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(0);
        }

        public System.Threading.Tasks.Task<bool> GetLockoutEnabledAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(false);
        }

        public System.Threading.Tasks.Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(false);
        }

        public System.Threading.Tasks.Task SetEmailAsync(User user, string email)
        {
            user.Email = email;
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task<string> GetEmailAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(user.Email);
        }

        public System.Threading.Tasks.Task<bool> GetEmailConfirmedAsync(User user)
        {
            return System.Threading.Tasks.Task.FromResult(user.EmailConfirmed);
        }

        public System.Threading.Tasks.Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return System.Threading.Tasks.Task.FromResult(default(object));
        }

        public System.Threading.Tasks.Task<User> FindByEmailAsync(string email)
        {
            var user = userRepository.GetUserByEmail(email);
            return System.Threading.Tasks.Task.FromResult(user);
        }
    }
}