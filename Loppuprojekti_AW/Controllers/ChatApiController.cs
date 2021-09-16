using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loppuprojekti_AW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        private readonly ILogger<ChatApiController> _logger;
        private readonly MoveoContext _context;

        public ChatApiController(ILogger<ChatApiController> logger, MoveoContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET api/<ChatApiController>/5
        [HttpGet("{id1}/{id2}")]
        public List<Message> Get(int id1, int id2)
        {
            return _context.Messages.Where(m => (m.Senderid == id1 && m.Receiverid == id2) || (m.Senderid == id2 && m.Receiverid == id1)).OrderBy(m => m.Sendtime).ToList();
        }

        [HttpGet("username/{id}")]
        public string GetUserName(int id)
        {
            return _context.Endusers.Where(u => u.Userid == id).Select(u => u.Username).FirstOrDefault();
        }

        [HttpGet("user/{id}")]
        public Enduser GetUser(int id)
        {
            return _context.Endusers.Where(u => u.Userid == id).FirstOrDefault();
        }

        [HttpPost()]
        public void Post([FromBody] Message message)
        {
            //Message msg = new Message() { Senderid=senderid, Messagebody=message, Receiverid=receiverid, Sendtime=DateTime.Now };
            message.Sendtime = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}
