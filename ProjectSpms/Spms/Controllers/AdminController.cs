using Microsoft.AspNetCore.Mvc;
using spms.Models;

namespace spms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{

    private readonly SiteDbContext site;

    public AdminController(SiteDbContext site){
        this.site = site;
    }
    [HttpGet] 
    public ActionResult<List<Admin>> Index()
    {
        var selection = from e in site.Admins
                        select e;
        return selection.Count() > 0 ? selection.ToList() : NotFound();
    }

    [HttpPost]
    public IActionResult AddAdmin( Admin input){
            var admin = site.Admins.Find(input.UserName);
            if(admin is null)
            {
                admin = input;
                site.Admins.Add(admin);
            
            site.SaveChanges();
            return Ok();
            }
            else
            {
                return Conflict($"{input.UserName} already exists.");
            }
    }

   [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAdmin(string name)
    {
        var admin = await site.Admins.FindAsync(name);

        if (admin == null)
        {
            return Conflict($"{name} does not exist.");
        }
        else
        {
            site.Admins.Remove(admin);
            await site.SaveChangesAsync();
            return Ok(); 
        }
    }

    [HttpPut("{name}")]
    public async Task<IActionResult> UpdateAdmin(string name, Admin input)
    {
    var admin = await site.Admins.FindAsync(name);

        if (admin == null)
        {
            return Conflict($"{name} does not exist.");
        }
        else
        {
            admin.Email = input.Email;
            admin.Password = input.Password;
            await site.SaveChangesAsync();
            return Ok(); 
        }
    }

    
    
}