<template>
    <v-autocomplete
        v-model="localModelValue"
        :item-title="itemTitle"
        :rules="rules"
        readonly
        clearable
        menu-icon=""
        @click="show = true"
        @click:clear="resetSelectedValues()"
    />

    <v-dialog v-model="show" :max-width="maxWidth">
        <v-card :max-width="maxWidth">
            <v-card-title class="dialog-title">
                {{ t(title) }}
            </v-card-title>
            <v-card-text>
                {{ t(subTitle) }}
            </v-card-text>
            <v-card-item class="mx-3">
                <v-data-table-server
                    v-model:items-per-page="itemsPerPage"
                    v-model.value="selectedRowsKeys"
                    :headers="headers"
                    :items="items"
                    :items-length="totalItems"
                    :item-value="itemValue"
                    :items-per-page-options="[
                        { value: 10, title: '10' },
                        { value: 25, title: '25' },
                    ]"
                    :loading="loading"
                    :search="search"
                    :select-strategy="selectType"
                    :show-select="multiple"
                    @update:options="loadPage"
                    @click:row="handleRowClick"
                >
                    <template #top>
                        <v-text-field v-model="search" :label="t('Core.Search')">
                            <template #append-inner>
                                <v-icon icon="mdi-magnify" color="black" />
                            </template>
                        </v-text-field>
                    </template>
                </v-data-table-server>
            </v-card-item>
            <v-card-actions>
                <v-btn-primary v-if="multiple" @click="closeAndMapSelectedRows()">{{ t('Core.Choose') }}</v-btn-primary>
                <v-btn-secondary @click="closeAndSetLastSelectedItems()">{{ t('Core.Close') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts" generic="TItem">
import { ref, type PropType, computed, onMounted, watch } from 'vue';
import { type DataTableHeader } from '../core.models';
import { useI18n } from 'vue-i18n';
import type { PaginationOptions } from '../services/api/api.models';
import type { ValidationRule } from '@/modules/shared/composables/validators';

const emit = defineEmits(['update:modelValue']);

const props = defineProps({
    modelValue: { type: Array<TItem>, default: [] },
    rules: { type: Array<ValidationRule>, default: [] },
    itemValue: { type: String, default: 'id' },
    itemTitle: { type: String, required: true },
    multiple: { type: Boolean, default: false },
    title: { type: String, required: true },
    subTitle: { type: String, required: true },
    headers: {
        type: Object as PropType<DataTableHeader[] | DataTableHeader[][]> | null,
        required: true,
    },
    loadItemsFn: { type: Function, required: true },
    maxWidth: { type: String, default: undefined },
});

const { t } = useI18n();

const selectedRows = ref<TItem[]>();
let selectedRowsInLastDialog = Array<TItem>();
const items = ref<TItem[]>();
const show = ref(false);
const search = ref('');
const selectedRowsKeys = ref();
const itemsPerPage = ref(10);
const loading = ref(false);
const totalItems = ref(0);
const viewedItems = ref();

const localModelValue = computed({
    get() {
        return props.modelValue;
    },
    set(value) {
        emit('update:modelValue', value);
    },
});

const selectType = computed(() => (props.multiple ? 'page' : 'single'));

async function loadPage(data: PaginationOptions): Promise<void> {
    loading.value = true;
    const responseData = await props.loadItemsFn(data);
    items.value = responseData.items;
    totalItems.value = responseData.totalCount;
    loading.value = false;
}

watch(items, newItems => {
    if (newItems != null) addItemsToList(newItems);
});

const handleRowClick = (_event: any, row: { item: any }) => {
    if (!props.multiple) {
        selectedRowsKeys.value = [];
        selectedRowsKeys.value.push(row.item.key);
        setSelectedRows();
        emit('update:modelValue', selectedRows.value);
        show.value = false;
    }
};

const setSelectedRows = () => {
    const selectedRowsObjects: TItem[] = [];
    if (selectedRowsKeys.value == null) return;

    for (const key in selectedRowsKeys.value) {
        selectedRowsObjects.push(
            viewedItems.value.find((item: any) => item[props.itemValue] === selectedRowsKeys.value[key])
        );
    }
    selectedRows.value = selectedRowsObjects;
    selectedRowsInLastDialog = selectedRowsObjects;
};

const addItemsToList = (newItems: TItem[]) => {
    if (viewedItems.value == null) viewedItems.value = [];

    newItems.forEach(item => {
        if (!viewedItems.value.includes(item)) viewedItems.value.push(item);
    });
};

const resetSelectedRowsKeys = () => {
    if (selectedRows.value == null || selectedRowsKeys.value == null) return;
    selectedRowsKeys.value = [];

    selectedRows.value?.forEach((item: any) => {
        if (item != null && props.itemTitle in item) selectedRowsKeys.value.push(item[props.itemValue]);
    });
};

const closeAndMapSelectedRows = () => {
    setSelectedRows();
    emit('update:modelValue', selectedRows.value);
    show.value = false;
};

const closeAndSetLastSelectedItems = () => {
    selectedRows.value = selectedRowsInLastDialog;
    resetSelectedRowsKeys();
    show.value = false;
};

const resetSelectedValues = () => {
    viewedItems.value = [];
    selectedRows.value = [];
    selectedRowsKeys.value = [];
    selectedRowsInLastDialog = [];
};

onMounted(async () => {
    selectedRows.value = { ...props.modelValue };
    await loadPage({ page: 1, itemsPerPage: 10 });
});
</script>

<style lang="scss" scoped>
.dialog-title {
    font-size: 24px;
    font-weight: 600;
}

.v-card.v-card--in-dialog .v-card-actions {
    padding-right: 36px !important;
}

:deep(.v-data-table .v-table__wrapper > table > thead > tr th) {
    background-color: #f3f4f6;
}
</style>
