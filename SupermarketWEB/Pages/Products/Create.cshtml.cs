using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketWEB.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SupermarketContext _context;

        public CreateModel(SupermarketContext context, ILogger<CreateModel> logger)
        {
            _context = context;
        }

        public class FormProducts
        {
            public int Id { get; set; }
            public string Name { get; set; }

            [Column(TypeName = "decimal(6,2)")]
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public int CategoryId { get; set; }
        }

        [BindProperty]
        public FormProducts Product { get; set; } = new FormProducts();

        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync(); // Carga las categorías
            return Page(); // Devuelve la página
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Encuentra la categoría correspondiente y asígnala a Product.Category
            //Product.Category = Categories.FirstOrDefault(c => c.Id == Product.CategoryId);


            Console.WriteLine($"Product Details: Id={Product.Id}, Name={Product.Name}, Price={Product.Price}, Stock={Product.Stock}, CategoryId={Product.CategoryId}");

            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync(); // Carga nuevamente las categorías si el modelo no es válido
                return Page(); // Devuelve la página para mostrar errores
            }

            var product = new Product
            {
                Name = Product.Name,
                Price = Product.Price,
                Stock = Product.Stock,
                CategoryId = Product.CategoryId
            };


            _context.Products.Add(product); // Agrega el nuevo producto al contexto
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return RedirectToPage("./Index"); // Redirige a la página de índice después de la creación
        }
    }
}
