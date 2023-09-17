import type { OrganizationPermission, PermissionLevel, SystemPermission } from '@/modules/core/services/api/api.models';
import { useAccountStore } from '@/modules/core/store/account-store';
import type { RouteLocationNormalized } from 'vue-router';

type NoAccessReason = 'onlyAnonymous' | 'missingPermission';

export interface AuthCheckResult {
    hasAccess: boolean;
    noAccessReason?: NoAccessReason;
}

export interface PermissionRequirement<T> {
    permission: T;
    minLevel: PermissionLevel;
}

export class RouteAuthMeta {
    public get requireUserDetails(): boolean {
        return !this.allowAnonymous;
    }

    constructor(
        private readonly allowAnonymous: boolean,
        private readonly onlyAnonymous: boolean,
        private readonly requiredOrganizationPermissions: PermissionRequirement<OrganizationPermission>[] | null,
        private readonly requiredSystemPermissions: PermissionRequirement<SystemPermission>[] | null
    ) {}

    public checkAccess(route: RouteLocationNormalized): AuthCheckResult {
        const { isLoggedIn: isAuthenticated } = useAccountStore();
        const isAuthorized = this.isAuthorized(route);

        if (this.allowAnonymous && !this.onlyAnonymous) {
            // has access
            return this.createResult(true);
        } else if (this.onlyAnonymous && isAuthenticated) {
            // no access
            return this.createResult('onlyAnonymous');
        } else if (this.onlyAnonymous && !isAuthenticated) {
            // has access
            return this.createResult(true);
        } else if (isAuthenticated && !isAuthorized) {
            // no access
            return this.createResult('missingPermission');
        } else if (isAuthenticated && isAuthorized) {
            // has access
            return this.createResult(true);
        }

        return this.createResult(false);
    }

    private createResult(value: boolean | NoAccessReason): AuthCheckResult {
        if (typeof value === 'boolean') return { hasAccess: value };

        return {
            hasAccess: false,
            noAccessReason: value,
        };
    }

    private isAuthorized(route: RouteLocationNormalized): boolean {
        // TODO: verify whether user have access using the permissions
        return true;
    }
}
