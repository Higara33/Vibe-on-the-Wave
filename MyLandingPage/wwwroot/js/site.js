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

// ====================
// Sticky header on scroll
// ====================
window.addEventListener('scroll', () => {
    const hdr = document.getElementById('site-header');
    if (!hdr) return;

    if (window.scrollY > 50) {
        hdr.classList.add('scrolled');
    } else {
        hdr.classList.remove('scrolled');
    }
});

// =========================================
// Auto-close Bootstrap offcanvas on link tap
// =========================================
document.addEventListener('DOMContentLoaded', () => {
    const offcanvasEl = document.getElementById('offcanvasNav');
    if (offcanvasEl && window.bootstrap && bootstrap.Offcanvas) {
        const offcanvasInstance = bootstrap.Offcanvas.getOrCreateInstance(offcanvasEl);
        offcanvasEl.querySelectorAll('.nav-link').forEach(link => {
            link.addEventListener('click', () => {
                offcanvasInstance.hide();
            });
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    new Swiper('.mySwiper', {
        slidesPerView: 1,
        spaceBetween: 0,
        loop: true,
        centeredSlides: true,
        navigation: { nextEl: '.swiper-button-next', prevEl: '.swiper-button-prev' }
    });
});

window.initSwiper = function () {
    if (window._mySwiper) {
        window._mySwiper.destroy(true, true); // если уже инициализирован, пересоздаём
    }
    window._mySwiper = new Swiper('.mySwiper', {
        slidesPerView: 1,
        spaceBetween: 0,
        loop: true,
        centeredSlides: true,
        navigation: { nextEl: '.swiper-button-next', prevEl: '.swiper-button-prev' }
    });
};