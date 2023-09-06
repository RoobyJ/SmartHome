import i18n from '@/plugins/i18n';
import { StringUtils } from '@cmpl/core';
import type { NavigationHookAfter } from 'vue-router';

const appName = String(import.meta.env.VITE_APP_NAME);

const hook: NavigationHookAfter = (to, from) => {
    if (window?.document == null) return;

    const routeTitle = to.meta.title;

    document.title = StringUtils.isNotEmpty(routeTitle) ? `${appName} - ${i18n.global.t(routeTitle)}` : appName;
};

export default hook;
