<template>
    <v-container fluid>
        <teleport v-if="afterMounted" to="#page-dashboard-actions">
            <add-application-dialog @created="setAddApplicationResult" />
        </teleport>

        <add-application-result-dialog
            v-if="addApplicationResult != null"
            :app="addApplicationResult"
            @close="_ => (addApplicationResult = null)"
        />

        {{ t('Applications.InThisViewYouCanSeeAListOfCreatedApplicationsInYourOrganization') }}
        <v-card rounded="0" class="card-shadow mt-4">
            <v-data-table-server
                v-model:expanded="expanded"
                :headers="headers"
                :items="items"
                :items-length="totalItems"
                item-value="name"
                :loading="loading"
                show-expand
                class="font-weight-medium"
                @update:options="loadItems"
            >
                <template #item.expirationDate="item">{{ formatDateTime(item.item.columns.expirationDate) }}</template>
                <template #item.appDelShow>
                    <v-icon icon="mdi-trash-can-outline" size="small" class="delete-icon" />
                </template>
                <template #expanded-row="{ columns, item }">
                    <tr>
                        <td :colspan="columns.length" class="pl-8 pt-6 pb-4">
                            <div class="scopes-title mb-1">{{ t('Applications.Scopes') }}</div>
                            <div class="scopes-filling">
                                <div class="mb-4">{{ t('Applications.ThisApplicationHasAccessTo') }}</div>
                                <div v-for="(scope, index) in item.raw.scopes" :key="index" class="mb-2">
                                    <v-icon icon="mdi-check-bold" size="12" class="mr-3" />
                                    {{ scope }}
                                </div>
                            </div>
                        </td>
                    </tr>
                </template>
            </v-data-table-server>
        </v-card>
    </v-container>
</template>

<script setup lang="ts">
import type { CreatedApplication, DataTableHeader } from '@/modules/core/core.models';
import type { ApplicationDto, PaginationOptions } from '@/modules/core/services/api/api.models';
import AddApplicationDialog from '@/modules/organization/components/add-application-dialog.vue';
import AddApplicationResultDialog from '@/modules/organization/components/add-application-result-dialog.vue';
import { useAfterMounted } from '@/modules/shared/composables/after-mounted';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { ApplicationsClient } from '@/modules/core/services/api-clients/applications-client';
import { format } from 'date-fns';

const { afterMounted } = useAfterMounted();
const { t } = useI18n();

const headers = ref<DataTableHeader[] | DataTableHeader[][]>([
    { title: t('Core.Name'), key: 'name', sortable: false },
    { title: t('Applications.Identifier'), key: 'clientId', sortable: false },
    { title: t('Applications.ExpirationDate'), key: 'expirationDate', sortable: false },
    { title: '', key: 'appDelShow', align: 'end', sortable: false },
    { title: '', key: 'data-table-expand' },
]);

const expanded = ref([]);
const loading = ref(false);
const totalItems = ref(0);
const items = ref<ApplicationDto[]>([]);
const addApplicationResult = ref<CreatedApplication | null>(null);
const lastPaginationOptions = ref<PaginationOptions | null>(null);

const formatDateTime = (x: Date | null) => {
    if (!x) return '-';

    const date = new Date(x);
    return format(date, t('Core.Localization.DateWithHoursAndMinutes'));
};

async function setAddApplicationResult(result: CreatedApplication) {
    addApplicationResult.value = result;
    await loadItems(lastPaginationOptions.value);
}

async function loadItems(paginationOptions: PaginationOptions | null): Promise<void> {
    loading.value = true;

    try {
        const response = await ApplicationsClient.getApplications(paginationOptions ?? null);

        items.value = response.items;
        lastPaginationOptions.value = paginationOptions;
        totalItems.value = response.totalCount;
    } finally {
        loading.value = false;
    }
}
</script>

<style lang="scss" scoped>
.delete-icon {
    margin-right: -12px;
}

.card-shadow {
    box-shadow: 0 1px 12px #0000000a;
}

.scopes-title {
    font-weight: 600;
}

.scopes-filling {
    font-size: 12px;
    color: #002343;
}
</style>
