import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import { RouteMetaBuilder } from '@/plugins/router/utils/route-meta-builder';
import type { RouteRecordRaw } from 'vue-router';
import KitchenSinkView from './views/kitchen-sink.vue';
import { useDev } from '../shared/composables/is-dev';

export const kitchenSinkRoutes: RouteRecordRaw[] = [];

let initialized = false;

if (!initialized && useDev().isDev) {
    initialized = true;

    kitchenSinkRoutes.push({
        path: '/kitchen-sink',
        component: PageDashboard,
        children: [
            {
                path: '',
                component: KitchenSinkView,
                meta: new RouteMetaBuilder().withTitle('Kitchen sink').withIcon('mdi-faucet').build(),
            },
        ],
    });
}
