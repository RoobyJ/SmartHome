import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import type { RouteRecordRaw } from 'vue-router';
import ProfileView from './views/profile-view.vue';

export const profileRoutes: RouteRecordRaw[] = [
    {
        path: '/',
        component: PageDashboard,
        children: [
            {
                path: '',
                name: View.profileView,
                component: ProfileView,
                meta: viewMetaDefinitions[View.profileView],
            },
        ],
    },
];
