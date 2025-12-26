using System;

namespace DAL.Entities
{
    public class EquipmentMovement
    {
        public int Id { get; set; }

        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        public DateTime MovedAt { get; set; }

        public int? OldEmployeeId { get; set; }
        public Employee OldEmployee { get; set; }

        public int? NewEmployeeId { get; set; }
        public Employee NewEmployee { get; set; }
    }
}
