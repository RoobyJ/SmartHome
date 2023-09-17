import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import type { RouteRecordRaw } from 'vue-router';
import TestView from './views/test-view.vue';

export const testRoutes: RouteRecordRaw[] = [
    {
        path: '/',
        component: PageDashboard,
        children: [
            {
                path: '',
                name: View.testView,
                component: TestView,
                meta: viewMetaDefinitions[View.testView],
            },
        ],
    },
];
