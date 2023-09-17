import { fileURLToPath } from 'node:url';

import vue from '@vitejs/plugin-vue';
import { configDefaults, defineConfig, mergeConfig } from 'vitest/config';

import viteConfig from './vite.config';

const baseConfig = viteConfig({
    command: 'serve',
    mode: '',
});

if (baseConfig.plugins != null) {
    // removing mkcert plugin when running unit tests as build server may not allow to add local CA certificate
    const idx = baseConfig.plugins.findIndex(i => (i as any).name === 'vite:plugin:mkcert');
    if (idx !== -1) baseConfig.plugins?.splice(idx, 1);
}

/**
 * Vitest Configure
 *
 * @see {@link https://vitest.dev/config/}
 */
export default mergeConfig(
    baseConfig,
    defineConfig({
        plugins: [vue()],
        // Resolver
        resolve: {
            // https://vitest.dev/config/#alias
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url)),
                '~': fileURLToPath(new URL('./node_modules', import.meta.url)),
            },
            extensions: ['.js', '.json', '.jsx', '.mjs', '.ts', '.tsx', '.vue'],
        },
        test: {
            environment: 'jsdom',
            exclude: [...configDefaults.exclude, 'e2e/*'],
            setupFiles: [fileURLToPath(new URL('./vitest/setup-fetch-mocks.ts', import.meta.url))],
            root: fileURLToPath(new URL('./', import.meta.url)),
        },
    })
);
