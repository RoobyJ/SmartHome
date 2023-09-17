import { View } from '@/plugins/router/view-definitions';

export interface MainMenuGroup {
    mainView: View;
    children: View[];
}

export const mainMenuItems: MainMenuGroup[] = [
    {
        mainView: View.profileView,
        children: [],
    },
    {
        mainView: View.organizationView,
        children: [View.organizationUsersView, View.organizationAppsView],
    },
    {
        mainView: View.systemView,
        children: [View.systemOrgsView, View.systemUsersView, View.systemAppsView],
    },
];
