using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Diagnoses = new HashSet<Diagnose>();
            Visitations = new HashSet<Visitation>();
            Prescriptions = new HashSet<PatientMedicament>();
        }

        [Key]
        public int PatientId { get; set; }

        [MaxLength(ValidationConstraints.FirstNameOfPatient)]
        public string FirstName { get; set; }

        [MaxLength(ValidationConstraints.LastNameOfPatient)]
        public string LastName { get; set; }

        [MaxLength(ValidationConstraints.AddressOfPatient)]
        public string Address { get; set; }

        [Unicode(false)]
        [MaxLength(ValidationConstraints.EmailOfPatient)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public virtual ICollection<Diagnose> Diagnoses { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
