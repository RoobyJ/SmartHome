import { loadUserDetails } from '@/modules/core/services/bootstrapper';
import { testRoutes } from '@/modules/test-module/test.routes';
import { createRouter, createWebHistory, type Router } from 'vue-router';
import authGuard from './guards/auth.guard';
import updateTitleHook from './hooks/update-title.hook';
import { View } from './view-definitions';
import { test2Routes } from '@/modules/data/test2.routes';

const router: Router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        ...testRoutes,
        ...test2Routes,
        {
            path: '/test2',
            redirect: {
                name: View.test2View,
                replace: true,
            },
        },
        {
            path: '/',
            redirect: {
                name: View.testView,
                replace: true,
            },
        },
    ],
});

router.beforeEach(async (to, from) => await loadUserDetails(to));
router.beforeEach(authGuard);

router.afterEach(updateTitleHook);

export default router;
