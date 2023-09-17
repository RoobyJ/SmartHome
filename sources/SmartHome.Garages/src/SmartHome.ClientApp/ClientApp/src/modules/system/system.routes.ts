import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import type { RouteRecordRaw } from 'vue-router';
import SystemAppsView from './views/system-apps-view.vue';
import SystemOrgsView from './views/system-orgs-view.vue';
import SystemUsersView from './views/system-users-view.vue';
import SystemView from './views/system-view.vue';

export const systemRoutes: RouteRecordRaw[] = [
    {
        path: '/system',
        component: PageDashboard,
        children: [
            {
                path: '',
                name: View.systemView,
                component: SystemView,
                meta: viewMetaDefinitions[View.systemView],
            },
            {
                path: 'organizations',
                name: View.systemOrgsView,
                component: SystemOrgsView,
                meta: viewMetaDefinitions[View.systemOrgsView],
            },
            {
                path: 'users',
                name: View.systemUsersView,
                component: SystemUsersView,
                meta: viewMetaDefinitions[View.systemUsersView],
            },
            {
                path: 'applications',
                name: View.systemAppsView,
                component: SystemAppsView,
                meta: viewMetaDefinitions[View.systemAppsView],
            },
        ],
    },
];
