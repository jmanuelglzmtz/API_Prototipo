using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [Route("/upload/upload")]
    public class UploadController : Controller
    {
        private readonly string[] ACCEPTED_FILE_TYPES = new[] {".jpg", ".jpeg", ".png"};
        private readonly IHostingEnvironment host;
        
        public UploadController(IHostingEnvironment host)
        {            
            this.host = host;
        }
        public async Task<IActionResult> Upload(IFormFile filesData)
        {   
            try
            {            
                if(filesData == null) 
                    return BadRequest("Null File");
                if(filesData.Length == 0)
                {
                    return  BadRequest("Empty File");
                }
                if(filesData.Length > 10 * 1024 * 1024) 
                    return BadRequest("Max file size exceeded.");
                if(!ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(filesData.FileName).ToLower())) 
                    return BadRequest("Invalid file type.");
                
                var uploadFilesPath = Path.Combine(host.WebRootPath, "uploads");
                
                if (!Directory.Exists(uploadFilesPath))
                    Directory.CreateDirectory(uploadFilesPath);
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(filesData.FileName);
                
                var filePath = Path.Combine(uploadFilesPath, fileName);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await filesData.CopyToAsync(stream);
                }                        
                return Ok();
            }
            catch 
            {
                return  BadRequest("Error en servidor");

            }
        }
    }
}