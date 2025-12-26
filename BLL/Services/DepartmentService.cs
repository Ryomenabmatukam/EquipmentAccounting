using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class DepartmentService
    {
        public List<Department> GetAll()
        {
            using (var db = new DLADbContext())
                return db.Departments.AsNoTracking().OrderBy(x => x.Name).ToList();
        }

        public void Add(Department d)
        {
            Validate(d);
            using (var db = new DLADbContext())
            {
                db.Departments.Add(d);
                db.SaveChanges();
            }
        }

        public void Update(Department d)
        {
            Validate(d);
            using (var db = new DLADbContext())
            {
                db.Departments.Update(d);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.Departments.FirstOrDefault(x => x.Id == id);
                if (entity == null) return;
                db.Departments.Remove(entity);
                db.SaveChanges();
            }
        }

        private void Validate(Department d)
        {
            if (d == null) throw new System.Exception("Пустые данные");
            if (string.IsNullOrWhiteSpace(d.Name)) throw new System.Exception("Название не заполнено");
            if (string.IsNullOrWhiteSpace(d.Manager)) throw new System.Exception("Руководитель не заполнен");
        }
    }
}
