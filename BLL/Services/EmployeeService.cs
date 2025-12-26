using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class EmployeeService
    {
        public List<Employee> GetAll()
        {
            using (var db = new DLADbContext())
                return db.Employees.Include(x => x.Department).AsNoTracking().OrderBy(x => x.FullName).ToList();
        }

        public List<Department> GetDepartments()
        {
            using (var db = new DLADbContext())
                return db.Departments.AsNoTracking().OrderBy(x => x.Name).ToList();
        }

        public void Add(Employee e)
        {
            Validate(e);
            using (var db = new DLADbContext())
            {
                db.Employees.Add(e);
                db.SaveChanges();
            }
        }

        public void Update(Employee e)
        {
            Validate(e);
            using (var db = new DLADbContext())
            {
                db.Employees.Update(e);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.Employees.FirstOrDefault(x => x.Id == id);
                if (entity == null) return;
                db.Employees.Remove(entity);
                db.SaveChanges();
            }
        }

        private void Validate(Employee e)
        {
            if (e == null) throw new System.Exception("Пустые данные");
            if (string.IsNullOrWhiteSpace(e.FullName)) throw new System.Exception("ФИО не заполнено");
            if (e.DepartmentId <= 0) throw new System.Exception("Подразделение не выбрано");
        }
    }
}
