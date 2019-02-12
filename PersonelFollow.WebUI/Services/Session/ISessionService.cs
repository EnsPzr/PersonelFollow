using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonelFollow.WebUI.Services.Session
{
    public interface ISessionService
    {
        void SetSession(string guId, string userInformation);

        String GetSession();

        String GetSessionUserInformation();

        void DeleteSession();
    }
}
