using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spms.Models;

[ApiController]
[Route("/api/student")]
public class StudentController : ControllerBase{

    private readonly SiteDbContext site;
    public StudentController(SiteDbContext site){
        this.site = site;
        ++Student.Count;
    }

    [HttpGet]
    public ActionResult<List<object>> GetStudent(){
    var students = from std in site.Students
                   select new 
                   {
                        std.StudentId,
                        std.FirstName,
                        std.MiddleName,
                        std.LastName,
                        std.Email,
                        std.Mobile,
                        std.DOB.Date,
                   };

    var studentList = students.ToList();

    return studentList.Any() ? studentList.Cast<object>().ToList() : NotFound("Data Not Found"); 
}


    [HttpPost]
    public async Task<IActionResult> AddStudent(Student std){
        var student = await site.Students.FirstOrDefaultAsync(a => a.Email == std.Email);
        var lastStudent = await site.Students.OrderByDescending(s => s.StudentId).FirstOrDefaultAsync();
        if(student != null){
            return Conflict("Student Already There");
        }
        else{
            std.StudentId = lastStudent != null ? lastStudent.StudentId + 1 : 1001 ;
            std.Password = std.StudentId.ToString();
            std.DOB = std.DOB.Date;
            site.Students.Add(std);
            site.SaveChanges();
            return Ok("Student Added Successfully");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, Student input)
    {
        var std = await site.Students.FindAsync(id);

        if (std == null)
        {
            return Conflict($"{id} does not exist.");
        }
        else
        {
            std.FirstName = input.FirstName ?? (site.Students.FirstOrDefault(e => e.StudentId == id)?.FirstName);
            std.MiddleName = input.MiddleName != null ? input.MiddleName : site.Students.FirstOrDefault(e => e.StudentId == id)?.MiddleName;
            std.LastName = input.LastName != null ? input.LastName :site.Students.FirstOrDefault(e => e.StudentId == id)?.LastName;
            std.Email = input.Email != null ? input.Email : site.Students.FirstOrDefault(e => e.StudentId == id)?.Email;
            std.Mobile = input.Mobile != null ? input.Mobile : site.Students.FirstOrDefault(e => e.StudentId == id)?.Mobile;
            #pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
            std.DOB = (DateTime)(input.DOB != null ? input.DOB : site.Students.FirstOrDefault(e => e.StudentId == id)?.DOB);
            #pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
            await site.SaveChangesAsync();
            return Ok($"{std.FirstName} Update Successfully"); 
        }
    }

    
   [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await site.Students.FindAsync(id);

        if (student == null)
        {
            return Conflict($"{id} does not exist.");
        }
        else
        {
            site.Students.Remove(student);
            await site.SaveChangesAsync();
            return Ok($"{id} deleted successfully"); 
        }
    }

}