using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using static SupermarketWEB.Pages.Products.CreateModel;

namespace SupermarketWEB.Pages.Products
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly SupermarketContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(SupermarketContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public FormProducts ProductForm { get; set; } = new FormProducts();

        public List<Category> Categories { get; set; } = new List<Category>();

        // Método para cargar los datos del producto en la página
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargar el producto desde la base de datos y mapearlo a ProductForm
            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Mapear `Product` a `FormProducts`
            ProductForm = new FormProducts
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };

            Categories = await _context.Categories.ToListAsync(); // Cargar las categorías
            return Page();
        }

        // Método para manejar la solicitud de guardado
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync(); // Cargar nuevamente las categorías en caso de error de validación
                return Page();
            }

            // Buscar el producto en la base de datos
            var productToUpdate = await _context.Products.FindAsync(ProductForm.Id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades del producto en la base de datos
            productToUpdate.Name = ProductForm.Name;
            productToUpdate.Price = ProductForm.Price;
            productToUpdate.Stock = ProductForm.Stock;
            productToUpdate.CategoryId = ProductForm.CategoryId;

            try
            {
                await _context.SaveChangesAsync(); // Guardar los cambios
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(ProductForm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index"); // Redirige al índice después de guardar
        }

        // Método para verificar si el producto existe
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
