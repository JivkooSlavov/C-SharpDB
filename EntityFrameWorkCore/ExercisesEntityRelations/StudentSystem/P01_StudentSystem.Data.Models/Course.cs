using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;
public class Course
{
    // CourseId
    // Name – up to 80 characters, unicode
    // Description – unicode, not required
    // StartDate
    // EndDate
    // Price

    public Course()
    {
        StudentsCourses = new HashSet<StudentCourse>();
        Resources = new HashSet<Resource>();
        Homeworks = new HashSet<Homework>();
    }
    public int CourseId { get; set; }

    [MaxLength(ValidationConstraints.CourseNameMaxLength)]
    public string Name { get; set; }

    //nullable
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    //relation to the StudentCourse table: one course to many students
    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    //relation to Resources table: one course to many resources
    public virtual ICollection<Resource> Resources { get; set; }

    //relation to Homework table: one course to many homeworks
    public virtual ICollection<Homework> Homeworks { get; set; }
}
