using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using mtask.Models.DomainModel;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace mtask.Models.DataModel
{
    public class UserEntity : TableEntity
    {
        public UserEntity()
        {
        }

        public UserEntity(User user)
        {
            this.PartitionKey = "user_" + user.Id;
            this.RowKey = "user_1";
            // Binary
            //var ms = new MemoryStream();
            //new BinaryFormatter().Serialize(ms, user);
            //this.Bytes = Convert.ToBase64String(ms.ToArray());
            // JSON
            this.Bytes = JsonConvert.SerializeObject(user);
        }

        public User GetObject()
        {
            if (Bytes != null)
            {
                // Binary
                //var ms = new MemoryStream(Convert.FromBase64String(Bytes));
                //ms.Seek(0, SeekOrigin.Begin);
                //object o = new BinaryFormatter().Deserialize(ms);
                //return o as User;
                // JSON
                var user = JsonConvert.DeserializeObject<User>(this.Bytes);
                user.DeserializedSetup();
                return user;
            }
            return null;
        }

        public string Bytes { get; set; }

    }
}