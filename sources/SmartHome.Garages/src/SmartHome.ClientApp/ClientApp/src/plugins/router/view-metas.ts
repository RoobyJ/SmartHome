import type { RouteMeta } from 'vue-router';
import { RouteMetaBuilder } from './utils/route-meta-builder';
import { View } from './view-definitions';

export const viewMetaDefinitions: Record<View, RouteMeta> = {
    [View.profileView]: new RouteMetaBuilder().withTitle('Profile.Profile').withIcon('mdi-view-dashboard').build(),
    [View.organizationView]: new RouteMetaBuilder()
        .withTitle('Organizations.Organization')
        .withIcon('mdi-office-building-outline')
        .build(),
    [View.organizationUsersView]: new RouteMetaBuilder().withTitle('Users.Users').withIcon('mdi-account-group').build(),
    [View.organizationAppsView]: new RouteMetaBuilder()
        .withTitle('Applications.Applications')
        .withIcon('mdi-application-brackets-outline')
        .build(),
    [View.organizationAcceptInvitationView]: new RouteMetaBuilder()
        .withTitle('Organizations.AcceptInvitation')
        .allowAnonymous()
        .usePanelLayout()
        .build(),
    [View.systemView]: new RouteMetaBuilder()
        .withTitle('System.System')
        .withIcon('mdi-application-cog-outline')
        .build(),
    [View.systemOrgsView]: new RouteMetaBuilder()
        .withTitle('Organizations.Organizations')
        .withIcon('mdi-domain')
        .build(),
    [View.systemUsersView]: new RouteMetaBuilder().withTitle('Users.Users').withIcon('mdi-account-group').build(),
    [View.systemAppsView]: new RouteMetaBuilder()
        .withTitle('Applications.Applications')
        .withIcon('mdi-application-brackets-outline')
        .build(),
};
