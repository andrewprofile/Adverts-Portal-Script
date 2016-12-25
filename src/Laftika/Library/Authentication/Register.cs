using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Models;
using Laftika.DAL;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Library.Authentication
{
    public class Register
    {
        private string _username;
        private string _password;
        private string _email;
        private int _referral;
        UserRepository _userRepository = new UserRepository(new DatabaseContext());

        public async Task<bool> CreateAccount(string username, string password, string email, int referral)
        {
            _username = username;
            _password = password;
            _email = email;
            _referral = referral;

            if (GetNumbersAccounts())
            {
                await AddAccountToDatabase();

                return true;
            }

            return false;
        }

        public async Task<bool> AddAccountToDatabase()
        {
            string securedPassword = Secure.HashString(_password);

            _userRepository.InsertUser(new User { Username = _username, Password = securedPassword, Email = _email });
            await _userRepository.Save();

            return true;
        }

        public bool GetNumbersAccounts()
        {
            var isExists = (from a in _userRepository.GetUsers()
                            where a.Email == _email || a.Username == _username
                            select a).Count();

            return isExists == 0;
        }
    }
}
