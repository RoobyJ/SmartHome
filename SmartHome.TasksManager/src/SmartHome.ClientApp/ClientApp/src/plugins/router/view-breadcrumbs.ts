import type { RouterLinkProps } from 'vue-router';
import { View } from './view-definitions';

type BreadcrumbLink = Partial<RouterLinkProps> & { title: string; disabled?: boolean; titleArguments?: any[] };
export type ViewBreadcrumbs = (string | BreadcrumbLink)[];
type ViewBreadcrumbsWithOrganizationName = (organizationName: string) => ViewBreadcrumbs;

export const viewBreadcrumbDefinitions: Record<View, null | ViewBreadcrumbs | ViewBreadcrumbsWithOrganizationName> = {
    [View.testView]: [
        {
            title: 'Test',
            disabled: false,
        },
    ],
};
