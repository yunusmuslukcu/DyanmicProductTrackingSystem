using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // --- GÜNCELLENEN ÜRÜN LİSTELEME VE ARAMA (INDEX) ---
        public async Task<IActionResult> Index(string searchText)
        {
            List<Product> values;

            // Eğer arama kutusu boşsa tümünü getir, doluysa filtrele
            if (string.IsNullOrEmpty(searchText))
            {
                values = await _productService.TGetAllAsync();
            }
            else
            {
                // Senin Generic yapınla Ürün Adı üzerinden filtreliyoruz
                string searchLower = searchText.ToLower();
                values = await _productService.TGetByFilterAsync(x =>
                    x.ProductName.ToLower().Contains(searchLower));
            }

            // Aranan kelimeyi View'da tutmak için ViewBag'e atıyoruz
            ViewBag.SearchText = searchText;

            return View(values);
        }

        // Ürün ekleme sayfası (GET)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.TGetAllAsync();

            List<SelectListItem> categoryList = (from x in categories
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();
            ViewBag.CategoryList = categoryList;

            return View();
        }

        // Ürün ekleme işlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _productService.TCreateAsync(product);
            return RedirectToAction("Index");
        }

        // Güncellemede önce kullanıcı ekranına verileri listeliyoruz!
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await _productService.TGetByIdAsync(id);
            var categories = await _categoryService.TGetAllAsync();

            List<SelectListItem> categoryList = (from x in categories
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()
                                                 }).ToList();

            ViewBag.CategoryList = categoryList;
            return View(value);
        }

        // Ürün güncelleme işleminde kullanıcıdan gelen veriler gönderiliyor. (POST)
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            await _productService.TUpdateAsync(product.ProductID, product);
            return RedirectToAction("Index");
        }

        // Ürün Silme İşlemi
        public async Task<IActionResult> Delete(string id)
        {
            await _productService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}