namespace P01_HospitalDatabase.Data.Common
{
    public static class ValidationConstraints
    {
        public const int FirstNameOfPatient = 50;
        public const int LastNameOfPatient = 50;
        public const int AddressOfPatient = 250;
        public const int EmailOfPatient = 80;

        ///Visitation
        public const int CommentsOfVisitation = 250;

        //•	Diagnose

        public const int NameOfDiagnose = 50;

        public const int CommentOfDiagnose = 250;

        //Medicament
        public const int NameOfMedicament = 50;
    }
}
