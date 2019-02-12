using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PersonelFollow.WebUI.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string guId, string userInformation)
        {
            Session.SetString("user", JsonConvert.SerializeObject(guId));
            Session.SetString("userInformation",JsonConvert.SerializeObject(userInformation));
        }

        public String GetSession()
        {
            var session = Session.GetString("user");
            return (string)(session != null ? JsonConvert.DeserializeObject(session) : null);
        }

        public string GetSessionUserInformation()
        {
            var session = Session.GetString("userInformation");
            return (string)(session != null ? JsonConvert.DeserializeObject(session) : null);
        }

        public void DeleteSession()
        {
            Session.Clear();
        }
    }
}
