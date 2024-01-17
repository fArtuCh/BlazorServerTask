using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalServices;

public static class Helper
{

    public static class User
    {
        // Static method to generate a list of users
        public static List<ModelUser> GenerateUsers()
        {
            var users = new List<ModelUser>
        {
            new ModelUser { Id = Guid.NewGuid() , Name = "Adam", Surname = "Kowalski", Location = "Radom", ProfilePictureLocation = "/images/User3.png", UserGroup = EnumUserGroup.None },
            new ModelUser {Id = Guid.NewGuid() , Name = "Piotr", Surname = "Piotrowski", Location = "Radom", ProfilePictureLocation = "/images/User2.png", UserGroup = EnumUserGroup.None },
            new ModelUser {Id = Guid.NewGuid() , Name = "Magda", Surname = "Nowakowska", Location = "Warszawa", ProfilePictureLocation = "/images/User1.png", UserGroup = EnumUserGroup.None },
            new ModelUser { Id = Guid.NewGuid() ,Name = "Jan", Surname = "Nowak", Location = "Radom", ProfilePictureLocation = "/images/User4.png", UserGroup = EnumUserGroup.Grupa1 },
        };

            return users;
        }


    }






}
