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
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Enduser eu, IFormFile Photo)
        {
            DataAccess da = new DataAccess(_context);
            string photourl = AddPhotoInContainer(Photo);
            da.CreateUser(eu);
            return RedirectToAction("Index", "Home");
        }
        //NÄMÄ LISÄTTY
        [HttpPost]
        private string AddPhotoInContainer(IFormFile Photo)
        {
            if (Photo == null)
            {
                return null;
            }
            using Stream imageStream = Photo.OpenReadStream();
            StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
            BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(blobName);
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
                var enduser = DataAccess.GetUserById(userid);
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
            var id = HttpContext.Session.GetInt32("userid");
            var user = DataAccess.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            return View(user);
        }


        [HttpPost] //editoidaan henkilöä ja lähetetaan se
        public IActionResult Edit(Enduser Eu)
        {
            DataAccess.EditUser(Eu);
            return View(Eu);

        }

        public IActionResult Delete(Enduser Eu)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var user = DataAccess.GetUserById(userid);
            if (user == null)
            {
                return RedirectToAction("Virhe", "Home");
            }
            else
            {
                DataAccess.DeleteUser(user);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }


        }
    }
}
