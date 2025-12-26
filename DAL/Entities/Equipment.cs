using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Equipment
    {
        public int Id { get; set; }

        public string InventoryNumber { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int Status { get; set; } 

        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public IEnumerable<InstalledSoftware> InstalledSoftwares { get; set; }
        public IEnumerable<EquipmentMovement> Movements { get; set; }
    }
}
