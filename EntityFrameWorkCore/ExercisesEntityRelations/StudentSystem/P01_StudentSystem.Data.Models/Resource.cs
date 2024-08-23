using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Common;
using P01_StudentSystem.Data.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;
public class Resource
{
    // ResourceId
    // Name – up to 50 characters, unicode
    // Url – not unicode
    // ResourceType – enum, can be Video, Presentation, Document or Other
    // CourseId

    public int ResourceId { get; set; }

    [MaxLength(ValidationConstraints.ResourceNameMaxLength)]
    public string Name { get; set; }

    [Unicode(false)] //could be a problem for Judge
    public string Url { get; set; }

    //required by default as it is kept as int representation in Db
    public ResourceType ResourceType { get; set; }

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }

    //navigation properties = virtual
    public virtual Course Course { get; set; }
}
