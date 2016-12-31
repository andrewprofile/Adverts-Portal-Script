using System.Linq;
using Laftika.Models;
using Microsoft.EntityFrameworkCore;

namespace Laftika.DAL
{
    public interface IUserRepository : IGenericRepository<User>
    {
        int  CountByUsernameAndPassword(string username, string password);
        int  CountByUsernameOrEmail(string username, string email);
        int  CountBySessionKey(string sessionKey);
        User GetByUsername(string username);
        User GetBySessionKey(string sessionKey);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public int CountByUsernameAndPassword(string username, string password) => (from a in GetAll()
            where a.Username == username && a.Password == password
            select a).Count();

        public int CountByUsernameOrEmail(string username, string email) => (from a in GetAll()
            where a.Email == email || a.Username == username
            select a).Count();

        public int CountBySessionKey(string sessionKey) => (from a in GetAll()
            where a.SessionKey == sessionKey
            select a).Count();

        public User GetByUsername(string username) => (from a in GetAll()
            where a.Username == username
            select a).FirstOrDefault();

        public User GetBySessionKey(string sessionKey) => (from a in GetAll()
            where a.SessionKey == sessionKey
            select a).FirstOrDefault();
    }
}