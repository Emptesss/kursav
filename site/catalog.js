// Глобальные переменные
let products = [];
let isInitialized = false;

// Основная функция инициализации
async function initCatalog() {
    try {
        // 1. Загрузка данных
        await loadProducts();
        
        // 2. Инициализация фильтров
        initFilters();
        
        // 3. Отметка успешной инициализации
        isInitialized = true;
    } catch (error) {
        console.error('Ошибка инициализации каталога:', error);
        showErrorMessage();
    }
}

// Загрузка данных из JSON с обработкой ошибок
async function loadProducts() {
    try {
        const response = await fetch('db.json');
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        
        if (!data.products || !Array.isArray(data.products)) {
            throw new Error('Invalid data format: expected products array');
        }
        
        products = data.products;
        safeRenderProducts(products);
    } catch (error) {
        console.error('Ошибка загрузки товаров:', error);
        throw error;
    }
}

// Безопасный рендеринг товаров
function safeRenderProducts(productsToRender) {
    try {
        const grid = document.getElementById('catalog-products');
        
        if (!grid) {
            throw new Error('Element #catalog-products not found');
        }
        
        if (!productsToRender || !Array.isArray(productsToRender)) {
            throw new Error('Invalid products data for rendering');
        }
        
        // Очищаем только если есть что очищать
        while (grid.firstChild) {
            grid.removeChild(grid.firstChild);
        }
        
        // Создаем фрагмент для эффективного добавления
        const fragment = document.createDocumentFragment();
        
        productsToRender.forEach(product => {
            if (!product.id || !product.name || !product.price) {
                console.warn('Invalid product data:', product);
                return;
            }
            
            const card = document.createElement('div');
card.className = 'product-card';
card.innerHTML = `
    <div class="product-image">
        <img src="${product.images?.[0] || 'images/placeholder.jpg'}" 
             alt="${product.name}" 
             loading="lazy"
             onerror="this.src='images/placeholder.jpg'">
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
`;
            fragment.appendChild(card);
        });
        
        grid.appendChild(fragment);
    } catch (error) {
        console.error('Ошибка рендеринга товаров:', error);
        showErrorMessage();
    }
    let userData = JSON.parse(localStorage.getItem('userData')) || {favorites: []};
document.querySelectorAll('.add-to-favorites').forEach(btn => {
    let id = parseInt(btn.dataset.id);
    const icon = btn.querySelector('i');
    if (userData.favorites.includes(id)) {
        icon.classList.remove('far');
        icon.classList.add('fas');
        btn.classList.add('active');
        btn.disabled = false; // звезда всегда доступна для повторного клика (если хочешь убирать из избранного)
    } else {
        icon.classList.add('far');
        icon.classList.remove('fas');
        btn.classList.remove('active');
        btn.disabled = false;
    }
});
}

// Инициализация фильтров
function initFilters() {
    try {
        const filterSelect = document.getElementById('type');
        const priceInput = document.getElementById('price');
        const resetBtn = document.getElementById('reset-filters');
        
        if (filterSelect) {
            filterSelect.addEventListener('change', safeFilterProducts);
        }
        
       if (priceInput) {
    priceInput.style.accentColor = 'var(--primary-color)';
    priceInput.addEventListener('input', function() {
        updatePriceDisplay();
        safeFilterProducts();
    });
}
        
        if (resetBtn) {
            resetBtn.addEventListener('click', () => {
                document.getElementById('type').value = 'all';
                document.getElementById('price').value = 1000;
                document.getElementById('stock').value = 'all';
                updatePriceDisplay();
                safeFilterProducts();
            });
        }
    } catch (error) {
        console.error('Ошибка инициализации фильтров:', error);
    }
}

// Безопасная фильтрация
function safeFilterProducts() {
    try {
        if (!isInitialized) return;
        
        const type = document.getElementById('type')?.value || 'all';
        const price = Number(document.getElementById('price')?.value || 1000);
        const stock = document.getElementById('stock')?.value || 'all';
        
        const filtered = products.filter(p => {
            return (type === 'all' || p.type === type) &&
                   (Number(p.price) <= price) &&
                   (stock === 'all' || p.stock === stock);
        });
            
        safeRenderProducts(filtered);
    } catch (error) {
        console.error('Ошибка фильтрации:', error);
    }
}

// Вспомогательные функции
function getProductType(type) {
    const types = {
        'green': 'Зеленый чай',
        'black': 'Черный чай',
        'herbal': 'Травяной чай',
        'fruit': 'Фруктовый чай'
    };
    return types[type] || 'Чай';
}

function formatPrice(price) {
    return Number(price) ? Number(price).toFixed(2) : price;
}

function escapeHTML(str) {
    return str.replace(/[&<>'"]/g, 
        tag => ({
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            "'": '&#39;',
            '"': '&quot;'
        }[tag]));
}

function updatePriceDisplay() {
    const priceInput = document.getElementById('price');
    const priceValue = document.getElementById('price-value');
    
    if (priceInput && priceValue) {
        priceValue.textContent = `0-${priceInput.value} ₽`;
    }
}

function showErrorMessage() {
    const grid = document.getElementById('catalog-products');
    if (grid) {
        grid.innerHTML = `
            <div class="error-message">
                <i class="fas fa-exclamation-triangle"></i>
                <p>Не удалось загрузить товары. Пожалуйста, попробуйте позже.</p>
                <button onclick="location.reload()">Обновить страницу</button>
            </div>
        `;
    }
}
document.addEventListener('click', function(e) {
    if (e.target.closest('.add-to-favorites')) {
        e.preventDefault();
        const btn = e.target.closest('.add-to-favorites');
        const productId = parseInt(btn.dataset.id);
        const icon = btn.querySelector('i');
        let userData = JSON.parse(localStorage.getItem('userData')) || {
            name: "Имя",
            email: "name@example.com",
            phone: "+7 (123) 456-78-90",
            favorites: []
        };
        if (userData.favorites.includes(productId)) {
            // Удалить из избранного
            userData.favorites = userData.favorites.filter(id => id !== productId);
            icon.classList.add('far');
            icon.classList.remove('fas');
            btn.classList.remove('active');
        } else {
            // Добавить в избранное
            userData.favorites.push(productId);
            icon.classList.remove('far');
            icon.classList.add('fas');
            btn.classList.add('active');
        }
        localStorage.setItem('userData', JSON.stringify(userData));
    }
});
// Запускаем инициализацию при загрузке страницы
document.addEventListener('DOMContentLoaded', initCatalog);