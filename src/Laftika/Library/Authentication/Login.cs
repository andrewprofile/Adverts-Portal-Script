using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laftika.Models;
using Microsoft.AspNetCore.Http;
using Laftika.DAL;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Library.Authentication
{
    public class Login
    {
        private string username;
        private string password;
        private HttpContext context;
        UserRepository userRepository = new UserRepository(new DatabaseContext());

        public Login(HttpContext context)
        {
            context = context;
        }

        public bool CreateAuthentication(string username, string password)
        {
            username = username;
            password = password;

            if (CheckExistsAccount())
            {
                CreateSessionKey();
                return true;
            }
            return false;
        }

        public bool CheckAuthentication()
        {
            int count = 0;
            if (!string.IsNullOrEmpty(context.Session.GetString("SessionKey")))
            {
                count = (from a in userRepository.GetUsers()
                         where a.SessionKey == context.Session.GetString("SessionKey")
                         select a).Count();
            }

            return count != 0;
        }

        public void DestroyAuthentication()
        {
            DestroySessionKey();
            context.Session.Remove("SessionKey");
        }

        public async Task<bool> CreateSessionKey()
        {
            string sessionKey = Secure.CreateKey();

            var user = (from a in userRepository.GetUsers()
                        where a.Username == username
                        select a).FirstOrDefault();

            if (user != null)
            {
                user.SessionKey = sessionKey;

                userRepository.UpdateUser(user);
                await userRepository.Save();
            }

            context.Session.SetString("SessionKey", sessionKey);

            return true;
        }

        public async Task<bool> DestroySessionKey()
        {
            var user = (from a in userRepository.GetUsers()
                        where a.SessionKey == context.Session.GetString("SessionKey")
                        select a).FirstOrDefault();

            if (user != null)
            {
                user.SessionKey = null;
                userRepository.UpdateUser(user);
                await userRepository.Save();
            }

            return true;
        }

        public bool CheckExistsAccount()
        {
            string securedPassword = Secure.HashString(password);

            var numberAccounts = (from a in userRepository.GetUsers()
                                  where a.Username == username && a.Password == securedPassword
                                  select a).Count();

            return numberAccounts == 1;
        }

        public User GetUserObject()
        {
            var user = (from a in userRepository.GetUsers()
                        where a.SessionKey == context.Session.GetString("SessionKey")
                        select a).FirstOrDefault();

            return user;
        }
    }
}
