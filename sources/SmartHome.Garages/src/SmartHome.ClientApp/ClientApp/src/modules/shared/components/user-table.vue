<template>
    <v-card rounded="0" class="card-shadow">
        <v-data-table-server
            :headers="headers"
            :items-length="totalItems"
            :items="items"
            :loading="loading"
            class="font-weight-medium"
            @update:options="loadPage"
        >
            <template #item.role="item">{{ t('Core.Roles.' + item.item.columns.role) }}</template>
            <template #item.email="item">
                <v-icon icon="mdi-email-outline" size="small" class="email-item" />
                {{ item.item.columns.email }}
            </template>
            <template #item.deleteUser>
                <v-icon icon="mdi-trash-can-outline" size="small" />
            </template>
        </v-data-table-server>
    </v-card>
</template>

<script setup lang="ts">
import { type DataTableHeader } from '@/modules/core/core.models';
import type { PaginatedListResultDto, PaginationOptions, UserDto } from '@/modules/core/services/api/api.models';
import { useAccountStore } from '@/modules/core/store/account-store';
import { storeToRefs } from 'pinia';
import { ref, type PropType } from 'vue';
import { useI18n } from 'vue-i18n';

const props = defineProps({
    headers: {
        type: Object as PropType<DataTableHeader[] | DataTableHeader[][]> | null,
        required: true,
    },
    loadItems: {
        type: Function as PropType<(options: PaginationOptions) => Promise<PaginatedListResultDto<UserDto>>>,
        required: true,
    },
});
interface TableItems {
    firstName: string;
    lastName: string;
    email: string;
    role: string;
}

const { t } = useI18n();
const { organizationId } = storeToRefs(useAccountStore());

const loading = ref(false);
const totalItems = ref(0);
const items = ref<TableItems[]>([]);

async function loadPage(data: PaginationOptions): Promise<void> {
    loading.value = true;

    if (!props.loadItems) return;

    const result = await props.loadItems(data);
    items.value = [];

    result.items.forEach((item: UserDto) => {
        const organization = item.organizations?.find(x => x.id === organizationId.value);

        items.value.push({
            firstName: item.firstName,
            lastName: item.lastName,
            email: item.email,
            role: organization?.role.rank ?? '',
        });
    });

    totalItems.value = result.totalCount;
    loading.value = false;
}
</script>

<style lang="scss">
.email-item {
    margin-right: 10px;
    color: #98a6b3;
}

.card-shadow {
    box-shadow: 0 1px 12px #0000000a;
}
</style>
