using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class EquipmentService
    {
        public List<Equipment> GetAll()
        {
            using (var db = new DLADbContext())
            {
                return db.Equipments
                    .Include(x => x.EquipmentType)
                    .Include(x => x.Employee)
                    .AsNoTracking()
                    .OrderBy(x => x.InventoryNumber)
                    .ToList();
            }
        }

        public List<Employee> GetEmployees()
        {
            using (var db = new DLADbContext())
                return db.Employees.Include(x => x.Department).AsNoTracking().OrderBy(x => x.FullName).ToList();
        }

        public List<EquipmentType> GetTypes()
        {
            using (var db = new DLADbContext())
                return db.EquipmentTypes.AsNoTracking().OrderBy(x => x.Name).ToList();
        }

        public void Add(Equipment e)
        {
            Validate(e);
            using (var db = new DLADbContext())
            {
                if (e.RegisteredAt == default) e.RegisteredAt = DateTime.Now;
                db.Equipments.Add(e);
                db.SaveChanges();

                // movement record if assigned
                if (e.EmployeeId != null)
                {
                    db.EquipmentMovements.Add(new EquipmentMovement
                    {
                        EquipmentId = e.Id,
                        MovedAt = DateTime.Now,
                        OldEmployeeId = null,
                        NewEmployeeId = e.EmployeeId
                    });
                    db.SaveChanges();
                }
            }
        }

        public void Update(Equipment e)
        {
            Validate(e);
            using (var db = new DLADbContext())
            {
                var old = db.Equipments.AsNoTracking().FirstOrDefault(x => x.Id == e.Id);
                db.Equipments.Update(e);
                db.SaveChanges();

                // if employee changed -> add movement
                if (old != null && old.EmployeeId != e.EmployeeId)
                {
                    db.EquipmentMovements.Add(new EquipmentMovement
                    {
                        EquipmentId = e.Id,
                        MovedAt = DateTime.Now,
                        OldEmployeeId = old.EmployeeId,
                        NewEmployeeId = e.EmployeeId
                    });
                    db.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.Equipments.FirstOrDefault(x => x.Id == id);
                if (entity == null) return;
                db.Equipments.Remove(entity);
                db.SaveChanges();
            }
        }

        private void Validate(Equipment e)
        {
            if (e == null) throw new Exception("Пустые данные");
            if (string.IsNullOrWhiteSpace(e.InventoryNumber)) throw new Exception("Инв. номер не заполнен");
            if (e.EquipmentTypeId <= 0) throw new Exception("Тип не выбран");
            if (string.IsNullOrWhiteSpace(e.Name)) throw new Exception("Название не заполнено");
            if (e.Status < 0 || e.Status > 2) throw new Exception("Неверный статус");
        }
    }
}
