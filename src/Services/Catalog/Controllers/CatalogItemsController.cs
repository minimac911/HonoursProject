using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using Catalog.Models;

namespace Catalog.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogItemsController : Controller
    {
        private readonly CatalogContext _context;

        public CatalogItemsController(CatalogContext context)
        {
            _context = context;
        }

        // GET: All catalog items
        [HttpGet]
        public async Task<IList<CatalogItem>> GetCatalog()
        {
            // TODO: add pagination 
            return await _context.CatalogItems.ToListAsync();
        }

        // POST: Create a new catalog itemm
        [HttpPost]
        public async Task<ActionResult<CatalogItem>> CreateCatalogItem(CatalogItem newItem)
        {
            // Add the new item to the catalog 
            _context.CatalogItems.Add(newItem);
            // save the item to the database
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCatalogItem), new { id = newItem.Id }, newItem );
        }

        // GET: A single catalog item
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetCatalogItem(int id)
        {
            // get a single catalog item
            var item = await _context.CatalogItems.FindAsync(id);

            // if item does not exist
            if(item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: Update Catalog Item
        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogItem>> PutCatalogItem(int id, CatalogItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            // Change the state of the original item to have the new changes
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                // Save changes to item
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool itemExists = await CatalogItemExists(id);

                if (!itemExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<bool> CatalogItemExists(int id)
        {
            var itemFound = await _context.CatalogItems.FindAsync(id);

            if(itemFound != null)
            {
                return false;
            }
            return true;
        }
    }
}
