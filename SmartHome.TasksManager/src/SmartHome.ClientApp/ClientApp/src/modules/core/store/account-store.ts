import { StringUtils } from '@cmpl/core';
import { acceptHMRUpdate, defineStore } from 'pinia';
import { computed, ref } from 'vue';
import type { IdTokenClaims } from '../services/api/api.models';

export const useAccountStore = defineStore('account', () => {
    const user = ref<IdTokenClaims | null>(null);

    const isLoggedIn = computed(() => user.value != null);

    const basicClaims = computed<IdTokenClaims | null>(() => user.value ?? null);
    const name = computed(() => basicClaims.value?.name ?? null);
    const email = computed(() => basicClaims.value?.email ?? null);
    const initials = computed(() => {
        const firstName = basicClaims.value?.givenName ?? null;
        const firstNameLetter = StringUtils.isNotEmpty(firstName) ? firstName[0].toUpperCase() : '';

        const lastName = basicClaims.value?.familyName ?? null;
        const lastNameLetter = StringUtils.isNotEmpty(lastName) ? lastName[0].toUpperCase() : '';

        const initials = `${firstNameLetter}${lastNameLetter}`;
        return StringUtils.isNotEmpty(initials) ? initials : null;
    });

    const setUser = (payload: IdTokenClaims | null) => (user.value = payload);
    return { isLoggedIn, name, email, initials, setUser };
});

if (import.meta.hot) {
    import.meta.hot.accept(acceptHMRUpdate(useAccountStore, import.meta.hot));
}
