using System.Collections.Generic;

namespace DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}
