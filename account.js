document.addEventListener('DOMContentLoaded', function() {
  

    // Инициализация данных пользователя
    let userData = JSON.parse(localStorage.getItem('userData')) || {
        name: "Имя",
        email: "name@example.com",
        phone: "+7 (123) 456-78-90",
        favorites: [1, 3]
    };
     function updateUserProfileBlock() {
        // Обновляет имя и email в блоке user-profile
        const nameBlock = document.getElementById('user-name');
        const emailBlock = document.getElementById('user-email');
        if (nameBlock) nameBlock.textContent = userData.name;
        if (emailBlock) emailBlock.textContent = userData.email;
    }   

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
    // Загружаем аватар из userData, если есть
    const avatarImg = document.getElementById('avatar-img');
    if (userData.avatar) {
        avatarImg.src = userData.avatar;
    }

    // Обработка загрузки аватара
    const avatarInput = document.getElementById('avatar-input');
    if (avatarInput) {
        avatarInput.addEventListener('change', function(event) {
            const file = event.target.files[0];
            if (file && file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    avatarImg.src = e.target.result;
                    userData.avatar = e.target.result;
                    localStorage.setItem('userData', JSON.stringify(userData));
                }
                reader.readAsDataURL(file);
            }
        });
    }
    // Загрузка данных профиля
    loadProfile();
    // Загрузка заказов
    loadOrders();
    // Загрузка избранных товаров
    loadFavorites();

    // Обработчики событий
    setupEventListeners();

    // ======================= ФУНКЦИИ =======================

    function loadProfile() {
        document.querySelector('.profile-form input[name="name"]').value = userData.name;
        document.querySelector('.profile-form input[name="email"]').value = userData.email;
        document.querySelector('.profile-form input[name="phone"]').value = userData.phone;
        updateUserProfileBlock();
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
        const ordersContainer = document.querySelector('#orders .orders-list');
        if (ordersContainer) {
            ordersContainer.innerHTML = orders.map(order => `
                <div class="order-card">
                    <div class="order-header">
                        <span class="order-date">${order.date}</span>
                        <span class="order-status ${order.status}">
                            ${order.status === 'completed' ? 'Выполнен' : 'В обработке'}
                        </span>
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
        const profileForm = document.querySelector('.profile-form');
        if (profileForm) {
            profileForm.addEventListener('submit', function(e) {
                e.preventDefault();
                userData.name = this.querySelector('input[name="name"]').value;
                userData.email = this.querySelector('input[name="email"]').value;
                userData.phone = this.querySelector('input[name="phone"]').value;
                localStorage.setItem('userData', JSON.stringify(userData));
                showSuccess(profileForm, 'Профиль успешно сохранён!');
                updateUserProfileBlock();
            });
        }

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
        // Удаляем активные классы у всех ссылок
        document.querySelectorAll('.account-menu a').forEach(a => a.classList.remove('active'));
        this.classList.add('active');

        // Скрываем все вкладки
        document.querySelectorAll('.tab-content').forEach(tab => tab.classList.remove('active'));

        // Показываем нужную вкладку
        const tabId = this.getAttribute('href');
        const target = document.querySelector(tabId);
        if (target) target.classList.add('active');
    });
});
    }

    function showSuccess(form, message) {
        let msg = form.querySelector('.success-message');
        if (!msg) {
            msg = document.createElement('div');
            msg.className = 'success-message';
            msg.style.color = 'green';
            msg.style.marginBottom = '10px';
            form.prepend(msg);
        }
        msg.textContent = message;
        msg.style.display = 'block';
        setTimeout(() => {
            msg.style.display = 'none';
        }, 2000);
    }
 loadProfile();
    // Для автозаполнения формы при загрузке (если страница была перезагружена)
    window.addEventListener('DOMContentLoaded', loadProfile);

});