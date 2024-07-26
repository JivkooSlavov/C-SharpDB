using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();
        //Console.WriteLine(GetEmployeesFullInformation(dbContext));
        //Console.WriteLine(GetEmployeesWithSalaryOver50000(dbContext));
        //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(dbContext));
        //Console.WriteLine(AddNewAddressToEmployee(dbContext));
        //Console.WriteLine(GetEmployeesInPeriod(dbContext));
        //Console.WriteLine(GetAddressesByTown(dbContext));
        //Console.WriteLine(GetEmployee147(dbContext));
        // Console.WriteLine(GetLatestProjects(dbContext));
        //Console.WriteLine(IncreaseSalaries(dbContext));
        //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbContext));
        //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbContext));
        //Console.WriteLine(DeleteProjectById(dbContext));
        Console.WriteLine(RemoveTown(dbContext));
    }

    //3.Employees Full Information
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        foreach (var em in employees)
        {
            sb.AppendLine($"{em.FirstName} {em.LastName} {em.MiddleName} {em.JobTitle} {em.Salary:f2}");
        }

        return sb.ToString();
    }
    //4.Employees with Salary Over 50 000
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employees = context.Employees
       .Where(e => e.Salary > 50000)
       .OrderBy(e => e.FirstName)
       .Select(e => new
       {
           e.FirstName,
           e.Salary
       })
       .ToList();

        foreach (var em in employees)
        {
            sb.AppendLine($"{em.FirstName} - {em.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }
    //5.	Employees from Research and Development
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var employees = context.Employees
        .Where(e => e.Department.Name == "Research and Development")
        .Select(e => new
        {
                   e.FirstName,
                   e.LastName,
                   DepartmentName = e.Department.Name,
                   e.Salary
        })
        .OrderBy(e => e.Salary)
        .ThenByDescending(e => e.FirstName)
        .ToList();

        foreach (var em in employees)
        {
            sb.AppendLine($"{em.FirstName} {em.LastName} from {em.DepartmentName} - ${em.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }
   // 6.	Adding a New Address and Updating Employee
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        Employee employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
        employee.Address = newAddress;
        context.SaveChanges();

        var employeeAddress = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address.AddressText)
            .ToList();
        
        return string.Join(Environment.NewLine, employeeAddress);
    }
    //7.	Employees and Projects
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager.FirstName,
                ManagerLastName = e.Manager.LastName,
                Projects = e.EmployeesProjects
              .Where(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003)
              .Select(p => new
              {
                  ProjectName = p.Project.Name,
                  ProjectStartDate = p.Project.StartDate,
                  ProjectEndDate = p.Project.EndDate.HasValue ? p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
              })
              .ToArray()
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var employee in employees)
        {
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

            foreach (var project in employee.Projects)
            {
                sb.AppendLine($"--{project.ProjectName} - {project.ProjectStartDate} - {project.ProjectEndDate}");
            }
        }
        return sb.ToString().TrimEnd();
    }
    //8.	Addresses by Town
    public static string GetAddressesByTown(SoftUniContext context)
    {
        var address = context.Addresses
            .OrderByDescending(a=>a.Employees.Count)
            .ThenBy(a=>a.Town.Name)
            .ThenBy(a=>a.AddressText)
            .Take(10)
            .Select(a => new 
            {
                a.AddressText,
                TownName = a.Town.Name,
                EmployeeCount = a.Employees.Count,
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var add in address)
        {
            sb.AppendLine($"{add.AddressText}, {add.TownName} - {add.EmployeeCount} employees");
        }

        return sb.ToString().TrimEnd();

    }
    //9.	Employee 147
    public static string GetEmployee147(SoftUniContext context)
    {
        var employee = context.Employees
            .Include(e => e.EmployeesProjects)
            .ThenInclude(e => e.Project).
            FirstOrDefault(e => e.EmployeeId == 147);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

        foreach (var employeeProject in employee.EmployeesProjects
                 .OrderBy(ep => ep.Project.Name))
        {
            sb.AppendLine(employeeProject.Project.Name);
        }

        return sb.ToString().TrimEnd();

    }
    //10.	Departments with More Than 5 Employees
    public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)

    {
        var departments = context.Departments
            .Where(x => x.Employees.Count > 5)
            .OrderBy(x => x.Employees.Count)
            .ThenBy(x => x.Name)
            .Select(x=> new 
            { 
                x.Name,
                ManagerFirstName = x.Manager.FirstName,
                ManagerLastName = x.Manager.LastName,
                Employees = x.Employees
                  .OrderBy(e=>e.FirstName)
                  .ThenBy(e=>e.LastName)
                  .Select(e => new 
                  {
                      e.FirstName,
                      e.LastName,
                      e.JobTitle
                  })

            }).ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var department in departments)
        {
            sb.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");

            foreach (var employee in department.Employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            }
        }

        return sb.ToString().TrimEnd();

    }
    //11.	Find Latest 10 Projects
     public static string GetLatestProjects(SoftUniContext context)
    {
        var projectsStarted = context.Projects
            .OrderByDescending(x=>x.StartDate)
            .Take(10)
            .OrderBy(x=>x.Name)
            .Select(x=> new 
            {
                x.Name,
                x.Description,
                x.StartDate
            }).ToArray();

        StringBuilder sb = new StringBuilder();
        foreach (var project in projectsStarted)
        {
            sb.AppendLine(project.Name);
            sb.AppendLine(project.Description);
            sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
        }
        return sb.ToString().TrimEnd();
    }
    //12.	Increase Salaries
    public static string IncreaseSalaries(SoftUniContext context)
    {
        string[] promotedDepartments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

        foreach (var employee in context.Employees
                     .Where(e => promotedDepartments.Contains(e.Department.Name)))
        {
            employee.Salary *= 1.12m;
        }

        context.SaveChanges();

        var employeesPromoted = context.Employees
            .Where(e => promotedDepartments.Contains(e.Department.Name))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            })
            .ToArray();

        StringBuilder sb = new StringBuilder();

        foreach (var employee in employeesPromoted)
        {
            sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
        }

        return sb.ToString().TrimEnd();

    }
    //13.	Find Employees by First Name Starting with "Sa"
    public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.FirstName.ToLower().StartsWith("sa"))
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary
            }).ToArray();


        StringBuilder sb = new StringBuilder();
        foreach (var eeeee in employees)
        {
            sb.AppendLine($"{eeeee.FirstName} {eeeee.LastName} - {eeeee.JobTitle} - (${eeeee.Salary:f2})");
        }
        return sb.ToString().TrimEnd();

    }

    //14.	Delete Project by Id
    public static string DeleteProjectById(SoftUniContext context)
    {
        var employeeProjectsToDelete = context.EmployeesProjects
            .Where(ep => ep.ProjectId == 2);

        context.RemoveRange(employeeProjectsToDelete);

        context.Projects.Remove(context.Projects.Find(2));

        context.SaveChanges();

        var projects = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToArray();

        return string.Join(Environment.NewLine, projects);
    }

    //15.	Remove Town
    public static string RemoveTown(SoftUniContext context)
    {
        var employeeAddresses = context.Employees
            .Where(e => e.Address.Town.Name == "Seattle");

        foreach (var address in employeeAddresses)
        {
            address.AddressId = null;
        }

        var townAddresses = context.Addresses
            .Where(a => a.Town.Name == "Seattle");

        int totalAddresses = townAddresses.Count();

        context.RemoveRange(townAddresses);

        context.Remove(context.Towns.FirstOrDefault(t => t.Name == "Seattle")!);

        context.SaveChanges();

        return $"{totalAddresses} addresses in Seattle were deleted";
    }
}