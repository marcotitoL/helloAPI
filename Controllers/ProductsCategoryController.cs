#nullable disable

namespace helloAPI.Controllers
{
    [Route("category")]
    [ApiController]
    public class ProductsCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

///<summary>Returns all categories</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductsCategory()
        {
            return await _context.ProductsCategory.Select(
                cat => new ProductCategoryDTO{
                    Id = cat.Guid,
                    CategoryName = cat.CategoryName,
                    ProductCount = _context.Products.Where(p=>p.CategoryId==cat.Id).Count()
                }
            ).ToListAsync();
        }

        // GET: api/ProductsCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryDTO>> GetProductsCategory(string id)
        {
            var productsCategory = await _context.ProductsCategory.Where( cat => cat.Guid == id).Select(
                cat => new ProductCategoryDTO{
                    Id = cat.Guid,
                    CategoryName = cat.CategoryName,
                    ProductCount = _context.Products.Where(p=>p.CategoryId==cat.Id).Count()
                }
            ).FirstOrDefaultAsync();

            if (productsCategory == null)
            {
                return NotFound();
            }

            return productsCategory;
        }

        // PUT: api/ProductsCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"),Authorize]
        public async Task<IActionResult> PutProductsCategory(string id, UpdateProductsCategoryDTO productsCategory)
        {
            /*if (id != productsCategory.Id)
            {
                return BadRequest();
            }*/

            var updatedProductsCategory = await _context.ProductsCategory.Where(cat => cat.Guid == id ).FirstOrDefaultAsync();

            updatedProductsCategory.CategoryName = productsCategory.CategoryName;

            _context.Entry(updatedProductsCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsCategoryExists(id))
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

        // POST: api/ProductsCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost,Authorize]
        public async Task<ActionResult<UpdateProductsCategoryDTO>> PostProductsCategory(UpdateProductsCategoryDTO productsCategory)
        {
            productsCategory.Id = Guid.NewGuid().ToString();
            _context.ProductsCategory.Add(new ProductsCategory(){
                Guid = productsCategory.Id,
                CategoryName = productsCategory.CategoryName
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductsCategory", new { id = productsCategory.Id }, productsCategory);
        }

        // DELETE: api/ProductsCategory/5
        [HttpDelete("{id}"),Authorize]
        public async Task<IActionResult> DeleteProductsCategory(string id)
        {
            var productsCategory = await _context.ProductsCategory.Where(cat=>cat.Guid==id).FirstOrDefaultAsync();
            if (productsCategory == null)
            {
                return NotFound();
            }

            _context.ProductsCategory.Remove(productsCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsCategoryExists(string id)
        {
            return _context.ProductsCategory.Any(e => e.Guid == id);
        }
    }
}
