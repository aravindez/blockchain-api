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
    public class BlockController : ControllerBase
    {
        //private readonly BlockContext _context;
        private readonly BlockService _service;

        //public BlockController(BlockContext context)
        public BlockController(BlockService service)
        {
            //_context = context;
            _service = service;
            /*
            if (_context.BlockItems.Count() == 0)
            {
                // Create a new BlockItem if collection is empty,
                // which means you can't delete all BlockItems.
                _context.BlockItems.Add(new BlockItem { });
                _context.SaveChanges();
            }
            //*/
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


        // POST new block
        [Route("PostBlockItem")]
        [HttpPost]
        public bool PostBlockItem(BlockItem item, int chain_id)
        {
            return _service.PostNewBlock(item, chain_id);
        }

        // POST validation
        [Route("PostValidation")]
        [HttpPost]
        public bool PostValidation(int user_id, int block_id, int chain_id, Boolean valid)
        {
            return _service.PostValidation(user_id, block_id, chain_id, valid);
        }

    }
}
