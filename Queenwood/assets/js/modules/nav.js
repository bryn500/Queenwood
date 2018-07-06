//Nav
export default (function () {
    let navOpen = false;
    const openMenuButton = document.querySelector('.js-menu');
    const slideMenu = document.querySelector('.js-nav');

    function openMenu() {
        navOpen = true;
        slideMenu.classList.add('visible');
        openMenuButton.classList.add('is-open');
    }

    function closeMenu() {
        navOpen = false;
        slideMenu.classList.remove('visible');
        openMenuButton.classList.remove('is-open');
    }

    openMenuButton.addEventListener('click', function (e) {
        e.preventDefault();
        if (navOpen) {
            closeMenu();
        } else {
            openMenu();
        }
    });

    document.addEventListener('keydown', function (evt) {
        evt = evt || window.event;
        if (evt.keyCode === 27) {
            closeMenu();
        }
    });
}());
