
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spms.Models;

namespace spms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase{

    private readonly SiteDbContext site;

    public CourseController(SiteDbContext site){
        this.site = site;
    }
    
    [HttpGet]
    public ActionResult<List<Course>> GetCourse(){
        var course = from crs in site.Courses
                     select crs;
        return course.Count() > 0 ? course.ToList() : NotFound();
    }
    
    [HttpPost]
    public IActionResult AddCourse(Course input){
        var course = site.Courses.Find(input.CourseId);
            if(course is null)
            {
                course=input;
                site.Courses.Add(course);
                site.SaveChanges();
                return Ok();
            }
            else
            {
                return Conflict($"{input.CourseId} already exists.");
            }       
    }  
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id,Course input){
        var course = await site.Courses.FirstOrDefaultAsync(crs=>crs.CourseId == id);
        if(course == null){
            return NotFound($"{id} Not Found");
        }
        course.CourseName = input.CourseName != null ? input.CourseName : site.Courses.FirstOrDefault(c=>c.CourseId == id)?.CourseName;
        site.SaveChanges();
        return Ok("Course Update Successfully");
    }   

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id){
        var course = await site.Courses.FirstOrDefaultAsync(crs=>crs.CourseId == id);
        if(course == null){
            return NotFound($"{id} Not Found");
        }
        site.Courses.Remove(course);
        await site.SaveChangesAsync();
        return Ok($"{course.CourseName} Delete Successfully");
    }
}