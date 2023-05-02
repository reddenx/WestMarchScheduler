// vue.config.js
module.exports = {
    outputDir: "../WestMarchSite/wwwroot",
    filenameHashing: false,
    devServer: {
        proxy: 'http://localhost:5003'
    }
}
