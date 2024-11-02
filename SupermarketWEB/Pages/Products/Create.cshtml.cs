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
            Categories = await _context.Categories.ToListAsync(); // Carga las categor�as
            return Page(); // Devuelve la p�gina
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Encuentra la categor�a correspondiente y as�gnala a Product.Category
            Product.Category = Categories.FirstOrDefault(c => c.Id == Product.CategoryId);

            Console.WriteLine($"Product Details: Id={Product.Id}, Name={Product.Name}, Price={Product.Price}, Stock={Product.Stock}, CategoryId={Product.CategoryId}");

            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync(); // Carga nuevamente las categor�as si el modelo no es v�lido
                return Page(); // Devuelve la p�gina para mostrar errores
            }

            _context.Products.Add(Product); // Agrega el nuevo producto al contexto
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return RedirectToPage("./Index"); // Redirige a la p�gina de �ndice despu�s de la creaci�n
        }
    }
}
