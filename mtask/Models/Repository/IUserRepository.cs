using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.Repository
{
    public interface IUserRepository : IRepository
    {
        User GetUser(string id);
        User GetUserByName(string userName);
        User GetUserByEmail(string email);
        void UpdateUser(User user);
    }
}