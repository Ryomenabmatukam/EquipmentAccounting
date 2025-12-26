using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class EquipmentTypeService
    {
        public List<EquipmentType> GetAll()
        {
            using (var db = new DLADbContext())
                return db.EquipmentTypes.AsNoTracking().OrderBy(x => x.Name).ToList();
        }

        public void Add(EquipmentType t)
        {
            if (string.IsNullOrWhiteSpace(t?.Name)) throw new System.Exception("Название не заполнено");
            using (var db = new DLADbContext())
            {
                db.EquipmentTypes.Add(t);
                db.SaveChanges();
            }
        }

        public void Update(EquipmentType t)
        {
            if (string.IsNullOrWhiteSpace(t?.Name)) throw new System.Exception("Название не заполнено");
            using (var db = new DLADbContext())
            {
                db.EquipmentTypes.Update(t);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.EquipmentTypes.FirstOrDefault(x => x.Id == id);
                if (entity == null) return;
                db.EquipmentTypes.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
