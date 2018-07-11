import FontFaceObserver from 'fontfaceobserver';

(function () {
    var font = new FontFaceObserver('Tajawal', {
        weight: 400
    });
    var fontBold = new FontFaceObserver('Tajawal', {
        weight: 700
    });

    Promise.all([font.load(), fontBold.load()]).then(function () {
        document.querySelector('html').classList.add('fontloaded');
        app.setCookie('sv', 1, 7);
    }).catch(function () {
        console.error('Fonts failed to load');
    });
}());
