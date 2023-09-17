import { StringUtils } from '@cmpl/core';
import { acceptHMRUpdate, defineStore } from 'pinia';
import { computed, ref } from 'vue';
import {
    RoleRank,
    type CurrentUserDto,
    type EditUserDetailsDto,
    type IdTokenClaims,
    type UserClaimsDto,
    type UserOrganizationDto,
} from '../services/api/api.models';

export const useAccountStore = defineStore('account-store', () => {
    /** Id of the organization, the user currently works with. */
    const organizationId = ref<number | null>(null);
    const user = ref<CurrentUserDto | null>(null);
    const claims = ref<IdTokenClaims | null>(null);

    const isLoggedIn = computed(() => user.value != null && claims.value != null);
    const firstName = computed(() => user.value?.firstName ?? null);
    const lastName = computed(() => user.value?.lastName ?? null);
    const name = computed(() => `${firstName.value ?? ''} ${lastName.value ?? ''}`.trim());
    const email = computed(() => claims.value?.email ?? null);
    const initials = computed(() => {
        const firstNameLetter = StringUtils.isNotEmpty(firstName.value) ? firstName.value[0].toUpperCase() : '';
        const lastNameLetter = StringUtils.isNotEmpty(lastName.value) ? lastName.value[0].toUpperCase() : '';

        const initials = `${firstNameLetter}${lastNameLetter}`;
        return StringUtils.isNotEmpty(initials) ? initials : null;
    });

    const organizationContext = computed<UserOrganizationDto | null>(
        () => user.value?.organizations.find(i => i.id === organizationId.value) ?? null
    );

    const userOrganizationCustomers = computed(() => organizationContext.value?.customers ?? []);
    const organizationName = computed<string>(() => organizationContext.value?.name ?? '');

    const availableOrganizationRolesToAssign = computed(() => {
        if (user.value == null) return [];

        if (user.value.systemRole != null) {
            return [RoleRank.owner, RoleRank.maintainer, RoleRank.user];
        }

        if (organizationContext.value == null) return [];

        switch (organizationContext.value.role.rank) {
            case RoleRank.owner:
                return [RoleRank.owner, RoleRank.maintainer, RoleRank.user];
            case RoleRank.maintainer:
                return [RoleRank.maintainer, RoleRank.user];
            case RoleRank.user:
                return [RoleRank.user];
        }

        return [];
    });

    const setUser = (payload: UserClaimsDto | null) => {
        if (payload == null) {
            user.value = null;
            claims.value = null;
            organizationId.value = null;
            return;
        }

        user.value = payload.userDetails;
        claims.value = payload.claims;

        const previousOrganizationId = organizationId.value;
        organizationId.value = null;

        const orgs = payload.userDetails.organizations ?? [];
        if (orgs.length === 0) return;

        if (orgs.findIndex(i => i.id === previousOrganizationId) !== -1) {
            // restore the previous context
            organizationId.value = previousOrganizationId;
            return;
        }

        // if only one organization, then select it
        if (orgs.length === 1) organizationId.value = orgs[0].id;

        // if many orgs available, then leave context empty, user must choose it explicitly
    };

    /** Sets the organization context the user is working with */
    const setOrganizationId = (id: number | null) => {
        if (id == null) {
            organizationId.value = null;
            return;
        }

        if (user.value == null) return;

        const orgs = user.value.organizations ?? [];
        if (orgs.findIndex(i => i.id === id) !== -1) organizationId.value = id;
    };

    const setUserDetails = (details: EditUserDetailsDto) => {
        if (user.value == null) return;

        user.value.firstName = details.firstName;
        user.value.lastName = details.lastName;
    };

    return {
        isLoggedIn,
        name,
        email,
        firstName,
        lastName,
        initials,
        organizationId,
        organizationName,
        userOrganizationCustomers,
        availableOrganizationRolesToAssign,
        setUser,
        setOrganizationId,
        setUserDetails,
    };
});

if (import.meta.hot) {
    import.meta.hot.accept(acceptHMRUpdate(useAccountStore, import.meta.hot));
}
