using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blockchainapi.Models;
using blockchainapi.Services;

namespace blockchainapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly BlockContext _context;
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        // GET: api/user
        [Route("GetUserItem")]
        [HttpGet]
        public UserItem GetUserItem(String uname, String pword)
        {
            return _service.GetUser(uname, pword);
        }

        //POST: api/block
        [HttpPost]
        public bool PostBlockItem(UserItem item)
        {
            return _service.PostUser(item);
        }
    }
}