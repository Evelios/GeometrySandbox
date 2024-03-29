// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

const path = require("path");

module.exports = {
    mode: "development",
    entry: "./src/App.fs.js",
    output: {
        path: path.resolve(__dirname, "./public"),
        filename: "bundle.js",
    },
    devServer: {
        static: {
            directory: "./public",
            publicPath: "/",
        },
        port: 8080,
    },
    module: {}
}