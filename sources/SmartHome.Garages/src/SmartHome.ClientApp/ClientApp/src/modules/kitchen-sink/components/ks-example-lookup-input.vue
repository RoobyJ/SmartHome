<template>
    <v-container fluid class="pa-0">
        <div cols="12" class="text-h5">EXAMPLE LOOKUP TABLE - {{ exampleTitle }}</div>

        <c-lookup-input
            v-model="selectedUsers"
            :rules="selectedUsersRules"
            :load-items-fn="loadItems"
            :headers="headers"
            :title="title"
            :sub-title="subTitle"
            :multiple="multiple"
            item-value="email"
            item-title="lastName"
        />
    </v-container>
</template>

<script setup lang="ts">
import { UsersClient } from '@/modules/core/services/api-clients/users-client';
import type { PaginatedListResultDto, PaginationOptions, UserDto } from '@/modules/core/services/api/api.models';
import { useValidators } from '@/modules/shared/composables/validators';
import { ref } from 'vue';

defineProps({
    exampleTitle: { type: String, required: true },
    title: { type: String, required: true },
    subTitle: { type: String, required: true },
    multiple: { type: Boolean, default: false },
});

const selectedUsers = ref<UserDto[]>([]);
const { minArrayLength } = useValidators();
const selectedUsersRules = [minArrayLength(1)];

async function loadItems(paginationOptions: PaginationOptions): Promise<PaginatedListResultDto<UserDto>> {
    // here is request to the API
    const response = await UsersClient.getUsers(
        {
            page: paginationOptions.page,
            itemsPerPage: paginationOptions.itemsPerPage,
        },
        false
    );
    return response;
}

const headers = ref([
    {
        title: 'Email',
        align: 'start',
        sortable: true,
        key: 'email',
    },
    {
        title: 'FirstName',
        align: 'start',
        sortable: true,
        key: 'firstName',
    },
    {
        title: 'LastName',
        align: 'start',
        sortable: true,
        key: 'lastName',
    },
]);
</script>
