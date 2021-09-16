using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartMed.CodeChalenge.Model;
using SmartMed.Database;
using SmartMed.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SmartMed.Repository
{
    public class MedicationRepository : IRepository<Medication>
    {

        MedicationDbContext MedicationDbContext;
        public MedicationRepository(MedicationDbContext medicationDbContext)
        {                
            MedicationDbContext = medicationDbContext;
        }

        public Medication Add(Medication item)
        {
            if (item.Id == default(Guid))
                item.Id = Guid.NewGuid();
            if (item.NumberOfDaysValid != 0)
                item.Expired = item.CreationDate.AddDays(item.NumberOfDaysValid);
            else
                item.Expired = DateTime.MaxValue;

            EntityEntry<Medication> entity = MedicationDbContext.Medications.Add(item);
            MedicationDbContext.SaveChanges();
            return item;
        }

        public void Delete(object id)
        {
            Guid guid = (Guid)id;

            Medication medication = Get(m => m.Id == guid);
            if (medication == null)
                throw new ArgumentException($"Invalid medication Id {id}!!!");
            medication.Active = false;
            MedicationDbContext.Medications.Update(medication);
            MedicationDbContext.SaveChanges();
        }

        public Medication Get(Expression<Func<Medication, bool>> query)
        {
            return MedicationDbContext.Medications.FirstOrDefault(query);
        }

        public IEnumerable<Medication> Get()
        {
            return MedicationDbContext.Medications.Where(m => m.Active);
        }

        public IEnumerable<Medication> Query(Expression<Func<Medication, bool>> query)
        {
            return MedicationDbContext.Medications.Where(query);
        }

        public Medication Update(Medication item)
        {
            Medication currentMedication = Get(m => m.Id == item.Id);

            if (currentMedication == null)
                throw new ArgumentException($"Invalid medication Id {item.Id}!!!");

            currentMedication.Name = item.Name;
            currentMedication.Quantity = item.Quantity;
            currentMedication.Dosage = item.Dosage;
            
            MedicationDbContext.Medications.Update(currentMedication);
            MedicationDbContext.SaveChanges();
            return Get(m => m.Id == item.Id);
        }
    }
}
