#nullable disable

namespace helloAPI.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

///<summary>Returns all products</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetProducts( int page = 1, int totalPerPage = 5 )
        {

            int skippedInPages = ( ( page == 0 ? 1 : page ) - 1 ) * totalPerPage;

            return await _context.Products.Select( p => new ProductsDTO(){
                Id = p.Guid,
                Name = p.Name,
                Qty = p.Qty,
                Description = p.Description,
                Price = p.Price,
                Category =  _context.ProductsCategory.
                            Where( cat => cat.Id == p.CategoryId ).
                            Select( cat => new idDTO(){
                                        Id = cat.Guid
                                        } )
                            .FirstOrDefault(),
                Seller =  _context.UserDetails.
                        Where( ud => ud.Id == p.UserId ).
                        Select( u => new idDTO(){
                            Id = u.AspNetUserId
                        } ).FirstOrDefault(),
                Added = p.Date.ToString("yyyy-MM-dd HH:MM:ss"),
                Status = p.Status.ToString()
            })
            .Skip( skippedInPages )
            .Take( totalPerPage )
            .ToListAsync();

        }

        [HttpGet,Route("count")]
        public async Task<IActionResult> GetProductsCount(){
            return Ok( await _context.Products.CountAsync() );
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsDTO>> GetProducts(string id)
        {
            var products = await _context.Products.Where( p => p.Guid == id ).Select( p => new ProductsDTO(){
                Id = p.Guid,
                Name = p.Name,
                Qty = p.Qty,
                Description = p.Description,
                Price = p.Price,
                Category = _context.ProductsCategory.
                            Where( cat => cat.Id == p.CategoryId ).
                            Select( cat => new idDTO(){
                                        Id = cat.Guid,
                                        } )
                            .FirstOrDefault(),
                Seller = _context.UserDetails.
                        Where( ud => ud.Id == p.UserId ).
                        Select( ud => new idDTO(){
                            Id = ud.AspNetUserId
                        } ).FirstOrDefault(),
                Added = p.Date.ToString("yyyy-MM-dd HH:MM:ss"),
            }).FirstOrDefaultAsync();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"),Authorize]
        public async Task<IActionResult> PutProducts(string id, ViewProducts viewProducts)
        {
            /*if (id != viewProducts.Guid)
            {
                return BadRequest();
            }*/

            var updatedProduct = await _context.Products.Where( p => p.Guid == id ).FirstOrDefaultAsync();

            updatedProduct.Name = viewProducts.Name;
            updatedProduct.Description = viewProducts.Description;
            updatedProduct.Qty = viewProducts.Qty;
            updatedProduct.Price = viewProducts.Price;
            updatedProduct.CategoryId = await _context.ProductsCategory.
                                            Where( cat => cat.Guid == viewProducts.Category.Id  ).
                                            Select( cat => cat.Id ).SingleOrDefaultAsync();
            updatedProduct.UserId = await _context.UserDetails.Where( u => u.AspNetUserId == viewProducts.Seller.Id )
                                    .Select( u => u.Id ).SingleOrDefaultAsync();



            _context.Entry(updatedProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost,Authorize]
        public async Task<ActionResult<ViewProducts>> PostProducts(ViewProducts viewProducts)
        {
            viewProducts.Id = Guid.NewGuid().ToString();
            Products newProduct = new Products(){
                    Guid = viewProducts.Id,
                    Name = viewProducts.Name,
                    Description = viewProducts.Description,
                    Qty = viewProducts.Qty,
                    Price = viewProducts.Price,
                    Date = DateTime.Now,
                    CategoryId = await _context.ProductsCategory.Where(cat=>cat.Guid==viewProducts.Category.Id).Select( c => c.Id ).SingleOrDefaultAsync(),
                    productsCategory = await _context.ProductsCategory.Where(cat=>cat.Guid==viewProducts.Category.Id).FirstOrDefaultAsync(),
                    UserId = await _context.UserDetails.Where( u => u.AspNetUserId == viewProducts.Seller.Id ).Select( u => u.Id).SingleOrDefaultAsync(),
                    userDetails = await _context.UserDetails.Where( u => u.AspNetUserId == viewProducts.Seller.Id ).FirstOrDefaultAsync(),
                    Status = ProductStatus.Available
            };
                 _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = newProduct.Guid }, viewProducts );
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}"),Authorize]
        public async Task<IActionResult> DeleteProducts(string id)
        {
            var products = await _context.Products.Where( p => p.Guid == id ).FirstOrDefaultAsync();
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(string id)
        {
            return _context.Products.Any(e => e.Guid == id);
        }
    }
}
