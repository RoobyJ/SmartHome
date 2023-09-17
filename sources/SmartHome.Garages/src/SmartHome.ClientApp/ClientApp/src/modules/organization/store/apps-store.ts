import { OrganizationsClient } from '@/modules/core/services/api-clients/organizations-client';
import type { ScopeBaseDto } from '@/modules/core/services/api/api.models';
import { useAccountStore } from '@/modules/core/store/account-store';
import { acceptHMRUpdate, defineStore } from 'pinia';
import { ref } from 'vue';

/** It holds information connected only with the current organization context. If the context changes, then the store is reset. */
export const useAppsStore = defineStore('apps-store', () => {
    let currentStateForOrgId: number | null = null;
    const accountStore = useAccountStore();

    // this will reset the state, when the organizationId changes
    accountStore.$subscribe((_mutation, state) => {
        if (state.organizationId === currentStateForOrgId) return;

        currentStateForOrgId = state.organizationId;
        scopes.value = [];
    });

    const scopes = ref<ScopeBaseDto[]>([]);

    const loadScopes = async (forceLoad = false) => {
        if (!forceLoad && scopes.value.length !== 0) return;

        const itemsResult = await OrganizationsClient.getScopes();
        scopes.value = itemsResult.items;
    };

    return { scopes, loadScopes };
});

if (import.meta.hot) {
    import.meta.hot.accept(acceptHMRUpdate(useAppsStore, import.meta.hot));
}
