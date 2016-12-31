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
        private readonly IUserRepository _userRepository;

        public Login(HttpContext context, IUserRepository userRepository)
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
                count = _userRepository.CountBySessionKey(_context.Session.GetString("SessionKey"));
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

            var user = _userRepository.GetByUsername(_username);

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
            var user = _userRepository.GetBySessionKey(_context.Session.GetString("SessionKey"));

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
            return _userRepository.CountByUsernameAndPassword(_username, Secure.HashString(_password)) == 1;
        }

        public User GetUserObject()
        {
            return _userRepository.GetBySessionKey(_context.Session.GetString("SessionKey"));
        }
    }
}
