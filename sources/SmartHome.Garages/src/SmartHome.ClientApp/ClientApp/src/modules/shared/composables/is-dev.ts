export function useDev() {
    return { isDev: import.meta.env.VITE_APP_IS_DEV_ENV === 'true' };
}
