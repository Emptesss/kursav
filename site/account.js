document.addEventListener('DOMContentLoaded', function() {
    // Инициализация данных пользователя
    let userData = JSON.parse(localStorage.getItem('userData')) || {
        name: "Имя",
        email: "name@example.com",
        phone: "+7 (123) 456-78-90",
        favorites: [1, 3] // ID избранных товаров
    };

    // Инициализация заказов
    let orders = JSON.parse(localStorage.getItem('orders')) || [
        {
            id: 1,
            date: "15.05.2023",
            status: "completed",
            total: 2450,
            items: [
                { id: 1, name: "Нежный жасмин", price: 450, quantity: 1, image: "images/tea1.jpg" }
            ]
        }
    ];

    // Загрузка данных профиля
    loadProfile();
    // Загрузка заказов
    loadOrders();
    // Загрузка избранных товаров
    loadFavorites();

    // Обработчики событий
    setupEventListeners();

    function loadProfile() {
        document.querySelector('.profile-form input[name="name"]').value = userData.name;
        document.querySelector('.profile-form input[name="email"]').value = userData.email;
        document.querySelector('.profile-form input[name="phone"]').value = userData.phone;
    }

    async function loadFavorites() {
        try {
            const response = await fetch('db.json');
            const data = await response.json();
            const favorites = data.products.filter(product => 
                userData.favorites.includes(product.id)
            );

            const favoritesGrid = document.querySelector('.favorites-grid');
            if (favoritesGrid) {
                favoritesGrid.innerHTML = favorites.map(product => `
                    <div class="product-card">
                        <div class="product-image">
                            <img src="${product.images[0]}" alt="${product.name}" loading="lazy">
                        </div>
                        <div class="product-info">
                            <h3>${product.name}</h3>
                            <p>${product.price} ₽</p>
                            <button class="btn add-to-cart" data-id="${product.id}">В корзину</button>
                            <button class="btn-outline remove-favorite" data-id="${product.id}">Удалить</button>
                        </div>
                    </div>
                `).join('');
            }
        } catch (error) {
            console.error('Ошибка загрузки избранного:', error);
        }
    }

    function loadOrders() {
        const ordersContainer = document.querySelector('#orders .order-card');
        if (ordersContainer) {
            ordersContainer.innerHTML = orders.map(order => `
                <div class="order">
                    <div class="order-header">
                        <span class="order-date">${order.date}</span>
                        <span class="order-status ${order.status}">${
                            order.status === 'completed' ? 'Выполнен' : 'В обработке'
                        }</span>
                        <span class="order-total">${order.total} ₽</span>
                    </div>
                    <div class="order-items">
                        ${order.items.map(item => `
                            <div class="order-item">
                                <img src="${item.image}" alt="${item.name}">
                                <div class="item-info">
                                    <h4>${item.name}</h4>
                                    <p>${item.quantity} × ${item.price} ₽</p>
                                </div>
                            </div>
                        `).join('')}
                    </div>
                    <a href="#" class="btn-outline repeat-order" data-id="${order.id}">Повторить заказ</a>
                </div>
            `).join('');
        }
    }

    function setupEventListeners() {
        // Сохранение профиля
        document.querySelector('.profile-form').addEventListener('submit', function(e) {
            e.preventDefault();
            userData.name = this.querySelector('input[name="name"]').value;
            userData.email = this.querySelector('input[name="email"]').value;
            userData.phone = this.querySelector('input[name="phone"]').value;
            localStorage.setItem('userData', JSON.stringify(userData));
            alert('Данные успешно сохранены!');
        });

        // Удаление из избранного
        document.addEventListener('click', function(e) {
            if (e.target.classList.contains('remove-favorite')) {
                const productId = parseInt(e.target.dataset.id);
                userData.favorites = userData.favorites.filter(id => id !== productId);
                localStorage.setItem('userData', JSON.stringify(userData));
                loadFavorites();
            }
        });

        // Переключение вкладок
        document.querySelectorAll('.account-menu a').forEach(link => {
            link.addEventListener('click', function(e) {
                e.preventDefault();
                document.querySelectorAll('.account-menu a').forEach(item => {
                    item.classList.remove('active');
                });
                this.classList.add('active');
                
                document.querySelectorAll('.tab-content').forEach(tab => {
                    tab.classList.remove('active');
                });
                
                const tabId = this.getAttribute('href');
                document.querySelector(tabId).classList.add('active');
            });
        });
    }
});