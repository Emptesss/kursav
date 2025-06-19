// Загрузка корзины из LocalStorage
let cart = JSON.parse(localStorage.getItem('cart')) || [];

// Отображение товаров в корзине
function renderCart() {
    const cartItems = document.getElementById('cart-items');
    const totalPrice = document.getElementById('total-price');
    const totalItemsPrice = document.getElementById('total-items-price');
    
    if (cart.length === 0) {
        cartItems.innerHTML = '<p class="empty-cart">Ваша корзина пуста</p>';
        totalPrice.textContent = '0 ₽';
        totalItemsPrice.textContent = '0 ₽ (0 товаров)';
        return;
    }

    // Отрисовка товаров
    cartItems.innerHTML = cart.map(item => `
        <div class="cart-item" data-id="${item.id}">
            <div class="cart-item-image">
                <img src="${item.image}" alt="${item.name}" loading="lazy">
            </div>
            <div class="cart-item-details">
                <h3 class="cart-item-title">${item.name}</h3>
                <div class="cart-item-price">${item.price} ₽</div>
                <div class="cart-item-actions">
                    <div class="quantity-control">
                        <button class="decrease">-</button>
                        <span>${item.quantity}</span>
                        <button class="increase">+</button>
                    </div>
                    <div class="remove-item">Удалить</div>
                </div>
            </div>
        </div>
    `).join('');

    // Подсчет суммы
    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
    
    totalPrice.textContent = `${total} ₽`;
    totalItemsPrice.textContent = `${total} ₽ (${totalItems} ${getItemsWord(totalItems)})`;

    // Обновление счетчика в шапке
    updateCartCounter();
}

// Обновление счетчика
function updateCartCounter() {
    const count = cart.reduce((sum, item) => sum + item.quantity, 0);
    const counter = document.querySelector('.cart-counter');
    if (counter) counter.textContent = count;
}

// Обработчики событий
document.addEventListener('click', function(e) {
    const cartItem = e.target.closest('.cart-item');
    if (!cartItem) return;

    const id = parseInt(cartItem.dataset.id);
    const item = cart.find(item => item.id === id);

    // Увеличение количества
    if (e.target.classList.contains('increase')) {
        item.quantity += 1;
    }

    // Уменьшение количества
    if (e.target.classList.contains('decrease')) {
        item.quantity = Math.max(1, item.quantity - 1);
    }

    // Удаление товара
    if (e.target.classList.contains('remove-item')) {
        cart = cart.filter(item => item.id !== id);
    }

    // Сохранение и обновление
    localStorage.setItem('cart', JSON.stringify(cart));
    renderCart();
});

// Оформление заказа
document.getElementById('checkout-btn')?.addEventListener('click', function() {
    if (cart.length === 0) {
        alert('Ваша корзина пуста!');
        return;
    }
    
    alert('Заказ оформлен! Спасибо за покупку :)');
    cart = [];
    localStorage.setItem('cart', JSON.stringify(cart));
    renderCart();
});

// Вспомогательные функции
function getItemsWord(count) {
    const lastDigit = count % 10;
    const lastTwoDigits = count % 100;
    
    if (lastTwoDigits >= 11 && lastTwoDigits <= 19) return 'товаров';
    if (lastDigit === 1) return 'товар';
    if (lastDigit >= 2 && lastDigit <= 4) return 'товара';
    return 'товаров';
}

// Инициализация
document.addEventListener('DOMContentLoaded', renderCart);