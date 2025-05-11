using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index(string category, string farmerName, DateTime? fromDate, DateTime? toDate)
        {
            var user = await _userManager.GetUserAsync(User);

            var products = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
                if (farmer == null) return NotFound();

                products = products.Where(p => p.FarmerId == farmer.FarmerId);
            }

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.Contains(category));

            if (!string.IsNullOrEmpty(farmerName))
                products = products.Where(p => p.Farmer.Name.Contains(farmerName));

            if (fromDate.HasValue)
                products = products.Where(p => p.ProductionDate >= fromDate.Value);

            if (toDate.HasValue)
                products = products.Where(p => p.ProductionDate <= toDate.Value);

            return View(await products.ToListAsync());
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Employee"))
            {
                ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Name");
            }

            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Category,ProductionDate")] Product product)
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
                if (farmer == null) return Unauthorized();

                product.FarmerId = farmer.FarmerId;
            }

            if (User.IsInRole("Employee"))
            {
                // Get FarmerId from ViewData dropdown
                if (Request.Form.TryGetValue("FarmerId", out var farmerIdStr) && int.TryParse(farmerIdStr, out int farmerId))
                {
                    product.FarmerId = farmerId;
                }
                else
                {
                    ModelState.AddModelError("", "Please select a farmer.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Employee"))
            {
                ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Name", product.FarmerId);
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Name", product.FarmerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Category,ProductionDate,FarmerId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FarmerId"] = new SelectList(_context.Farmers, "FarmerId", "Name", product.FarmerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
