using System.IO;
using System.Threading.Tasks;
using azure_academy.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace azure_academy.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationDbContext ctx = null;
        private readonly IWebHostEnvironment env = null;
    
        public PhotoController(
            ApplicationDbContext ctx,
            IWebHostEnvironment env)
        {
            this.ctx = ctx;
            this.env = env;
        }
    
        public async Task<IActionResult> Index()
        {
            var model = await this.ctx.Photos.ToListAsync();
            return View(model);
        }
    
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<IActionResult> Create(Photo model, IFormFile file)
        {
            try
            {
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                if(!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                if (file.Length > 0) {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                    }
                    model.FileName = "/uploads/" + file.FileName;
                }
    
                ctx.Add(model);
                ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }
    
        public async Task<IActionResult> Delete(int id)
        {
            var student = await ctx.Photos
                .SingleOrDefaultAsync(m => m.Id == id);
    
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
    
            try
            {
                ctx.Photos.Remove(student);
                await ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    
    }
}