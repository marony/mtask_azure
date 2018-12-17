using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DataModel;
using mtask.Models.DomainModel;

namespace mtask.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        public User GetUser(string id)
        {
            var table = DataAccessUtil.GetTable("users");
            var entity = DataAccessUtil.Retrieve<UserEntity>(table, "user_" + id).FirstOrDefault();

            return entity?.GetObject();
        }

        public User GetUserByName(string userName)
        {
            var table = DataAccessUtil.GetTable("users");
            var entities = DataAccessUtil.Retrieve<UserEntity>(table);
            return entities.Select(entity => entity.GetObject()).FirstOrDefault(user => user.UserName == userName);
        }

        public User GetUserByEmail(string email)
        {
            var table = DataAccessUtil.GetTable("users");
            var entities = DataAccessUtil.Retrieve<UserEntity>(table);
            return entities.Select(entity => entity.GetObject()).FirstOrDefault(user => user.Email == email);
        }

        public void InsertUser(User user)
        {
            user.InsertedAt = user.UpdatedAt = DateTime.Now;
            UpdateUser(user);
        }

        public void UpdateUser(User user)
        {
            user.UpdatedAt = DateTime.Now;
            var entity = new UserEntity(user);
            var table = DataAccessUtil.GetTable("users");
            DataAccessUtil.InsertOrReplace(table, entity);
        }
    }
}
