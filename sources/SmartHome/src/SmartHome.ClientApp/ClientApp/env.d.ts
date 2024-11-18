/// <reference types="vite/client" />
import 'vue-router';

interface ImportMetaEnv {
    // see https://vitejs.dev/guide/env-and-mode.html#env-files
    readonly VITE_APP_NAME: string;
    readonly VITE_APP_VERSION: string;
    readonly VITE_APP_IS_DEV_ENV: string;
}

// eslint-disable-next-line no-unused-vars
interface ImportMeta {
    readonly env: ImportMetaEnv;
}