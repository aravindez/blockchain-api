using System;
using Microsoft.AspNetCore.Mvc;
using blockchainapi.Models;
using blockchainapi.Services;
using System.Collections.Generic;

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

        // GET: api/user
        [Route("GetUsers")]
        [HttpGet]
        public List<UserItem> GetUsers(int user_id)
        {
            return _service.GetUsers(user_id);
        }

        // GET: api/user
        [Route("GetUserGroups")]
        [HttpGet]
        public List<GroupItem> GetUserGroups(int user_id)
        {
            return _service.GetGroups(user_id);
        }

        // GET: api/user
        [Route("GetGroups")]
        [HttpGet]
        public List<GroupItem> GetGroups()
        {
            return _service.GetGroups();
        }

        // GET: api/user
        [Route("GetGroupUsers")]
        [HttpGet]
        public List<UserItem> GetGroupUsers(int group_id, bool notIn)
        {
            return _service.GetGroupUsers(group_id, notIn);
        }

        //POST: api/user
        [Route("PostUser")]
        [HttpPost]
        public bool PostUser(UserItem item)
        {
            return _service.PostUser(item);
        }

        //POST: api/user
        [Route("PostUserGroup")]
        [HttpPost]
        public bool PostUserGroup(UserGroup ug)
        {
            return _service.PostUserGroup(ug);
        }

        //POST: api/user
        [Route("PostGroup")]
        [HttpPost]
        public bool PostGroup(GroupItem group)
        {
            return _service.PostGroup(group);
        }
    }
}