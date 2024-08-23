using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;


namespace P01_HospitalDatabase.Data.Models
{
    public class Medicament
    {
        public Medicament()
        {
            Prescriptions = new HashSet<PatientMedicament>();
        }
        [Key]
        public int MedicamentId { get; set; }

        [MaxLength(ValidationConstraints.NameOfMedicament)]
        public string Name { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
