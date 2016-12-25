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
        private string _username;
        private string _password;
        private HttpContext _context;
        UserRepository _userRepository = new UserRepository(new DatabaseContext());

        public Login(HttpContext context)
        {
            _context = context;
        }

        public bool CreateAuthentication(string username, string password)
        {
            _username = username;
            _password = password;

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
            if (!string.IsNullOrEmpty(_context.Session.GetString("SessionKey")))
            {
                count = (from a in _userRepository.GetUsers()
                         where a.SessionKey == _context.Session.GetString("SessionKey")
                         select a).Count();
            }

            return count != 0;
        }

        public void DestroyAuthentication()
        {
            DestroySessionKey();
            _context.Session.Remove("SessionKey");
        }

        public async Task<bool> CreateSessionKey()
        {
            string sessionKey = Secure.CreateKey();

            var user = (from a in _userRepository.GetUsers()
                        where a.Username == _username
                        select a).FirstOrDefault();

            if (user != null)
            {
                user.SessionKey = sessionKey;

                _userRepository.UpdateUser(user);
                await _userRepository.Save();
            }

            _context.Session.SetString("SessionKey", sessionKey);

            return true;
        }

        public async Task<bool> DestroySessionKey()
        {
            var user = (from a in _userRepository.GetUsers()
                        where a.SessionKey == _context.Session.GetString("SessionKey")
                        select a).FirstOrDefault();

            if (user != null)
            {
                user.SessionKey = null;
                _userRepository.UpdateUser(user);
                await _userRepository.Save();
            }

            return true;
        }

        public bool CheckExistsAccount()
        {
            string securedPassword = Secure.HashString(_password);

            var numberAccounts = (from a in _userRepository.GetUsers()
                                  where a.Username == _username && a.Password == securedPassword
                                  select a).Count();

            return numberAccounts == 1;
        }

        public User GetUserObject()
        {
            var user = (from a in _userRepository.GetUsers()
                        where a.SessionKey == _context.Session.GetString("SessionKey")
                        select a).FirstOrDefault();

            return user;
        }
    }
}
