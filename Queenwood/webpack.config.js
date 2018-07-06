/*
 * Webpack config file
 */
const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

const inProduction = process.env.NODE_ENV === 'production';

module.exports = {
    entry: {
        // js
        'js/layout': './assets/js/layout.js',
        'js/loadfonts': './assets/js/loadfonts.js',
        // scss
        'css/all': './assets/scss/all.scss',
        'css/critical': './assets/scss/critical.scss'
    },
    output: {
        path: path.resolve(__dirname, './wwwroot/'),
        filename: '[name].min.js'
    },
    module: {
        rules: [
            /*
             *   standard website static content rules
             */
            {
                // for .scss files in scss folder
                // converts scss files to css
                // resolves imports and urls
                // uses ExtractTextPlugin to create seperate scss file
                // fallback to adding CSS to the DOM by injecting a <style> tag
                test: /\.scss$/,
                include: [
                    path.resolve(__dirname, "./assets/scss/")
                ],
                use: ExtractTextPlugin.extract({
                    use: [
                        { loader: 'css-loader', options: { minimize: true } },
                        'sass-loader'
                    ],
                    fallback: 'style-loader'
                })
            },
       
            /*
             *   js files
             */
            {
                // for all .js files
                test: /\.js$/,
                include: [
                    path.resolve(__dirname, "./assets/js/")
                ],
                // run babel to convert js to es5
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['env']
                    }
                }
            }
        ]
    },
    plugins: [
        // extract scss to build folder taking name from entry list
        new ExtractTextPlugin('[name].css')
    ]
};

// in production changes minify/compress
if (inProduction) {
    // http://vue-loader.vuejs.org/en/workflow/production.html
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        // uglify js
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false
            }
        })
    ])
}

