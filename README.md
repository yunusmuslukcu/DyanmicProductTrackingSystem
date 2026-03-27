# Dynamic Product Tracking System

ASP.NET Core MVC ile geliştirdiğim bu proje, işletmelerin ürün, stok, sipariş ve kategori yönetimini tek bir panel üzerinden yapabilmesi için tasarlanmış bir yönetim sistemidir.Veri Tabanı olarak MongoDB Atlas, kimlik doğrulama için ASP.NET Core Identity kullandım.
Projeyi Azure App Service üzerinden canlıya aldım.

## Kullanılan Teknolojiler

- ASP.NET Core MVC (.NET 9)
- MongoDB Atlas
- ASP.NET Core Identity (MongoDbStores)
- Chart.js
- Bootstrap 5
- Azure App Service

## Mimari

Projeyi N-Katmanlı Mimari ile dört katmana böldüm, her katman ayrı bir .csproj projesidir.

- Entity: Varlık sınıfları (Product, Order, Stock, Category, AppUser, AppRole)
- DataAccess: Generic Repository ve MongoDB implamentasyonları
- Business: İş mantığı, Generic Manager ve servis sınıfları
- WebApplication: Controller'lar, View'lar, Program.cs

Controller'lar doğrudan veritabanına erişmez. Tüm işlemler Business katmanındaki servis arayüzleri üzerinden yürür. Bağımlılıklar Program.cs içinde Dependency Injection ile interface üzerinden tanımlanmıştır.

## Özellikler

Sistemde Admin ve Staff olmak üzere iki rol var. Roller uygulama başlarken otomatik oluşturuluyor. Tüm sayfalar giriş gerektiriyor, yalnızca Login ve Register sayfaları herkese açık. Silme işlemleri yalnızca Admin rolüyle mümkün.

Ürün yönetiminde ekleme, listeleme, güncelleme ve silme işlemlerinin yanı sıra ürün adı üzerinden arama yapılabiliyor. Bu projede asıl üzerinde durduğum konu her ürünün kendi dinamik özelliklerini taşıyabilmesidir. DynamicAttributes adında bir Dictionary yapısı kullandım. Bu sayede bir elektronik ürüne "RAM: 16GB" gibi bilgiler eklerken, bir giyim ürününe "Beden: L" gibi farklı özellikler atanabiliyor. Veritabanı şemasında herhangi bir değişiklik yapmak gerekmiyor.

Stok yönetiminde her ürün için ayrı kayıt tutuluyor ve kritik seviye takibi yapılıyor. Sipariş oluştururken stok yeterli değilse işlem reddediliyor, sipariş verilince stok düşülüyor, sipariş güncellenince fark yansıtılıyor, sipariş silinince ise adet stoğa iade ediliyor. Bu mantığı Business katmanında OrderManager sınıfı içinde yazdım.

Dashboard ekranında toplam ciro, sipariş sayısı, kritik stok adedi ve son 5 sipariş görüntüleniyor. Kategori bazlı ürün dağılımı Chart.js ile grafik olarak gösteriliyor.


## Deployment

Azure App Service üzerine deploy ettim. MongoDB Atlas cloud tabanlı olduğu için ek bir veritabanı sunucusu kurmadan direkt yayına aldım. Bağlantı bilgilerini Azure'un Application Settings bölümünden yönetiyorum.
