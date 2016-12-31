using System.Linq;
using System.Threading.Tasks;
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
        private readonly HttpContext _context;
        private readonly IGenericRepository<User> _userRepository;

        public Login(HttpContext context, IGenericRepository<User> userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<bool> CreateAuthentication(string username, string password)
        {
            _username = username;
            _password = password;

            if (!CheckExistsAccount())
            {
                return false;
            }

            return await CreateSessionKey();
        }

        public bool CheckAuthentication()
        {
            int count = 0;
            if (!string.IsNullOrEmpty(_context.Session.GetString("SessionKey")))
            {
                count = (from a in _userRepository.GetAll()
                         where a.SessionKey == _context.Session.GetString("SessionKey")
                         select a).Count();
            }

            return count != 0;
        }

        public async Task DestroyAuthentication()
        {
            await DestroySessionKey();
            _context.Session.Remove("SessionKey");
        }

        public async Task<bool> CreateSessionKey()
        {
            string sessionKey = Secure.CreateKey();

            var user = (from a in _userRepository.GetAll()
                        where a.Username == _username
                        select a).FirstOrDefault();

            if (user != null)
            {
                user.SessionKey = sessionKey;

                _userRepository.Update(user);
                await _userRepository.Save();
            }

            _context.Session.SetString("SessionKey", sessionKey);

            return true;
        }

        public async Task<bool> DestroySessionKey()
        {
            var user = (from a in _userRepository.GetAll()
                        where a.SessionKey == _context.Session.GetString("SessionKey")
                        select a).FirstOrDefault();

            if (user == null)
            {
                return true;
            }

            user.SessionKey = null;
            _userRepository.Update(user);

            return await _userRepository.Save();
        }

        public bool CheckExistsAccount()
        {
            string securedPassword = Secure.HashString(_password);

            var numberAccounts = (from a in _userRepository.GetAll()
                                  where a.Username == _username && a.Password == securedPassword
                                  select a).Count();

            return numberAccounts == 1;
        }

        public User GetUserObject()
        {
            return (from a in _userRepository.GetAll()
                where a.SessionKey == _context.Session.GetString("SessionKey")
                select a).FirstOrDefault();
        }
    }
}
