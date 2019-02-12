using System;
using System.Collections.Generic;
using System.Text;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Abstract
{
    public interface IUserRepository
    {
        String GetUser(String eMail, String password);

        List<UserInformation> GetAllUser();
        UserInformation GetUserInformation(String id);

        bool Add(UserInformation user);

        bool Update(UserInformation user);

        bool IsSave(UserInformation eMail);

        bool IsSave(Guid userId, String eMail);
    }
}
