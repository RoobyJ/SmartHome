import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import type { RouteRecordRaw } from 'vue-router';
import Test2View from './views/test-2-view.vue';

export const test2Routes: RouteRecordRaw[] = [
    {
        path: '/test2',
        component: PageDashboard,
        children: [
            {
                path: '',
                name: View.test2View,
                component: Test2View,
                meta: viewMetaDefinitions[View.test2View],
            },
        ],
    },
];
