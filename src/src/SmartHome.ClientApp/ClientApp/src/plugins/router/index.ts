import { loadUserDetails } from '@/modules/core/services/bootstrapper';
import { kitchenSinkRoutes } from '@/modules/kitchen-sink/kitchen-sink.routes';
import { organizationRoutes } from '@/modules/organization/organization.routes';
import { profileRoutes } from '@/modules/profile/profile.routes';
import { systemRoutes } from '@/modules/system/system.routes';
import { createRouter, createWebHistory, type Router } from 'vue-router';
import authGuard from './guards/auth.guard';
import updateTitleHook from './hooks/update-title.hook';
import { View } from './view-definitions';

const router: Router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        ...profileRoutes,
        ...organizationRoutes,
        ...systemRoutes,
        ...kitchenSinkRoutes,
        {
            path: '/(.*)',
            redirect: {
                name: View.profileView,
                replace: true,
            },
        },
    ],
});

router.beforeEach(async (to, from) => await loadUserDetails(to));
router.beforeEach(authGuard);

router.afterEach(updateTitleHook);

export default router;
