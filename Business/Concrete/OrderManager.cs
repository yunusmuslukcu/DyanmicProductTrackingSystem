using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OrderManager : GenericManager<Order>, IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Stock> _stockRepository;

        public OrderManager(IGenericRepository<Order> orderRepository, IGenericRepository<Stock> stockRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
            _stockRepository = stockRepository;
        }

        //Stokta bulunan adetten fazla ürün siparişi verilmek istendiğinde uyarı mesajı atılıyor!
        public override async Task TCreateAsync(Order entity)
        {

            var stock = (await _stockRepository.GetByFilterAsync(x => x.ProductId == entity.ProductId)).FirstOrDefault();
            if (stock == null)
            {
                throw new Exception("HATA: Bu ürüne ait bir stok kaydı sistemde bulunamadı!");
            }
            if (stock.Quantity < entity.Quantity)
            {
                throw new Exception($"Yetersiz Stok! Mevcut Stok: {stock.Quantity}, Sipariş Edilen Adet: {entity.Quantity}. Lütfen stok miktarını kontrol edin.");
            }
            stock.Quantity -= entity.Quantity;
            await _stockRepository.UpdateAsync(stock.StockID, stock);
            await _orderRepository.CreateAsync(entity);
        }

        //Üründe örneğin bir adet güncellemesi olduğunda bu adet güncellemesini stok alanına yansıtıyoruz!
        public override async Task TUpdateAsync(string id, Order entity)
        {
            var oldOrder = await _orderRepository.GetByIdAsync(id);
            if (oldOrder == null) throw new Exception("Sipariş bulunamadı!");
            var stock = (await _stockRepository.GetByFilterAsync(x => x.ProductId == entity.ProductId)).FirstOrDefault();
            if (stock == null) throw new Exception("Stok kaydı bulunamadı!");
            int quantityDifference = entity.Quantity - oldOrder.Quantity;

            if (quantityDifference > 0 && stock.Quantity < quantityDifference)
            {
                throw new Exception($"Yetersiz stok! Mevcut adet: {stock.Quantity}, Eklenmek istenen fark: {quantityDifference}. Güncelleme yapılamaz.");
            }
            stock.Quantity -= quantityDifference;
            await _stockRepository.UpdateAsync(stock.StockID, stock);
            await _orderRepository.UpdateAsync(id, entity);
        }

        //Silme işlemi olduğunda stoğa geri bir şekilde yansıtılıyor!

        public override async Task TDeleteAsync(string id)
        {
            // 1. Silinecek siparişi önce veritabanından buluyoruz (İçindeki Adet bilgisi lazım)
            var order = await _orderRepository.GetByIdAsync(id);

            if (order != null)
            {
                // 2. Bu siparişe ait stok kaydını buluyoruz
                var stock = (await _stockRepository.GetByFilterAsync(x => x.ProductId == order.ProductId)).FirstOrDefault();

                if (stock != null)
                {
                    // 3. İADE MANTIĞI: Sipariş silindiği için adedi stoğa geri ekliyoruz
                    stock.Quantity += order.Quantity;

                    // 4. Stok miktarını güncelliyoruz
                    await _stockRepository.UpdateAsync(stock.StockID, stock);
                }

                // 5. Stok iadesi tamamlandıktan sonra asıl siparişi siliyoruz
                await _orderRepository.DeleteAsync(id);
            }
        }
    }
}