import {dirname, resolve} from 'node:path';
import {fileURLToPath, URL} from 'node:url';

import VueI18nPlugin from '@intlify/unplugin-vue-i18n/vite';
import vue from '@vitejs/plugin-vue';
import {visualizer} from 'rollup-plugin-visualizer';
import {defineConfig, type UserConfig} from 'vite';
import {checker} from 'vite-plugin-checker';
import mkcert from 'vite-plugin-mkcert';
import vuetify, {transformAssetUrls} from 'vite-plugin-vuetify';

/**
 * Vite Configure
 *
 * @see {@link https://vitejs.dev/config/}
 */
export default defineConfig(({command, mode}): UserConfig => {
    const config: UserConfig = {
        // https://vitejs.dev/config/shared-options.html#base
        base: '/',
        // https://vitejs.dev/config/shared-options.html#define
        define: {'process.env': {}},
        plugins: [
            // Vue3
            vue({
                template: {
                    // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vite-plugin#image-loading
                    transformAssetUrls,
                },
                script: {
                    defineModel: true,
                },
            }),
            VueI18nPlugin({
                include: resolve(dirname(fileURLToPath(import.meta.url)), 'src/plugins/i18n/locales/**'),
            }),
            // Vuetify Loader
            // https://github.com/vuetifyjs/vuetify-loader/tree/master/packages/vite-plugin#vite-plugin-vuetify
            vuetify({
                autoImport: true,
                styles: {configFile: 'src/styles/settings.scss'},
            }),
            // vite-plugin-checker
            // https://github.com/fi3ework/vite-plugin-checker
            checker({
                typescript: true,
                vueTsc: true,
                eslint: {
                    // for example, lint .ts and .tsx
                    lintCommand: 'eslint "./src/**/*.{ts,tsx,vue}"',
                },
            }),
            // generate certificates for local dev server
            // to configure on mobile check this link: https://github.com/liuweiGL/vite-plugin-mkcert#mobile-devices
            mkcert(),
        ],
        // https://vitejs.dev/config/server-options.html
        server: {
            https: true,
            open: false,
            strictPort: true,
            port: 3020,
            proxy: {
                '/api': {
                    target: 'https://localhost:3021',
                    changeOrigin: true,
                    secure: false,
                    rewrite: path => path.replace(/^\/api/, '/api'),
                }
            },
        },
        // Resolver
        resolve: {
            // https://vitejs.dev/config/shared-options.html#resolve-alias
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url)),
                '~': fileURLToPath(new URL('./node_modules', import.meta.url)),
            },
            extensions: ['.js', '.json', '.jsx', '.mjs', '.ts', '.tsx', '.vue'],
            dedupe: ['vue'],
        },
        // Build Options
        // https://vitejs.dev/config/build-options.html
        build: {
            // Build Target
            // https://vitejs.dev/config/build-options.html#build-target
            target: 'esnext',
            // Minify option
            // https://vitejs.dev/config/build-options.html#build-minify
            minify: 'esbuild',
            // Rollup Options
            // https://vitejs.dev/config/build-options.html#build-rollupoptions
            rollupOptions: {
                output: {
                    manualChunks: {
                        // Split external library from transpiled code.
                        vue: ['vue', 'vue-router', 'pinia', 'pinia-plugin-persistedstate'],
                        vuetify: [
                            'vuetify',
                            'vuetify/components',
                            'vuetify/directives',
                            'vuetify/labs/components',
                            'vuetify/labs/date',
                        ],
                        materialdesignicons: ['@mdi/font/css/materialdesignicons.css'],
                    },
                    plugins: [
                        mode === 'analyze'
                            ? // rollup-plugin-visualizer
                              // https://github.com/btd/rollup-plugin-visualizer
                            visualizer({
                                open: true,
                                filename: 'dist/stats.html',
                            })
                            : undefined,
                    ],
                },
            },
        },
        esbuild: {
            // Drop console when production build.
            drop: command === 'serve' ? [] : ['console'],
        },
    };

    return config;
});
