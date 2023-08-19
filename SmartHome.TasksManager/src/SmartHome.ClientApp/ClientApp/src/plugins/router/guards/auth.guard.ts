import { accountClient } from '@/modules/core/services/api/account-client';
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
        next({ name: View.testView });
        return;
    }

    if (!authCheckResult.hasAccess && authCheckResult.noAccessReason === 'missingPermission') {
        next(false);
        return;
    }

    // no user session active, go to login page
    await accountClient.login();
    next();
};

export default guard;
