// Plugins
import vue from '@vitejs/plugin-vue'
import vuetify, {transformAssetUrls} from 'vite-plugin-vuetify'
import ViteFonts from 'unplugin-fonts/vite'
import mkcert from 'vite-plugin-mkcert'

// Utilities
import {defineConfig} from 'vite'
import {fileURLToPath, URL} from 'node:url'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        vue({
            template: {transformAssetUrls}
        }),
        // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin
        vuetify({
            autoImport: true,
        }),
        ViteFonts({
            google: {
                families: [{
                    name: 'Roboto',
                    styles: 'wght@100;300;400;500;700;900',
                }],
            },
        }),
        mkcert(),
    ],
    define: {'process.env': {}},
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        },
        extensions: [
            '.js',
            '.json',
            '.jsx',
            '.mjs',
            '.ts',
            '.tsx',
            '.vue',
        ],
    },
    server: {
        port: 3000,
        open: false,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'http://localhost:3001', // 5001 https production
                changeOrigin: true,
                secure: false,
                rewrite: path => path.replace(/^\/api/, '/api'),
            },
        },
    },
})