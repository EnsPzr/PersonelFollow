using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonelFollow.Core.Abstract;
using PersonelFollow.Entities.Concrete;

namespace PersonelFollow.Core.Concrete.EntityFramework
{
    public class EfUserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public EfUserRepository(DataContext context)
        {
            this._context = context;
        }

        public String GetUser(string eMail, string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            var sha1Password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
            var user = _context.UserInformations.FirstOrDefault(p => p.EMail == eMail && p.Password == sha1Password);
            return user != null ? user.UserId.ToString() : "";
        }

        public List<UserInformation> GetAllUser()
        {
            return _context.UserInformations.ToList();
        }

        public UserInformation GetUserInformation(String id)
        {
            var user = _context.UserInformations.FirstOrDefault(p => p.UserId.ToString() == id);
            return user;
        }

        public bool Add(UserInformation user)
        {
            if (IsSave(user))
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                var sha1Password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
                user.UserRegisterDate = DateTime.Today;
                user.Password = sha1Password;
                user.isAdministrator = false;
                var addedUser = _context.Entry(user);
                addedUser.State = EntityState.Added;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Update(UserInformation user)
        {
            if (IsSave(user.UserId, user.EMail))
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                var sha1Password = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
                var updatedUser = _context.UserInformations.FirstOrDefault(p=>p.UserId==user.UserId);
                if (updatedUser != null)
                {
                    updatedUser.Password = sha1Password;
                    updatedUser.UserName = user.UserName;
                    updatedUser.UserSurname = user.UserSurname;
                    updatedUser.EMail = user.EMail;
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool IsSave(Guid userId, string eMail)
        {
            if (_context.UserInformations.FirstOrDefault(p => p.UserId != userId && p.EMail == eMail) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsSave(UserInformation user)
        {
            if (_context.UserInformations.FirstOrDefault(p => p.EMail == user.EMail) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
