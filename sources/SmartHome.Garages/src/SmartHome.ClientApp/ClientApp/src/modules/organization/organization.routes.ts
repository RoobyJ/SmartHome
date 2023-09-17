import PageDashboard from '@/modules/shared/components/page-dashboard.vue';
import PagePanel from '@/modules/shared/components/page-panel.vue';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import type { RouteRecordRaw } from 'vue-router';
import OrganizationAcceptInvitationView from './views/organization-accept-invitation-view.vue';
import OrganizationAppsView from './views/organization-apps-view.vue';
import OrganizationUsersView from './views/organization-users-view.vue';
import OrganizationView from './views/organization-view.vue';

export const organizationRoutes: RouteRecordRaw[] = [
    {
        path: '/organization',
        component: PageDashboard,
        children: [
            {
                path: '',
                name: View.organizationView,
                component: OrganizationView,
                meta: viewMetaDefinitions[View.organizationView],
            },
            {
                path: 'users',
                name: View.organizationUsersView,
                component: OrganizationUsersView,
                meta: viewMetaDefinitions[View.organizationUsersView],
            },
            {
                path: 'applications',
                name: View.organizationAppsView,
                component: OrganizationAppsView,
                meta: viewMetaDefinitions[View.organizationAppsView],
            },
        ],
    },
    {
        path: '/accept/:token',
        component: PagePanel,
        children: [
            {
                path: '',
                name: View.organizationAcceptInvitationView,
                component: OrganizationAcceptInvitationView,
                meta: viewMetaDefinitions[View.organizationAcceptInvitationView],
            },
        ],
    },
];
