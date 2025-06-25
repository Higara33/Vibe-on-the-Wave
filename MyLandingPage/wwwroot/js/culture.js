// Управление культурой и синхронизация между вкладками
window.cultureManager = {
    // Ключ для localStorage
    storageKey: 'preferred-culture',
    
    // Получить текущую культуру из localStorage
    getCulture: function() {
        return localStorage.getItem(this.storageKey) || 'ru';
    },
    
    // Установить культуру в localStorage
    setCulture: function(culture) {
        localStorage.setItem(this.storageKey, culture);
        // Уведомляем все вкладки об изменении
        window.dispatchEvent(new StorageEvent('storage', {
            key: this.storageKey,
            newValue: culture,
            storageArea: localStorage
        }));
    },
    
    // Подписаться на изменения культуры в других вкладках
    onCultureChanged: function(callback) {
        window.addEventListener('storage', function(e) {
            if (e.key === window.cultureManager.storageKey && e.newValue) {
                callback(e.newValue);
            }
        });
    },
    
    // Навигация с учетом культуры
    navigateWithCulture: function(path, culture) {
        culture = culture || this.getCulture();
        
        // Удаляем возможные дублирующие слеши и нормализуем путь
        let normalizedPath = path.replace(/\/+/g, '/');
        if (normalizedPath.startsWith('/')) {
            normalizedPath = normalizedPath.substring(1);
        }
        
        // Создаем новый URL с культурой
        let newUrl;
        if (culture === 'ru') {
            // Для русского языка не добавляем префикс
            newUrl = '/' + normalizedPath;
        } else {
            // Для других языков добавляем префикс
            newUrl = '/' + culture + '/' + normalizedPath;
        }
        
        // Убираем лишние слеши в конце
        newUrl = newUrl.replace(/\/+$/, '') || '/';
        
        // Сохраняем культуру и переходим
        this.setCulture(culture);
        window.location.href = newUrl;
    },
    
    // Получить текущий путь без префикса культуры
    getCurrentPathWithoutCulture: function() {
        let path = window.location.pathname;
        
        // Убираем префиксы культур
        const cultures = ['en', 'de'];
        for (let culture of cultures) {
            const prefix = '/' + culture;
            if (path.startsWith(prefix)) {
                path = path.substring(prefix.length);
                break;
            }
        }
        
        return path || '/';
    }
};

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    // Синхронизируем культуру при загрузке страницы
    const savedCulture = window.cultureManager.getCulture();
    const currentUrl = window.location.pathname;
    
    // Проверяем, соответствует ли URL сохраненной культуре
    let urlCulture = 'ru'; // по умолчанию
    if (currentUrl.startsWith('/en/')) {
        urlCulture = 'en';
    } else if (currentUrl.startsWith('/de/')) {
        urlCulture = 'de';
    }
    
    // Если культуры не совпадают, перенаправляем
    if (savedCulture !== urlCulture) {
        const pathWithoutCulture = window.cultureManager.getCurrentPathWithoutCulture();
        window.cultureManager.navigateWithCulture(pathWithoutCulture, savedCulture);
    }
});
