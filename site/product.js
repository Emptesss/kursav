let product;
let products = [];

async function loadProduct() {
    try {
        const id = new URLSearchParams(window.location.search).get('id');
        if (!id) throw new Error('Product ID not specified');
        
        const response = await fetch('db.json');
        if (!response.ok) throw new Error('Network response was not ok');
        
        const data = await response.json();
        products = data.products;
        product = products.find(p => p.id == id);
        
        if (!product) throw new Error('Product not found');
        
        renderProduct();
        loadRelatedProducts();
        setupAddToCart();
    } catch (error) {
        console.error('Error loading product:', error);
        window.location.href = 'catalog.html';
    }
}

// Отображение данных
function renderProduct() {
    document.querySelector('.breadcrumbs span').textContent = product.name;
    document.querySelector('.product-details h1').textContent = product.name;
    document.querySelector('.product-type').textContent = getProductType(product.type);
    
    document.querySelector('.price').innerHTML = product.oldPrice 
        ? `${product.price} ₽ <span class="old-price">${product.oldPrice} ₽</span>`
        : `${product.price} ₽`;
    
    document.querySelector('.description p').textContent = product.description;
    document.querySelector('.rating').innerHTML = getRatingStars(product.rating) + 
        ` <span class="reviews">(${product.reviews} отзывов)</span>`;
    
    // Галерея
    const mainImg = document.getElementById('main-img');
    mainImg.src = product.images[0];
    mainImg.alt = product.name;
    
    const thumbnails = document.querySelector('.thumbnails');
    thumbnails.innerHTML = product.images.slice(1).map(img => `
        <img src="${img}" alt="${product.name}" onclick="document.getElementById('main-img').src='${img}'">
    `).join('');
    
    // Характеристики
    document.querySelector('.description ul').innerHTML = `
        <li>Состав: ${product.composition}</li>
        <li>Вес: ${product.weight}</li>
        <li>Страна происхождения: ${product.country}</li>
    `;
}

// Загрузка похожих товаров
function loadRelatedProducts() {
    const grid = document.querySelector('.related-products .products-grid');
    if (!grid) return;
    
    const related = products.filter(p => p.type === product.type && p.id !== product.id).slice(0, 4);
    
    if (related.length === 0) {
        grid.parentElement.style.display = 'none';
        return;
    }
    
    grid.innerHTML = related.map(p => `
        <div class="product-card">
            <div class="product-image">
                <img src="${p.images[0]}" alt="${p.name}" loading="lazy">
            </div>
            <div class="product-info">
                <h3>${p.name}</h3>
                <p>${p.description.substring(0, 60)}...</p>
                <span class="product-price">${p.price} ₽</span>
                <a href="product.html?id=${p.id}" class="btn">Подробнее</a>
            </div>
        </div>
    `).join('');
}

// Настройка кнопки "В корзину"
function setupAddToCart() {
    const btn = document.querySelector('.add-to-cart');
    if (!btn) return;
    
    btn.addEventListener('click', () => {
        const quantity = parseInt(document.querySelector('.quantity input').value) || 1;
        addToCart(product, quantity);
    });
}

// Добавление в корзину
function addToCart(product, quantity = 1) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    
    const existingItem = cart.find(item => item.id === product.id);
    if (existingItem) {
        existingItem.quantity += quantity;
    } else {
        cart.push({
            id: product.id,
            name: product.name,
            price: product.price,
            image: product.images[0],
            quantity: quantity
        });
    }
    
    localStorage.setItem('cart', JSON.stringify(cart));
    alert(`${product.name} добавлен в корзину!`);
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

function getRatingStars(rating) {
    const fullStars = Math.floor(rating);
    const halfStar = rating % 1 >= 0.5 ? 1 : 0;
    const emptyStars = 5 - fullStars - halfStar;
    
    return '★'.repeat(fullStars) + 
           (halfStar ? '½' : '') + 
           '☆'.repeat(emptyStars);
}

// Инициализация
document.addEventListener('DOMContentLoaded', loadProduct);