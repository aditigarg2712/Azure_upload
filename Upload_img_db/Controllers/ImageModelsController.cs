using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;
using Upload_img_db.Models;

namespace Upload_img_db.Controllers
{
    public class ImageModelsController : Controller
    {
        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        ImageModel ob1 = new ImageModel();


        public ImageModelsController(ImageDbContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        
        // GET: ImageModels
        public async Task<IActionResult> Index()
        {
              return View(await _context.Images.ToListAsync());
        }

        // GET: ImageModels/Details/5
       /* public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }*/

        // GET: ImageModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImageModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile ImageFile, string? Title)
        {


            string fileName = Path.GetFileName(ImageFile.FileName);
            string p1 = "C:\\Users\\AditiGarg\\source\\repos\\upload_img_db\\Upload_img_db\\wwwroot\\Images";
            string p = "C:\\Users\\AditiGarg\\source\\repos\\upload_img_db\\Upload_img_db\\wwwroot\\";
            string file_path = Path.Combine(p1, fileName);
            Directory.CreateDirectory(p1);
            var stream = new FileStream(file_path, FileMode.Create);
            await ImageFile.CopyToAsync(stream);
            stream.Close();
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=taskmgi;AccountKey=tuJIS7vmEmFmfkS2PXsVXL3vECCPx0nfJEieWpHZYz9k+Uc+kqcsUHQAj+hDp3nqq62Pp9Z22tRZ+AStggzgvg==;EndpointSuffix=core.windows.net";
            string containerName = "development";
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            var filePathOverCloud = file_path.Replace(p, string.Empty);
            long filesize = ImageFile.Length;
            if (filesize <= 5000000)
            {
                MemoryStream str = new MemoryStream(System.IO.File.ReadAllBytes(file_path));
                foreach (BlobItem blob in containerClient.GetBlobs())
                {
                    string s = blob.Name;
                    string a = "/";
                    string b = @"\";
                    if (s.Replace(a, b) == filePathOverCloud)
                    {
                        return View("Exists");
                    }
                    
                }
                Console.WriteLine("File started to upload");
                containerClient.UploadBlob(filePathOverCloud, str);
                str.Close();

                ob1.ImageName = filePathOverCloud;
                ob1.Title = Title;
                _context.Add(ob1);
                await _context.SaveChangesAsync();
                return View("Index");
            }



            /*containerClient.UploadBlob(filePathOverCloud, str);*/
            /*str.Close();

            ob1.ImageName = filePathOverCloud;
            ob1.Title = Title;
            _context.Add(ob1);
            await _context.SaveChangesAsync();*/

            /*containerClient.UploadBlob(filePathOverCloud, str);*/


            /*str.Close();*/
            /*ob1.ImageName = filePathOverCloud;
            ob1.Title = Title;
            _context.Add(ob1);
            await _context.SaveChangesAsync();*/

            
            return View("Error");
            
            
        }
            

        // GET: ImageModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            return View(imageModel);
        }

        // POST: ImageModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,Title,ImageName")] ImageModel imageModel)
        {
            if (id != imageModel.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.ImageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: ImageModels/Delete/5
        /*public async Task<IActionResult> Delete(int? id,string blobName)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }*/

        // POST: ImageModels/Delete/5

        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string blobName)
        {

            /*if (_context.Images == null)
            {
                return Problem("Entity set 'ImageDbContext.Images'  is null.");
            }*/
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=taskmgi;AccountKey=tuJIS7vmEmFmfkS2PXsVXL3vECCPx0nfJEieWpHZYz9k+Uc+kqcsUHQAj+hDp3nqq62Pp9Z22tRZ+AStggzgvg==;EndpointSuffix=core.windows.net";
            string containerName = "development";

            BlobClient blob = new BlobClient(connectionString,containerName, blobName);
            blob.Delete();

            
            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel != null)
            {
                _context.Images.Remove(imageModel);
                

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageModelExists(int id)
        {
          return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
