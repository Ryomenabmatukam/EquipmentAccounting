using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class ReportsService
    {
        public List<dynamic> EquipmentByDepartments()
        {
            using (var db = new DLADbContext())
            {
                return db.Equipments
                    .Include(x => x.Employee).ThenInclude(e => e.Department)
                    .Include(x => x.EquipmentType)
                    .AsNoTracking()
                    .OrderBy(x => x.Employee.Department.Name)
                    .ThenBy(x => x.InventoryNumber)
                    .Select(x => new
                    {
                        Department = x.EmployeeId == null ? "Не закреплено" : x.Employee.Department.Name,
                        Employee = x.EmployeeId == null ? "-" : x.Employee.FullName,
                        x.InventoryNumber,
                        EquipmentType = x.EquipmentType.Name,
                        x.Name,
                        x.SerialNumber,
                        Status = x.Status == 0 ? "В работе" : x.Status == 1 ? "На списании" : "В ремонте"
                    })
                    .ToList<dynamic>();
            }
        }

        public List<dynamic> SoftwareForEmployee(int employeeId)
        {
            using (var db = new DLADbContext())
            {
                var equipmentIds = db.Equipments.Where(x => x.EmployeeId == employeeId).Select(x => x.Id).ToList();

                return db.InstalledSoftwares
                    .Where(x => equipmentIds.Contains(x.EquipmentId))
                    .Include(x => x.SoftwareLicense)
                    .Include(x => x.Equipment)
                    .AsNoTracking()
                    .OrderBy(x => x.Equipment.InventoryNumber)
                    .ThenBy(x => x.SoftwareLicense.SoftwareName)
                    .Select(x => new
                    {
                        x.Equipment.InventoryNumber,
                        EquipmentName = x.Equipment.Name,
                        Software = x.SoftwareLicense.SoftwareName,
                        x.SoftwareLicense.Manufacturer,
                        x.InstalledAt,
                        x.SoftwareLicense.ExpirationDate
                    })
                    .ToList<dynamic>();
            }
        }
    }
}
