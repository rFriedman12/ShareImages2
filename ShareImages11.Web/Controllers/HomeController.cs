using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShareImages11.Data;
using ShareImages11.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShareImages11.Web.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;

        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            return View(new HomeViewModel
            {
                Images = imagesRepo.GetAllImages()
            });
        }
        
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string imageTitle)
        {
            string fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

            using FileStream fileStream = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fileStream);

            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            imagesRepo.AddImage(new Image
            {
                Title = imageTitle,
                FileName = fileName,
                FilePath = filePath,
                DateTimeUploaded = DateTime.Now,
                Likes = 0
            });

            return Redirect("/");
        }

        public IActionResult GetAll()
        {
            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            return Json(imagesRepo.GetAllImages());
        }

        public IActionResult ViewImage(int id)
        {
            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            var likedImages = HttpContext.Session.Get<List<int>>("LikedImages");
            if(likedImages == null)
            {
                likedImages = new List<int>();
            }
            return View(new ViewImageViewModel { 
                Image = imagesRepo.GetImageById(id),
                Liked = likedImages.Contains(id)
            });
        }

        [HttpPost]
        public void LikeImage(int imageId)
        {
            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            imagesRepo.LikeImage(imageId);
            var likedImages = HttpContext.Session.Get<List<int>>("LikedImages");
            if(likedImages == null)
            {
                likedImages = new List<int>();
            }
            likedImages.Add(imageId);
            HttpContext.Session.Set("LikedImages", likedImages);
        }

        public IActionResult GetLikes(int imageId)
        {
            var imagesRepo = new ImagesRepository(_configuration.GetConnectionString("ConStr"));
            return Json(imagesRepo.GetLikes(imageId));
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
