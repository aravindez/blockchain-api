using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using blockchainapi.Models;
using blockchainapi.Services;

namespace blockchainapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        //private readonly BlockContext _context;
        private readonly BlockService _service;

        //public BlockController(BlockContext context)
        public BlockController(BlockService service)
        {

            _service = service;

        }

        // GET: api/block
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<BlockItem>>> GetBlockItems()
        public List<BlockItem> GetBlockItems()
        {
            //return await _context.BlockItems.ToListAsync();
            return _service.GetBlocks();
        }

        [HttpGet("len")]
        public int GetBlockItemsLen()
        {
            return _service.GetBlocks().Count;
        }

        // GET: api/block/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<BlockItem>> GetBlockItem(long id)
        public BlockItem GetBlockItem(int id)
        {
            return _service.GetBlock(id);
        }

        // GET pending_blocks
        [Route("GetPendingBlocks")]
        [HttpGet]
        public List<BlockItem> GetPendingBlocks(int user_id)
        {
            return _service.GetPendingBlocks(user_id);
        }

        // POST init block
        [Route("PostInitBlock")]
        [HttpPost]
        public bool PostInitBlock(BlockItem item)
        {
            return _service.PostInitBlock(item);
        }

        // POST new block
        [Route("PostNewBlock")]
        [HttpPost]
        public int PostNewBlock(BlockItem item)
        {
            return _service.PostNewBlock(item);
        }

        // POST validation
        [Route("PostValidation")]
        [HttpPost]
        public bool PostValidation(ValidationItem validation)
        {
            return _service.PostValidation(validation);
        }

        // GET validation
        [Route("GetValidation")]
        [HttpGet]
        public String GetValidation(int block_id)
        {
            return _service.GetValidation(block_id);
        }
    }
}
