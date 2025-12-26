using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class LicenseService
    {
        public List<SoftwareLicense> GetAll()
        {
            using (var db = new DLADbContext())
                return db.SoftwareLicenses.AsNoTracking().OrderBy(x => x.SoftwareName).ToList();
        }

        public void Add(SoftwareLicense l)
        {
            Validate(l);
            using (var db = new DLADbContext())
            {
                db.SoftwareLicenses.Add(l);
                db.SaveChanges();
            }
        }

        public void Update(SoftwareLicense l)
        {
            Validate(l);
            using (var db = new DLADbContext())
            {
                db.SoftwareLicenses.Update(l);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.SoftwareLicenses.FirstOrDefault(x => x.Id == id);
                if (entity == null) return;
                db.SoftwareLicenses.Remove(entity);
                db.SaveChanges();
            }
        }

        private void Validate(SoftwareLicense l)
        {
            if (l == null) throw new System.Exception("Пустые данные");
            if (string.IsNullOrWhiteSpace(l.SoftwareName)) throw new System.Exception("Название ПО не заполнено");
            if (string.IsNullOrWhiteSpace(l.Manufacturer)) throw new System.Exception("Производитель не заполнен");
            if (string.IsNullOrWhiteSpace(l.LicenseKey)) throw new System.Exception("Ключ лицензии не заполнен");
        }
    }
}
