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
    public class AdvertRepository
    {
        private DatabaseContext db;

        public AdvertRepository(DatabaseContext db)
        {
            this.db = db;
        }

        public Advert GetAdvertById(int id)
        {
            var advert = db.Adverts.Find(id);

            return advert;
        }

        public IEnumerable<Advert> GetAdverts()
        {
            return db.Adverts.ToList();
        }

        public void InsertAdvert(Advert advert)
        {
            db.Adverts.Add(advert);
        }

        public void DeleteUser(int advertId)
        {
            Advert advert = db.Adverts.Find(advertId);
            db.Adverts.Remove(advert);
        }

        public void UpdateAdvert(Advert advert)
        {
            db.Entry(advert).State = EntityState.Modified;
        }

        public async Task<bool> Save()
        {
            await db.SaveChangesAsync();

            return true;
        }
    }
}
