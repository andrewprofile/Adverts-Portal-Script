using System.Linq;
using System.Threading.Tasks;
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
        private readonly IUserRepository _userRepository;

        public Register(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateAccount(string username, string password, string email)
        {
            _username = username;
            _password = password;
            _email = email;

            if (!GetNumbersAccounts())
            {
                return false;
            }

            return await AddAccountToDatabase();
        }

        public async Task<bool> AddAccountToDatabase()
        {
            string securedPassword = Secure.HashString(_password);

            _userRepository.Insert(new User { Username = _username, Password = securedPassword, Email = _email });

            return await _userRepository.Save();
        }

        public bool GetNumbersAccounts()
        {
            return _userRepository.CountByUsernameOrEmail(_username, _email) == 0;
        }
    }
}
