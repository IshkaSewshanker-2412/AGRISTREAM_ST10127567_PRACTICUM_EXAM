
using AgriStreamMVC.Data;
using AgriStreamMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
namespace AgriStreamMVC.Controllers
{
    public class FarmSensorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public FarmSensorsController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var sensors = _context.FarmSensors.ToList();
            return View(sensors);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            FarmSensor sensor,
            IFormFile? imageFile)
        {
            try
            {
                // Test if file is reaching controller
                if (imageFile == null)
                {
                    return Content("imageFile is NULL");
                }
                string connectionString =
                    _configuration["AzureStorage:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    return Content("Storage connection string is NULL");
                }
                BlobContainerClient container =
                    new BlobContainerClient(
                        connectionString,
                        "drone-images");
                await container.CreateIfNotExistsAsync();
                BlobClient blob =
                    container.GetBlobClient(imageFile.FileName);
                using (var stream = imageFile.OpenReadStream())
                {
                    await blob.UploadAsync(stream, overwrite: true);
                }
                sensor.ImageUrl = blob.Uri.ToString();
                _context.FarmSensors.Add(sensor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}
//using AgriStreamMVC.Data;
//using AgriStreamMVC.Models;
//using Microsoft.AspNetCore.Mvc;
//using Azure.Storage.Blobs;
//using Microsoft.Extensions.Configuration;
//namespace AgriStreamMVC.Controllers

//{
//    public class FarmSensorsController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _configuration;
//        public FarmSensorsController(ApplicationDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }
//        public IActionResult Index()
//        {
//            var sensors = _context.FarmSensors.ToList();
//            return View(sensors);
//        }
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(FarmSensor sensor, IFormFile? imageFile)
//        {
//            try
//            {
//                if (imageFile != null && imageFile.Length > 0)
//                {
//                    string connectionString =
//                        _configuration["AzureStorage:ConnectionString"];
//                    BlobContainerClient container =
//                        new BlobContainerClient(
//                            connectionString,
//                            "drone-images");
//                    await container.CreateIfNotExistsAsync();
//                    BlobClient blob =
//                        container.GetBlobClient(imageFile.FileName);
//                    using (var stream = imageFile.OpenReadStream())
//                    {
//                        await blob.UploadAsync(stream, overwrite: true);
//                    }
//                    sensor.ImageUrl = blob.Uri.ToString();
//                }
//                _context.FarmSensors.Add(sensor);
//                _context.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            catch (Exception ex)
//            {
//                return Content($"Error: {ex.Message}");
//            }
//        }
//    }
//}