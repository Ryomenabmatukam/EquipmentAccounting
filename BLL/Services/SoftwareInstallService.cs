using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class SoftwareInstallService
    {
        public List<Equipment> GetEquipments()
        {
            using (var db = new DLADbContext())
                return db.Equipments.Include(x => x.Employee).AsNoTracking().OrderBy(x => x.InventoryNumber).ToList();
        }

        public List<SoftwareLicense> GetLicenses()
        {
            using (var db = new DLADbContext())
                return db.SoftwareLicenses.AsNoTracking().OrderBy(x => x.SoftwareName).ToList();
        }

        public List<dynamic> GetInstalledForEquipment(int equipmentId)
        {
            using (var db = new DLADbContext())
            {
                return db.InstalledSoftwares
                    .Where(x => x.EquipmentId == equipmentId)
                    .Include(x => x.SoftwareLicense)
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.EquipmentId,
                        x.SoftwareLicenseId,
                        x.SoftwareLicense.SoftwareName,
                        x.SoftwareLicense.Manufacturer,
                        x.InstalledAt
                    })
                    .ToList<dynamic>();
            }
        }

        public void Install(int equipmentId, int licenseId, DateTime installedAt)
        {
            using (var db = new DLADbContext())
            {
                var exists = db.InstalledSoftwares.Any(x => x.EquipmentId == equipmentId && x.SoftwareLicenseId == licenseId);
                if (exists) throw new Exception("Эта лицензия уже установлена на выбранное оборудование.");

                db.InstalledSoftwares.Add(new InstalledSoftware
                {
                    EquipmentId = equipmentId,
                    SoftwareLicenseId = licenseId,
                    InstalledAt = installedAt
                });
                db.SaveChanges();
            }
        }

        public void Uninstall(int equipmentId, int licenseId)
        {
            using (var db = new DLADbContext())
            {
                var entity = db.InstalledSoftwares.FirstOrDefault(x => x.EquipmentId == equipmentId && x.SoftwareLicenseId == licenseId);
                if (entity == null) return;
                db.InstalledSoftwares.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
