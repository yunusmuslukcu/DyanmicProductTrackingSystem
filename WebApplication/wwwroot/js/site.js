// --- KARANLIK MOD YÖNETİMİ ---

document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.getElementById('theme-toggle');
    const themeIcon = document.getElementById('theme-icon');
    const themeText = themeToggle.querySelector('span');

    // 1. Hafızadaki temayı kontrol et
    const currentTheme = localStorage.getItem('theme');

    // Eğer hafızada 'dark' varsa sayfaya uygula
    if (currentTheme === 'dark') {
        document.documentElement.setAttribute('data-theme', 'dark');
        themeIcon.classList.replace('fa-moon', 'fa-sun');
        themeText.textContent = 'Aydınlık Mod';
    }

    // 2. Butona tıklanınca olacaklar
    themeToggle.addEventListener('click', () => {
        let theme = document.documentElement.getAttribute('data-theme');

        if (theme === 'dark') {
            // Karanlıktan Aydınlığa Geçiş
            document.documentElement.setAttribute('data-theme', 'light');
            localStorage.setItem('theme', 'light');
            themeIcon.classList.replace('fa-sun', 'fa-moon');
            themeText.textContent = 'Karanlık Mod';
        } else {
            // Aydınlıktan Karanlığa Geçiş
            document.documentElement.setAttribute('data-theme', 'dark');
            localStorage.setItem('theme', 'dark');
            themeIcon.classList.replace('fa-moon', 'fa-sun');
            themeText.textContent = 'Aydınlık Mod';
        }
    });

    // --- MOBİL HAMBURGER MENÜ ---
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (sidebarToggle && sidebar && overlay) {
        // Hamburger butonuna tıkla → sidebar aç/kapat
        sidebarToggle.addEventListener('click', () => {
            sidebar.classList.toggle('open');
            overlay.style.display = sidebar.classList.contains('open') ? 'block' : 'none';
        });

        // Overlay'e tıkla → sidebar kapat
        overlay.addEventListener('click', () => {
            sidebar.classList.remove('open');
            overlay.style.display = 'none';
        });

        // Sidebar içindeki bir linke tıklayınca → sidebar kapat (sayfa geçişi için)
        sidebar.querySelectorAll('a').forEach(link => {
            link.addEventListener('click', () => {
                if (window.innerWidth <= 768) {
                    sidebar.classList.remove('open');
                    overlay.style.display = 'none';
                }
            });
        });
    }
});
