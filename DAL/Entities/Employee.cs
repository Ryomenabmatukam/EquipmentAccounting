using System.Collections.Generic;

namespace DAL.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public IEnumerable<Equipment> Equipments { get; set; }
    }
}
