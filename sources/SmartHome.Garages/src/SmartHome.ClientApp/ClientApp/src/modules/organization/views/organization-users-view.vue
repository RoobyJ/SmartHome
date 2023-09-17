<template>
    <v-container fluid>
        <teleport v-if="afterMounted || organizationId == null" to="#page-dashboard-actions">
            <invite-user-dialog :customers="userOrganizationCustomers" :organization-id="organizationId ?? -1" />
        </teleport>

        <v-tabs v-model="tab" color="#0288d1">
            <v-tab value="users">{{ t('Organizations.OrganizationUsers') }}</v-tab>
            <v-tab value="invitations">{{ t('Organizations.SentInvitations') }}</v-tab>
        </v-tabs>

        <v-divider thickness="2" class="pseudo-divider" />

        <v-window v-model="tab" class="mt-4">
            <v-window-item value="users">
                <div class="mb-4">
                    {{ t('Applications.InThisViewYouCanSeeAListOfAllUsersAssignedToYourOrganization') }}
                </div>
                <user-table :headers="headers" :load-items="loadItems" />
            </v-window-item>
            <v-window-item value="invitations" />
        </v-window>
    </v-container>
</template>

<script setup lang="ts">
import { type DataTableHeader } from '@/modules/core/core.models';
import { UsersClient } from '@/modules/core/services/api-clients/users-client';
import type { PaginatedListResultDto, PaginationOptions, UserDto } from '@/modules/core/services/api/api.models';
import { useAccountStore } from '@/modules/core/store/account-store';
import InviteUserDialog from '@/modules/shared/components/invite-user-dialog.vue';
import UserTable from '@/modules/shared/components/user-table.vue';
import { useAfterMounted } from '@/modules/shared/composables/after-mounted';
import { storeToRefs } from 'pinia';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';

const { t } = useI18n();

const tab = ref(null);

const { userOrganizationCustomers, organizationId } = storeToRefs(useAccountStore());

const { afterMounted } = useAfterMounted();

const headers = ref<DataTableHeader[]>([
    { title: t('Core.FirstName'), align: 'start', key: 'firstName', sortable: false },
    { title: t('Core.LastName'), align: 'start', key: 'lastName', sortable: false },
    { title: t('Core.Role'), align: 'start', key: 'role', sortable: false },
    { title: t('Core.Email'), align: 'start', key: 'email', sortable: false },
    { title: '', align: 'center', key: 'deleteUser', sortable: false },
]);

async function loadItems(paginationOptions: PaginationOptions): Promise<PaginatedListResultDto<UserDto>> {
    return await UsersClient.getUsers(paginationOptions, true, true);
}
</script>

<style lang="scss" scoped>
.pseudo-divider {
    margin-top: -2px;
}
</style>
