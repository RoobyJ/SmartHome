import type { RouteMeta } from 'vue-router';
import { RouteMetaBuilder } from './utils/route-meta-builder';
import { View } from './view-definitions';

export const viewMetaDefinitions: Record<View, RouteMeta> = {
    [View.testView]: new RouteMetaBuilder().allowAnonymous().withTitle('Test').withIcon('mdi-view-dashboard').build(),
    [View.test2View]: new RouteMetaBuilder().withTitle('Test-SmartHome-Desktop').withIcon('mdi-view-dashboard').build(),
};
