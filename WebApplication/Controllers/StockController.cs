using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IProductService _productService;

        public StockController(IStockService stockService, IProductService productService)
        {
            _stockService = stockService;
            _productService = productService;
        }
        //Listeleme ve Filtreleme işlemi!
        public async Task<IActionResult> Index(string searchText)
        {
            List<Stock> values;
            var products = await _productService.TGetAllAsync();
            ViewBag.Products = products;

            if (string.IsNullOrEmpty(searchText))
            {
                values = await _stockService.TGetAllAsync();
            }
            else
            {

                var filteredProductIds = products
                    .Where(p => p.ProductName.ToLower().Contains(searchText.ToLower()))
                    .Select(p => p.ProductID)
                    .ToList();
                values = await _stockService.TGetByFilterAsync(x => filteredProductIds.Contains(x.ProductId));
            }

            ViewBag.SearchText = searchText;
            return View(values);
        }

        //Ekleme işlemşnde önce listeliyoruz ekleme ekranına!
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.TGetAllAsync();
            ViewBag.ProductList = (from x in products
                                   select new SelectListItem
                                   {
                                       Text = x.ProductName + " " +
                                              (x.DynamicAttributes != null && x.DynamicAttributes.Any()
                                               ? "(" + string.Join(", ", x.DynamicAttributes.Select(a => $"{a.Key}: {a.Value}")) + ")"
                                               : ""),
                                       Value = x.ProductID.ToString()
                                   }).ToList();

            return View();
        }

        //Ekleme işlemi veri tabanına kaydediliyor!
        [HttpPost]
        public async Task<IActionResult> Create(Stock stock)
        {
            await _stockService.TCreateAsync(stock);
            return RedirectToAction("Index");
        }

        //Güncelleme işleminde önce ekrana verileri getiriyoruz.
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await _stockService.TGetByIdAsync(id);
            var products = await _productService.TGetAllAsync();

            ViewBag.ProductList = (from x in products
                                   select new SelectListItem
                                   {
                                       Text = x.ProductName + " " +
                                              (x.DynamicAttributes != null && x.DynamicAttributes.Any()
                                               ? "(" + string.Join(", ", x.DynamicAttributes.Select(a => $"{a.Key}: {a.Value}")) + ")"
                                               : ""),
                                       Value = x.ProductID.ToString(),
                                       Selected = x.ProductID == value.ProductId
                                   }).ToList();

            return View(value);
        }

        //Güncelleme işleminde verileri veri tabanına gönderiyoruz!
        [HttpPost]
        public async Task<IActionResult> Update(Stock stock)
        {
            await _stockService.TUpdateAsync(stock.StockID, stock);
            return RedirectToAction("Index");
        }

        //Silme işlemi!
        public async Task<IActionResult> Delete(string id)
        {
            await _stockService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}