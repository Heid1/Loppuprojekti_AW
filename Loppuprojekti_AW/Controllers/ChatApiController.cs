using Loppuprojekti_AW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<ChatApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ChatApiController>/5
        [HttpGet("{id1}/{id2}")]
        public List<Message> Get(int id1, int id2)
        {
            return _context.Messages.Where(m => (m.Senderid == id1 && m.Receiverid == id2) || (m.Senderid == id2 && m.Receiverid == id1)).OrderBy(m => m.Sendtime).ToList();
        }
    }
}
