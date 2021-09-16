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
using System.Globalization;

namespace Loppuprojekti_AW.Controllers
{
    public class AccountController : Controller
    {
        private readonly string blobServiceEndpoint;
        private readonly string storageAccountName;
        private readonly string storageAccountKey;
        private readonly string photoBlob;
        private readonly string thumbnailPhotoBlob;
        private readonly MoveoContext _context;
        public readonly CultureInfo _ci;

        public AccountController(MoveoContext context, IConfiguration configuration)
        {
            _context = context;
            photoBlob = configuration["BlobName"];
            thumbnailPhotoBlob = photoBlob + "-thumb";
            storageAccountKey = configuration["StorageAccountKey"];
            storageAccountName = configuration["StorageAccountName"];
            blobServiceEndpoint = configuration["StorageEndPoint"];
            _ci = new CultureInfo("fi-fi");
        }

        public async Task<IActionResult> Index()
        {
            DataAccess da = new DataAccess(_context);
            var userid = HttpContext.Session.GetInt32("userid");
            ViewBag.Username = HttpContext.Session.GetString("username");

            var enduser = new DataAccess(_context).GetUserById(userid);
            ViewBag.Avatar = enduser.Photo;

            ViewBag.UserOwnPosts = await Task.Run(() => da.GetUserPosts(userid)); //organiser true
            ViewBag.UserAttendenceToday = await Task.Run(() => da.GetOtherPostsByAttendanceToday(userid)); //organiser false
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
            ViewBag.Failed = false;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Enduser enduser, IFormFile Photo)
        {
            DataAccess da = new DataAccess(_context);
            bool result = da.GetUsersByEmail(enduser.Email); //boolin result on joko false tai true

            if (result == false) //jos result on sama kuin false eli vastaavuutta ei löytynyt
            {
                string photourl = AddPhotoInContainer(Photo);
                enduser.Photo = photourl;
                enduser.Password = Hash(enduser.Password);
                if (enduser.Userrole == null)
                {
                    enduser.Userrole = "käyttäjä";
                }
                da.CreateUser(enduser);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Failed = true;
                return View();
            }
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
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var enduser = new DataAccess(_context).GetUserById(userid);
            if (enduser == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(enduser);
        }


        [HttpPost]
        public IActionResult Edit(Enduser enduser, IFormFile Photo)
        {
            DataAccess da = new DataAccess(_context);
            enduser.Photo = AddPhotoInContainer(Photo, da.GetCurrentPhotoUrl(enduser.Userid));
            if (enduser.Password != null)
            {
                enduser.Password = Hash(enduser.Password);
            }
            da.EditUser(enduser);
            return RedirectToAction("Profile", new { Id = enduser.Userid });
        }

        public IActionResult Delete(Enduser Eu)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            DataAccess da = new DataAccess(_context);
            var enduser = da.GetUserById(userid);
            if (enduser == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                da.DeleteUser(enduser);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lisää Blobiin uuden kuvan ja poistaa samalla vanhan, jos sellainen on olemassa.
        /// Myös vanhan kuvan thumbnailversio poistetaan.
        /// </summary>
        /// <param name="Photo">Uusi kuva</param>
        /// <param name="photoUrl">Alkuperäisen thumbnailkuvan URL</param>
        /// <returns></returns>
        [HttpPost]
        private string AddPhotoInContainer(IFormFile Photo, string photoUrl = null)
        {
            if (Photo == null)
            {
                return photoUrl;
            }
            using Stream imageStream = Photo.OpenReadStream();
            StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
            BlobServiceClient service = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
            BlobContainerClient container = service.GetBlobContainerClient(photoBlob);
            string photoname = Guid.NewGuid().ToString();
            if (photoUrl != null)
            {
                photoname = photoUrl.Substring(photoUrl.LastIndexOf('/') + 1);
            }
            BlobClient blob = container.GetBlobClient(photoname);
            blob.Upload(imageStream, true);
            //function käynnistyy...
            container = service.GetBlobContainerClient(thumbnailPhotoBlob);
            blob = container.GetBlobClient(photoname);
            //if (blob.Exists())
            //{
            return blob.Uri.ToString();
            //}
            //container = service.GetBlobContainerClient(photoBlob);
            //blob = container.GetBlobClient(photoname);
            //return blob.Uri.ToString();
        }

        /// <summary>
        /// Muokkaa salasanan eri muotoon, jottei näy suoraan tietokannassa.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
