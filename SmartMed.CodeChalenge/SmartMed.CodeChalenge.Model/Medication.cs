using System;

namespace SmartMed.CodeChalenge.Model
{
    public class Medication
    {
        public Guid Id { get; set; }
        
        public string Name{ get; set; }

        private int _quantity;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value > 0)
                    _quantity = value;
                else
                    throw new ArgumentException("Medication quantity must be higher then 0");
            }
        }

        public double _dosage;

        public double Dosage
        {
            get
            {
                return _dosage;
            }
            set
            {
                if (value > 0)
                    _dosage = value;
                else
                    throw new ArgumentException("Medication dosage must be higher then 0");
            }
        }

        public DateTime CreationDate { get; set; }

        public bool Active { get; set; }

        public int NumberOfDaysValid { get; set; }

        public DateTime? Expired { get; set; }
    }
}
