using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // --- GÜNCELLENEN LİSTELEME VE FİLTRELEME ALANI ---
        public async Task<IActionResult> Index(string searchText)
        {
            List<Category> values;

            // Eğer arama kutusu boşsa tümünü getir, doluysa senin filtre metodunu çalıştır
            if (string.IsNullOrEmpty(searchText))
            {
                values = await _categoryService.TGetAllAsync();
            }
            else
            {
                // İşte burası senin Business katmanındaki Generic yapın!
                // Lambda sorgusunu (x => ...) doğrudan filtre metoduna paslıyoruz.
                values = await _categoryService.TGetByFilterAsync(x =>
                    x.CategoryName.ToLower().Contains(searchText.ToLower()));
            }

            // Kullanıcının ne aradığını kutuda tutmak için geri gönderiyoruz
            ViewBag.SearchText = searchText;

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _categoryService.TCreateAsync(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _categoryService.TDeleteAsync(id);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            await _categoryService.TUpdateAsync(category.CategoryID, category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoryAttributes(string id)
        {
            var category = await _categoryService.TGetByIdAsync(id);
            return Json(category.AttributeTemplate);
        }
    }
}