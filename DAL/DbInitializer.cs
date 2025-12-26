using DAL.Entities;
using System.Linq;

namespace DAL
{
    public static class DbInitializer
    {
        public static void EnsureCreatedAndSeed()
        {
            using (var db = new DLADbContext())
            {
                db.Database.EnsureCreated();

                if (!db.Departments.Any())
                {
                    db.Departments.AddRange(
                        new Department { Name = "ИТ отдел", Manager = "Иванов И.И." },
                        new Department { Name = "Бухгалтерия", Manager = "Петрова А.А." },
                        new Department { Name = "Отдел продаж", Manager = "Сидоров С.С." }
                    );
                    db.SaveChanges();
                }

                if (!db.EquipmentTypes.Any())
                {
                    db.EquipmentTypes.AddRange(
                        new EquipmentType { Name = "Системный блок" },
                        new EquipmentType { Name = "Монитор" },
                        new EquipmentType { Name = "Принтер" }
                    );
                    db.SaveChanges();
                }
            }
        }
    }
}
