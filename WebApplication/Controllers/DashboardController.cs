using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IStockService _stockService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService; 

        // Mühendislik kuralı: İhtiyacımız olan tüm servisleri "Dependency Injection" ile içeri alıyoruz.
        public DashboardController(IOrderService orderService, IStockService stockService, IProductService productService, ICategoryService categoryService)
        {
            _orderService = orderService;
            _stockService = stockService;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.TGetAllAsync();
            var stocks = await _stockService.TGetAllAsync();
            var products = await _productService.TGetAllAsync();
            var categories = await _categoryService.TGetAllAsync(); // Kategorileri Çektik

            // --- Mevcut Hesaplamalar ---
            ViewBag.TotalSales = orders.Sum(x => x.TotalPrice).ToString("C2");
            ViewBag.OrderCount = orders.Count();
            ViewBag.CriticalStockCount = stocks.Count(x => x.Quantity <= x.CriticalLevel);
            ViewBag.ProductCount = products.Count();

            // --- KATEGORİ BAZLI ANALİZ (ÖĞRENME ALANI) ---

            // 1. Toplam Kategori Sayısı
            ViewBag.CategoryCount = categories.Count();

            // 2. Kategorileri View tarafında isimle eşlemek için listeyi gönderiyoruz
            ViewBag.AllCategories = categories;

            // 3. İleri Düzey: Hangi kategoride kaç ürün var? (İstatistik için)
            // Bu veriyi View'da grafik yaparken kullanacağız.
            ViewBag.CategoryStats = categories.Select(c => new {
                CategoryName = c.CategoryName,
                ProductCount = products.Count(p => p.CategoryID == c.CategoryID)
            }).ToList();

            ViewBag.LastOrders = orders.OrderByDescending(x => x.OrderDate).Take(5).ToList();
            ViewBag.AllProducts = products;

            return View();
        }
    }
}
