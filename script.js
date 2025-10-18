document.addEventListener('DOMContentLoaded', function() {
        // 🔊 Музыка по клику на логотип
    const audio = document.getElementById('site-audio');
    const logo = document.getElementById('logo');

    if (audio && logo) {
        let isPlaying = localStorage.getItem('musicPlaying') === 'true';

        if (isPlaying) {
            audio.play().catch(() => {});
        }

        logo.addEventListener('click', function(e) {
            e.preventDefault();
            if (isPlaying) {
                audio.pause();
                isPlaying = false;
            } else {
                audio.play().catch(() => {});
                isPlaying = true;
            }
            localStorage.setItem('musicPlaying', isPlaying);
        });
    }

    // 1. Слайдер героя
    const initSlider = () => {
        const slides = document.querySelectorAll('.slide');
        const prevBtn = document.querySelector('.prev-slide');
        const nextBtn = document.querySelector('.next-slide');

        if (!slides.length || !prevBtn || !nextBtn) return;

        let currentSlide = 0;
        let slideInterval;
        
        const showSlide = (index) => {
            slides.forEach((slide, i) => {
                slide.classList.toggle('active', i === index);
            });
        };

        const nextSlide = () => {
            currentSlide = (currentSlide + 1) % slides.length;
            showSlide(currentSlide);
        };

        const prevSlide = () => {
            currentSlide = (currentSlide - 1 + slides.length) % slides.length;
            showSlide(currentSlide);
        };

        // Инициализация
        showSlide(currentSlide);
        slideInterval = setInterval(nextSlide, 5000);

        // Обработчики событий
        nextBtn.addEventListener('click', () => {
            clearInterval(slideInterval);
            nextSlide();
            slideInterval = setInterval(nextSlide, 5000);
        });

        prevBtn.addEventListener('click', () => {
            clearInterval(slideInterval);
            prevSlide();
            slideInterval = setInterval(nextSlide, 5000);
        });
    };
    const searchIcon = document.getElementById('search-icon');
    const searchInput = document.getElementById('search-input');

    // Показать/спрятать поле поиска по клику на иконку
    if (searchIcon && searchInput) {
        searchIcon.addEventListener('click', function(e) {
            e.preventDefault();
            if (searchInput.style.display === 'none' || !searchInput.style.display) {
                searchInput.style.display = 'inline-block';
                searchInput.focus();
            } else {
                searchInput.value = '';
                searchInput.style.display = 'none';
            }
        });

        // Логика поиска по Enter
        searchInput.addEventListener('keydown', async function(e) {
            if (e.key === 'Enter') {
                const query = searchInput.value.trim().toLowerCase();
                if (!query) return;
                // Загрузка товаров (если на странице каталога, можно использовать переменную products)
                let productsArr = window.products;
                if (!productsArr || !Array.isArray(productsArr) || !productsArr.length) {
                    // Если products нет, пробуем загрузить заново
                    try {
                        const resp = await fetch('db.json');
                        const data = await resp.json();
                        productsArr = data.products || [];
                    } catch {
                        alert('Ошибка загрузки базы товаров');
                        return;
                    }
                }
                // Прямое совпадение по названию (без учёта регистра)
                const found = productsArr.find(p => p.name.toLowerCase() === query);
                if (found) {
                    window.location.href = `product.html?id=${found.id}`;
                } else {
                    // Можно сделать частичный поиск (по вхождению)
                    const partial = productsArr.find(p => p.name.toLowerCase().includes(query));
                    if (partial) {
                        window.location.href = `product.html?id=${partial.id}`;
                    } else {
                        alert('Товар не найден');
                    }
                }
            }
        });
    }

    // 2. Загрузка популярных товаров
    const loadProducts = async () => {
        const productsGrid = document.querySelector('.products-grid');
        if (!productsGrid) return;

        try {
            const response = await fetch('db.json');
            if (!response.ok) throw new Error('Network response was not ok');
            
            const data = await response.json();
            const popularProducts = data.products.slice(0, 4);

            productsGrid.innerHTML = popularProducts.map(product => `
    <div class="product-card">
        <div class="product-image">
            <img src="${product.images[0]}" alt="${product.name}" loading="lazy">

        </div>
       <div class="product-info">
        <h3>${escapeHTML(product.name)}</h3>
        <p>${getProductType(product.type)}</p>
        <span class="product-price">${formatPrice(product.price)} ₽</span>
        <a href="product.html?id=${product.id}" class="btn">Подробнее</a>
        <button class="favorite-star add-to-favorites" data-id="${product.id}" aria-label="В избранное" title="В избранное">
    <i class="far fa-star"></i>
</button>
    </div>
    </div>
`).join('');
        } catch (error) {
            console.error('Error loading products:', error);
            productsGrid.innerHTML = '<p>Не удалось загрузить товары. Пожалуйста, попробуйте позже.</p>';
        }
    };

    // 3. Обработчик формы подписки
    const setupSubscriptionForm = () => {
        const subscribeForm = document.querySelector('.subscribe-form');
        if (!subscribeForm) return;

        subscribeForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const emailInput = this.querySelector('input[type="email"]');
            if (!emailInput) return;

            const email = emailInput.value.trim();
            if (email) {
                alert(`Спасибо за подписку! На адрес ${email} будут приходить наши новости.`);
                this.reset();
            }
        });
    };

    // Инициализация всех модулей
    initSlider();
    loadProducts();
    setupSubscriptionForm();
});
