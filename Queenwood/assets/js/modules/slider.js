export default (function () {
    const albums = document.querySelectorAll('.imgBoard__item--album');
    let sliders = [],
        i = 0;

    for (i = 0; i < albums.length; i++) {
        let album = albums[i];
        let images = Array.from(album.querySelectorAll('.album__item')).map(x => x.dataset.src);
        let imgElement = album.querySelector('.imgBoard__img');
        let next = album.querySelector('.imgBoard__nextIcon');
        let prev = album.querySelector('.imgBoard__prevIcon');

        // swipe events
        let touchstartX = 0;

        let slider = {
            container: album,
            imgElement: imgElement,
            next, prev,
            current: 0,
            images: images,
            nextImg: function () {
                this.current++;

                if (this.current >= this.images.length) {
                    this.current = 0;
                }

                this.imgElement.src = this.images[this.current];

            },
            prevImg: function () {
                this.current--;

                if (this.current <= -1) {
                    this.current = this.images.length - 1;
                }

                this.imgElement.src = this.images[this.current];
            },
            handleGesture: function (change) {
                if (change >= 0) {
                    this.prevImg();
                } else if (change <= 0) {
                    this.nextImg();
                }
            }
        };

        slider.next.addEventListener('click', function (event) {
            event.preventDefault();
            slider.nextImg();
        }, false);

        slider.prev.addEventListener('click', function (event) {
            event.preventDefault();
            slider.prevImg();
        }, false);

        slider.container.addEventListener('touchstart', function (event) {
            touchstartX = event.changedTouches[0].screenX;
        }, false);

        slider.container.addEventListener('touchend', function (event) {
            let change = event.changedTouches[0].screenX - touchstartX;

            if (Math.abs(change) < 30) {
                return;
            }

            event.preventDefault();
            slider.handleGesture(change);
        }, false);

        sliders.push(slider);
    }
}());