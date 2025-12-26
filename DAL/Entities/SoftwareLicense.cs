using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class SoftwareLicense
    {
        public int Id { get; set; }

        public string SoftwareName { get; set; }
        public string Manufacturer { get; set; }
        public string LicenseKey { get; set; }
        public DateTime ExpirationDate { get; set; }

        public IEnumerable<InstalledSoftware> InstalledSoftwares { get; set; }
    }
}
