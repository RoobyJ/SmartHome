import type { RouteLocationNormalized } from 'vue-router';
import { useAccountStore } from '../store/account-store';
import { useRouterStore } from '../store/router-store';
import { AccountClient } from './api-clients/account-client';

/** Initializes the user details or redirect to login page when the page is opened and the route is protected */
export async function loadUserDetails(destinationRoute: RouteLocationNormalized) {
    const { initialized, notifyAppInitialized } = useRouterStore();
    if (initialized) return;

    const userDetails = await AccountClient.getDetails();
    if (userDetails) {
        const { setUser } = useAccountStore();
        setUser(userDetails);
        notifyAppInitialized();
        return;
    }

    if (destinationRoute.meta?.authGuard?.requireUserDetails) {
        AccountClient.login();

        // don't call notifyAppInitialized here, we want to display the loading screen until the redirection happens
        return;
    }

    notifyAppInitialized();
}
