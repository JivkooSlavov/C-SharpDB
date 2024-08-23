using P01_StudentSystem.Data.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;
public class Homework
{
    // HomeworkId
    // Content – string, linking to a file, not unicode
    // ContentType - enum, can be Application, Pdf or Zip
    // SubmissionTime
    // StudentId
    // CourseId

    public int HomeworkId { get; set; }

    public string Content { get; set; }

    //required by default as it is kept as int representation in Db
    public ContentType ContentType { get; set; }

    public DateTime SubmissionTime { get; set; }

    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    public Student Student { get; set; }

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }

    public Course Course { get; set; }

}
