using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Loppuprojekti_AW.Controllers
{
    public class AccountController : Controller
    {
        private readonly string blobServiceEndpoint;
        private readonly string storageAccountName;
        private readonly string storageAccountKey;
        private readonly string blobName;

        private readonly MoveoContext _context;
        public AccountController(MoveoContext context, IConfiguration configuration)
        {
            _context = context;
            blobName = configuration["BlobName"];
            storageAccountKey = configuration["StorageAccountKey"];
            storageAccountName = configuration["StorageAccountName"];
            blobServiceEndpoint = configuration["StorageEndPoint"];
        }

        public IActionResult Index()
        {
            ViewBag.Nimi = HttpContext.Session.GetString("username");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Epäonnistui = false;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Enduser eu, IFormFile Photo)
        {
            DataAccess da = new DataAccess(_context);
            bool result = da.GetUsersByEmail(eu.Email); //boolin result on joko false tai true

            if (result == false) //jos result on sama kuin false eli vastaavuutta ei löytynyt
            {
                string photourl = AddPhotoInContainer(Photo);
                eu.Photo = photourl;
                da.CreateUser(eu);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Epäonnistui = true;
                return View();
            }
        }

        /// <summary>
        /// Lisää kuvan Blobiin ja palauttaa kuvan URL:n
        /// </summary>
        /// <param name="Photo"></param>
        /// <returns>string URL</returns>
        [HttpPost]
        private string AddPhotoInContainer(IFormFile Photo, string oldPhoto = null)
        {
            if (Photo == null)
            {
                return null;
            }
            using Stream imageStream = Photo.OpenReadStream();
            StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
            BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(blobName);

            if (oldPhoto != null)
            {
                oldPhoto = oldPhoto.Substring(oldPhoto.LastIndexOf('/')+1);
                var bc = containerClient.DeleteBlob(oldPhoto);
            }

            string photoname = Guid.NewGuid().ToString();
            containerClient.UploadBlob(photoname, imageStream);
            BlobClient blob = containerClient.GetBlobClient(photoname);
            return blob.Uri.ToString();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid != null)
            {
                var enduser = new DataAccess(_context).GetUserById(userid);
                return View(enduser);
            }
            else
            {
                return RedirectToAction("Virhe", "Home");
            }
        }

        [HttpGet]
        public IActionResult Edit()//siirrytään tiettyyn palveluun uniikin palveluid perusteella, uusi muokkausnäkymä
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var enduser = new DataAccess(_context).GetUserById(userid);
            if (enduser == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            return View(enduser);
        }


        [HttpPost] //editoidaan henkilöä ja lähetetaan se
        public IActionResult Edit(Enduser Eu, IFormFile Photo)
        {
            DataAccess da = new DataAccess(_context);
            string oldPhoto = da.GetCurrentPhotoUrl(Eu.Userid);
            Eu.Photo = AddPhotoInContainer(Photo, oldPhoto);
            da.EditUser(Eu);
            return RedirectToAction("Profile", new { Id = Eu.Userid });
        }

        public IActionResult Delete(Enduser Eu)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            DataAccess da = new DataAccess(_context);
            var enduser = da.GetUserById(userid);
            if (enduser == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            else
            {
                da.DeleteUser(enduser);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }
        public static string Hash(string value)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes("Passw0rd"),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool Validate(string value, string hash)
            => Hash(value) == hash;
    }
}
