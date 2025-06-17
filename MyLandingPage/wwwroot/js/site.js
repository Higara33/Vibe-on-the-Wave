window.scrollToSection = (id) => {
    const el = document.getElementById(id);
    if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};

// Запускает Bootstrap-карусель по её id
window.startCarousel = (carouselId) => {
    const el = document.getElementById(carouselId);
    if (el && window.bootstrap && bootstrap.Carousel) {
        // Инициализируем заново с теми же опциями
        new bootstrap.Carousel(el, {
            interval: 5000,
            ride: 'carousel',
            pause: false,
            wrap: true
        });
    }
};