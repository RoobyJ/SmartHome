import { View } from '@/router/view-definitions';

export interface MainMenuGroup {
    mainView: View;
    title: string
    children: View[];
}

export const mainMenuItems: MainMenuGroup[] = [
    {
        mainView: View.homeView,
        title: 'home',
        children: [],
    },
    {
        mainView: View.garageListView,
        title: 'garages',
        children: [View.garageDetailsView],
    },
];
