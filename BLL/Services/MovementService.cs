using DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class MovementService
    {
        public List<dynamic> GetMovements(int equipmentId)
        {
            using (var db = new DLADbContext())
            {
                return db.EquipmentMovements
                    .Where(x => x.EquipmentId == equipmentId)
                    .Include(x => x.OldEmployee)
                    .Include(x => x.NewEmployee)
                    .AsNoTracking()
                    .OrderByDescending(x => x.MovedAt)
                    .Select(x => new
                    {
                        x.Id,
                        x.MovedAt,
                        OldEmployee = x.OldEmployeeId == null ? "-" : x.OldEmployee.FullName,
                        NewEmployee = x.NewEmployeeId == null ? "-" : x.NewEmployee.FullName
                    })
                    .ToList<dynamic>();
            }
        }
    }
}
