using System;
using Microsoft.AspNetCore.Mvc;
using blockchainapi.Models;
using blockchainapi.Services;
using System.Collections.Generic;

namespace blockchainapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChainController : ControllerBase
    {
        private readonly ChainService _service;

        public ChainController(ChainService service)
        {
            _service = service;
        }

        // GET: api/chain
        [Route("GetChainItem")]
        [HttpGet]
        public ChainItem GetChainItem(int id)
        {
            return _service.GetChainItem(id);
        }

        //GET: api/chain
        [Route("GetChainItems")]
        [HttpGet]
        public List<ChainItem> GetChainItems(int user_id)
        {
            return _service.GetChainItems(user_id);
        }

        //GET: api/chain
        [Route("GetChain")]
        [HttpGet]
        public List<BlockItem> GetChain(int chain_id, bool store = false)
        {
            return _service.GetChain(chain_id, store);
        }

        //GET: api/chain
        [Route("GetValidChain")]
        [HttpGet]
        public List<BlockItem> GetValidChain(int chain_id, bool store = false)
        {
            return _service.GetChain(chain_id, store);
        }

        //POST: api/chain
        [Route("PostChainItem")]
        [HttpPost]
        public int PostChainItem(ChainItem item)
        {
            return _service.PostChain(item);
        }
    }
}