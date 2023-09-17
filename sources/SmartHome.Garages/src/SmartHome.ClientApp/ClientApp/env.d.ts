/// <reference types="vite/client" />
import { RouteAuthMeta } from '@/plugins/router/utils/route-auth-meta';
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

// To ensure it is treated as a module, add at least one `export` statement
export {};

// extending RouteMeta object
declare module 'vue-router' {
    interface RouteMeta {
        /** It is displayed in the tab and used in breadcrumbs */
        title: string;
        icon: string | null;
        authGuard: RouteAuthMeta;
        /** Whether the navigation drawer, top-bar and footer are visible. If false, then panel layout is used */
        useDashboardLayout: boolean;
    }
}
