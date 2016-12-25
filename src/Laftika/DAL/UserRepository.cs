using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Laftika.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.DAL
{
    public class UserRepository
    {
        private DatabaseContext db;

        public UserRepository(DatabaseContext db)
        {
            this.db = db;
        }

        public User GetUserById(int id)
        {
            var user = db.Users.Find(id);

            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return db.Users.ToList();
        }

        public void InsertUser(User user)
        {
            db.Users.Add(user);
        }

        public void DeleteUser(int userId)
        {
            User user = db.Users.Find(userId);
            db.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            await db.SaveChangesAsync();

            return true;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
