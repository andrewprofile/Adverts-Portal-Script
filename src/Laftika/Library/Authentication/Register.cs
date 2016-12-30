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
        private string username;
        private string password;
        private string email;
        private UserRepository userRepository = new UserRepository(new DatabaseContext());

        public async Task<bool> CreateAccount(string username, string password, string email)
        {
            this.username = username;
            this.password = password;
            this.email = email;

            if (GetNumbersAccounts())
            {
                await AddAccountToDatabase();

                return true;
            }

            return false;
        }

        public async Task<bool> AddAccountToDatabase()
        {
            string securedPassword = Secure.HashString(password);

            userRepository.InsertUser(new User { Username = username, Password = securedPassword, Email = email });
            await userRepository.Save();

            return true;
        }

        public bool GetNumbersAccounts()
        {
            var isExists = (from a in userRepository.GetUsers()
                            where a.Email == email || a.Username == username
                            select a).Count();

            return isExists == 0;
        }
    }
}
