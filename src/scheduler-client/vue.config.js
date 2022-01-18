// vue.config.js
module.exports = {
    outputDir: "../WestMarchSite/wwwroot/client_app",
    filenameHashing: false,
    devServer: {
        proxy: 'http://localhost:54903'
    }
}