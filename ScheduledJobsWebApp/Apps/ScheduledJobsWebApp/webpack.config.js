const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const entry = {
    user: './Scripts/Internal/User/Start/MainPage.ts',
    accessDenied: './Scripts/Internal/User/AccessDenied/MainPage.ts',
    error: './Scripts/Internal/User/Error/MainPage.ts',
    home: './Scripts/Internal/Home/MainPage.ts',
    jobDefinitions: './Scripts/Internal/JobDefinitions/MainPage.ts',
    failedJobs: './Scripts/Internal/Jobs/FailedJobs/MainPage.ts',
    jobDetail: './Scripts/Internal/Jobs/JobDetail/MainPage.ts',
    eventNotifications: './Scripts/Internal/Events/Notifications/MainPage.ts',
    eventNotificationDetail: './Scripts/Internal/Events/NotificationDetail/MainPage.ts'
};
const exportModule = {
    rules: [
        {
            test: /\.tsx?$/,
            use: 'ts-loader',
            exclude: /node_modules/
        },
        {
            test: /\.s[ac]ss$/i,
            use: [
                {
                    loader: 'file-loader',
                    options: {
                        name: '../../styles/css/[name].css',
                    },
                },
                'sass-loader',
            ]
        },
        {
            test: /\.css$/i,
            use: [
                {
                    loader: 'file-loader',
                    options: {
                        name: (resourcePath, resourceQuery) => {
                            if (/@fortawesome[\\\/]fontawesome-free/.test(resourcePath)) {
                                return '../../styles/css/fontawesome/[name].css';
                            }
                            return '../../styles/css/[name].css';
                        }
                    }
                }
            ]
        },
        {
            test: /\.html$/i,
            use: [{
                loader: 'html-loader',
                options: {
                    minimize: {
                        removeComments: false
                    },
                    esModule: false
                }
            }]
        },
        {
            test: /\.(svg|eot|woff|woff2|ttf)$/,
            use: [{
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]',
                    outputPath: '../../styles/css/webfonts'
                }
            }]
        }
    ]
};
const outputFilename = '[name].js';

const resolve = {
    alias: {
    }
};
const plugins = [
    new MiniCssExtractPlugin({
        filename: '[name].css',
        chunkFilename: '[id].css',
    })
];
module.exports = [
    {
        mode: 'production',
        context: __dirname,
        devtool: false,
        entry: entry,
        module: exportModule,
        plugins: plugins,
        output: {
            filename: outputFilename,
            path: path.resolve(__dirname, 'wwwroot', 'js', 'dist')
        },
        resolve: resolve
    },
    {
        mode: 'development',
        context: __dirname,
        devtool: 'eval-source-map',
        entry: entry,
        module: exportModule,
        plugins: plugins,
        output: {
            filename: outputFilename,
            path: path.resolve(__dirname, 'wwwroot', 'js', 'dev')
        },
        resolve: resolve
    }
];