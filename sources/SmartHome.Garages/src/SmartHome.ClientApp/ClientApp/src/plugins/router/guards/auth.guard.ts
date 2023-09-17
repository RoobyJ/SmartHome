import { AccountClient } from '@/modules/core/services/api-clients/account-client';
import type { NavigationGuardWithThis } from 'vue-router';
import { View } from '../view-definitions';

const guard: NavigationGuardWithThis<undefined> = async (to, from, next) => {
    if (to.meta?.authGuard == null) {
        next();
        return;
    }

    const authCheckResult = to.meta?.authGuard?.checkAccess(to);

    if (authCheckResult.hasAccess) {
        next();
        return;
    }

    if (!authCheckResult.hasAccess && authCheckResult.noAccessReason === 'onlyAnonymous') {
        next({ name: View.profileView });
        return;
    }

    if (!authCheckResult.hasAccess && authCheckResult.noAccessReason === 'missingPermission') {
        next(false);
        return;
    }

    // no user session active, go to login page
    AccountClient.login();
    next();
};

export default guard;
