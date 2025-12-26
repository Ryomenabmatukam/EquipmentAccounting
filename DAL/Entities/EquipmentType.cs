using System.Collections.Generic;

namespace DAL.Entities
{
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Equipment> Equipments { get; set; }
    }
}
