using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Products
{
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
        public Product Product { get; set; } = default!;

        public List<Category> Categories { get; set; } = new List<Category>();

        // Método para cargar los datos del producto en la página
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

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

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
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
