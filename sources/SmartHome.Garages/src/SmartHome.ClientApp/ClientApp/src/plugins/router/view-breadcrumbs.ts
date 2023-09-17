import type { RouterLinkProps } from 'vue-router';
import { View } from './view-definitions';

type BreadcrumbLink = Partial<RouterLinkProps> & { title: string; disabled?: boolean; titleArguments?: any[] };
export type ViewBreadcrumbs = (string | BreadcrumbLink)[];
type ViewBreadcrumbsWithOrganizationName = (organizationName: string) => ViewBreadcrumbs;

export const viewBreadcrumbDefinitions: Record<View, null | ViewBreadcrumbs | ViewBreadcrumbsWithOrganizationName> = {
    [View.profileView]: [
        {
            title: 'Profile.Profile',
            disabled: false,
        },
    ],
    [View.organizationView]: organizationName => [
        {
            title: 'Organizations.OrganizationX',
            titleArguments: [organizationName],
            disabled: false,
        },
    ],
    [View.organizationUsersView]: organizationName => [
        {
            title: 'Organizations.OrganizationX',
            titleArguments: [organizationName],
            to: { name: View.organizationView },
            disabled: false,
        },
        {
            title: 'Users.Users',
            disabled: false,
        },
    ],
    [View.organizationAppsView]: organizationName => [
        {
            title: 'Organizations.OrganizationX',
            titleArguments: [organizationName],
            to: { name: View.organizationView },
            disabled: false,
        },
        {
            title: 'Applications.Applications',
            disabled: false,
        },
    ],
    [View.systemView]: [
        {
            title: 'System.System',
            disabled: false,
        },
    ],
    [View.systemOrgsView]: [
        {
            title: 'System.System',
            to: { name: View.systemView },
            disabled: false,
        },
        {
            title: 'Organizations.Organizations',
            disabled: false,
        },
    ],
    [View.systemUsersView]: [
        {
            title: 'System.System',
            to: { name: View.systemView },
            disabled: false,
        },
        {
            title: 'Users.Users',
            disabled: false,
        },
    ],
    [View.systemAppsView]: [
        {
            title: 'System.System',
            to: { name: View.systemView },
            disabled: false,
        },
        {
            title: 'Applications.Applications',
            disabled: false,
        },
    ],
    [View.organizationAcceptInvitationView]: [
        {
            title: 'Organizations.AcceptInvitation',
            disabled: false,
        },
    ],
};
