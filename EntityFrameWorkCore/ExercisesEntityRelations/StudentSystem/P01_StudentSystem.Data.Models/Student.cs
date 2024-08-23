using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;
public class Student
{

    // StudentId
    // Name – up to 100 characters, unicode
    // PhoneNumber – exactly 10 characters, not unicode, not required
    // RegisteredOn
    // Birthday – not required

    public Student()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Homeworks = new HashSet<Homework>();
    }

    public int StudentId { get; set; }

    [MaxLength(ValidationConstraints.StudentNameMaxLength)]
    public string Name { get; set; }

    //nullable
    [Unicode(false)]
    [StringLength(ValidationConstraints.StudentPhoneNumberExactLengthStudentPhoneNumberExactLength, MinimumLength = ValidationConstraints.StudentPhoneNumberExactLengthStudentPhoneNumberExactLength)]
    public string? PhoneNumber { get; set; }

    public DateTime RegisteredOn { get; set; }

    //nullable
    public DateTime? Birthday { get; set; }

    //relation to the StudentCourse table: one student to many courses
    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    //relation to Homework table: one student to many homeworks
    public virtual ICollection<Homework> Homeworks { get; set; }

}
