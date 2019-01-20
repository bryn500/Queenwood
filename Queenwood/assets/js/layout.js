import polyfills from './utilities/polyfills.js';
import lazyload from 'vanilla-lazyload';
import nav from './modules/nav';
//import hero from './modules/hero';

// init
(function () {
    new lazyload({
        elements_selector: '.lazy',
        threshold: 100
    });
}());