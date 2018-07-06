// Hero
export default (function () {
    let last_known_scroll_position = 0;
    let ticking = false;
    const screenHeight = window.innerHeight - 60;

    function hideHeader(scrollPos) {
        let hero = document.querySelector('.hero');
        let percent = scrollPos / screenHeight;

        if (hero) {
            if (percent >= 1) {
                hero.style.opacity = 0;
            } else {
                hero.style.opacity = 1 - percent;
            }
        }
    }

    window.addEventListener('scroll', function (e) {
        last_known_scroll_position = window.scrollY;

        if (!ticking) {
            window.requestAnimationFrame(function () {
                hideHeader(last_known_scroll_position);
                ticking = false;
            });

            ticking = true;
        }
    });
}());
