document.addEventListener('DOMContentLoaded', function() {
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
                        <h3>${product.name}</h3>
                        <p>${product.description.substring(0, 60)}...</p>
                        <span class="product-price">${product.price} ₽</span>
                        <a href="product.html?id=${product.id}" class="btn">Подробнее</a>
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