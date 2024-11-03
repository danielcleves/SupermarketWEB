using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.PayModes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly SupermarketContext _context;
        public CreateModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PayMode PayMode { get; set; } = new PayMode();
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PayModes.Add(PayMode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
