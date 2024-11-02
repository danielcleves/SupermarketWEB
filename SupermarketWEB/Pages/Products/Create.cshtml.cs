using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly SupermarketContext _context;

        public CreateModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync(); // Carga las categorías
            return Page(); // Devuelve la página
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Encuentra la categoría correspondiente y asígnala a Product.Category
            Product.Category = Categories.FirstOrDefault(c => c.Id == Product.CategoryId);

            Console.WriteLine($"Product Details: Id={Product.Id}, Name={Product.Name}, Price={Product.Price}, Stock={Product.Stock}, CategoryId={Product.CategoryId}");

            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync(); // Carga nuevamente las categorías si el modelo no es válido
                return Page(); // Devuelve la página para mostrar errores
            }

            _context.Products.Add(Product); // Agrega el nuevo producto al contexto
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return RedirectToPage("./Index"); // Redirige a la página de índice después de la creación
        }
    }
}
