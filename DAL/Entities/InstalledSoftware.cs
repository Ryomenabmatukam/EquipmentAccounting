using System;

namespace DAL.Entities
{
    public class InstalledSoftware
    {
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        public int SoftwareLicenseId { get; set; }
        public SoftwareLicense SoftwareLicense { get; set; }

        public DateTime InstalledAt { get; set; }
    }
}
