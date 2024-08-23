using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;
public class StudentSystemContext : DbContext
{
    public StudentSystemContext()
    {

    }

    //Needed for Judge
    public StudentSystemContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User));
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Defining complex key for mapping table as it is needed by Judge
        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(pk => new { pk.StudentId, pk.CourseId });
            entity.HasOne(sc => sc.Student)
                .WithMany(s => s.StudentsCourses)
                .HasForeignKey(sc => sc.StudentId);
            entity.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.CourseId);
        });
    }
}
