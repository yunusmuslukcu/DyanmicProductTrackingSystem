using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IStockService _stockService;

        public OrderController(IOrderService orderService, IProductService productService, IStockService stockService)
        {
            _orderService = orderService;
            _productService = productService;
            _stockService = stockService;
        }


        public async Task<IActionResult> Index(string searchText)
        {
            List<Order> values;

            // 1. Ürün bilgilerini çekiyoruz (Tabloda ID yerine isim göstermek için şart)
            var products = await _productService.TGetAllAsync();
            ViewBag.Products = products;

            // 2. Arama kutusu dolu mu kontrol et
            if (string.IsNullOrEmpty(searchText))
            {
                // Kutuda bir şey yoksa her şeyi getir
                values = await _orderService.TGetAllAsync();
            }
            else
            {
                // Sadece Müşteri Adı üzerinden filtreleme yapıyoruz (Senin Generic Metodunla)
                string searchLower = searchText.ToLower();
                values = await _orderService.TGetByFilterAsync(x =>
                    x.CustomerName.ToLower().Contains(searchLower));
            }

            // Arama metnini View tarafındaki kutuda tutmak için gönderiyoruz
            ViewBag.SearchText = searchText;

            return View(values);
        }

        // 2. Yeni Sipariş Oluşturma (GET)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.TGetAllAsync();
            ViewBag.Products = products;
            List<SelectListItem> productList = (from x in products
                                                select new SelectListItem
                                                {
                                                    Text = x.ProductName + " " +
                                                           (x.DynamicAttributes != null && x.DynamicAttributes.Any()
                                                            ? "(" + string.Join(", ", x.DynamicAttributes.Select(a => $"{a.Key}: {a.Value}")) + ")"
                                                            : ""),
                                                    Value = x.ProductID.ToString()
                                                }).ToList();

            ViewBag.ProductList = productList;
            return View();
        }

        // 3. Yeni Sipariş Oluşturma (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            try
            {
                await _orderService.TCreateAsync(order);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var products = await _productService.TGetAllAsync();
                ViewBag.Products = products;
                ViewBag.ProductList = (from x in products
                                       select new SelectListItem
                                       {
                                           Text = x.ProductName + " (" + string.Join(", ", x.DynamicAttributes.Select(a => $"{a.Key}: {a.Value}")) + ")",
                                           Value = x.ProductID.ToString()
                                       }).ToList();

                return View(order);
            }
        }

        // 4. Sipariş Silme
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _orderService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }

        // 5. Sipariş Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var value = await _orderService.TGetByIdAsync(id);
            var products = await _productService.TGetAllAsync();
            ViewBag.Products = products;
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

        // 6. Sipariş Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Update(Order order)
        {
            await _orderService.TUpdateAsync(order.OrderID, order);
            return RedirectToAction("Index");
        }
    }
}