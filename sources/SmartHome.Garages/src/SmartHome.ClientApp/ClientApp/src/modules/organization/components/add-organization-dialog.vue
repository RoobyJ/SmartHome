<template>
    <v-dialog v-model="showDialog" :loading="loading">
        <template #activator="{ props }">
            <v-btn-secondary v-bind="props">
                <!-- // TODO: generate new icon for this button -->
                <v-icon size="20" start>mdi-city</v-icon>
                {{ t('Organizations.AddOrganization') }}
            </v-btn-secondary>
        </template>

        <v-card>
            <v-card-title>{{ t('Organizations.AddingAnOrganization') }}</v-card-title>
            <v-card-text>
                <v-form ref="form">
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Name') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field
                                ref="organizationNameInputRef"
                                v-model="organizationName"
                                :rules="organizationNameValidators"
                                :error-messages="nameApiErrors"
                            />
                        </v-col>
                    </v-row>
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Organizations.Customers') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <c-lookup-input
                                v-model="selectedCustomers"
                                title="Organizations.Customers"
                                sub-title="Organizations.ChooseCustomersFromList"
                                item-title="name"
                                :rules="customersValidators"
                                :load-items-fn="loadCustomers"
                                :headers="customerHeaders"
                            />
                        </v-col>
                    </v-row>
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Roles.Owner') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <c-lookup-input
                                v-model="owner"
                                title="Users.Users"
                                sub-title="Organizations.ChooseUserFromListOnTheOwner"
                                item-title="lastName"
                                :load-items-fn="loadUsers"
                                :headers="userHeaders"
                            />
                        </v-col>
                    </v-row>
                    <v-row v-if="apiErrorAlert != null" dense>
                        <v-col cols="12">
                            <c-api-error-alert v-model="apiErrorAlert" />
                        </v-col>
                    </v-row>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-btn-primary :loading="isSaving" @click="addOrganization">{{ t('Core.Add') }}</v-btn-primary>
                <v-btn-secondary @click="showDialog = false">{{ t('Core.Cancel') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import type { DataTableHeader } from '@/modules/core/core.models';
import { CustomersClient } from '@/modules/core/services/api-clients/customers-client';
import { OrganizationsClient } from '@/modules/core/services/api-clients/organizations-client';
import { UsersClient } from '@/modules/core/services/api-clients/users-client';
import type {
    CreateOrganizationDto,
    CustomerBaseDto,
    PaginatedListResultDto,
    PaginationOptions,
    UserBaseDto,
} from '@/modules/core/services/api/api.models';
import { createObject, type ValueProvider } from '@/modules/core/services/api/api.utils';
import { useFormErrorHandler } from '@/modules/core/services/errors/form-error-handler';
import { useValidators } from '@/modules/shared/composables/validators';
import { ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { VForm, VTextField } from 'vuetify/components';

// composables
const { t } = useI18n();

// flags
const showDialog = ref(false);
const isSaving = ref(false);
const loading = ref(false);

// form
const form = ref<VForm | null>(null);

const { required, length, minArrayLength } = useValidators();

const organizationName = ref('');
const organizationNameValidators = [required, length(4, 256)];
const organizationNameInputRef = ref<VTextField | null>(null);

const customerHeaders: DataTableHeader[] = [
    {
        title: t('Core.Name'),
        key: 'name',
        align: 'start',
    },
];

const selectedCustomers = ref<CustomerBaseDto[]>([]);
const customersValidators = [minArrayLength(1)];

const userHeaders: DataTableHeader[] = [
    {
        title: t('Core.Email'),
        key: 'email',
        align: 'start',
    },
    { title: t('Core.FirstName'), key: 'firstName', align: 'start' },
    { title: t('Core.LastName'), key: 'lastName', align: 'start' },
];

const owner = ref<UserBaseDto[]>();

const dtoValueProvider: ValueProvider<CreateOrganizationDto> = {
    name: () => organizationName,
    customersIds: () => selectedCustomers.value.map(i => i.id),
    ownerId: () => (owner.value != null && owner.value.length > 0 ? owner.value[0].id : null),
};

const { setApiError, resetApiError, apiErrorAlert, nameApiErrors } = useFormErrorHandler({
    valueProvider: dtoValueProvider,
    apiAlertIgnoredProps: ['name'],
});

async function addOrganization() {
    resetApiError();

    if (isSaving.value || form.value == null) return;

    const validationResult = await form.value.validate();
    if (!validationResult.valid) return;

    isSaving.value = true;

    const payload = createObject(dtoValueProvider);
    const response = await OrganizationsClient.createOrganization(payload);

    if (response.isSuccess) {
        emit('created');
        showDialog.value = false;
    } else {
        setApiError(response.error);
        if (nameApiErrors.value.length > 0) {
            organizationNameInputRef.value?.focus();
        }
    }

    isSaving.value = false;
}

async function loadCustomers(page: PaginationOptions): Promise<PaginatedListResultDto<CustomerBaseDto>> {
    const response = await CustomersClient.getCustomers(page);
    return response;
}

async function loadUsers(page: PaginationOptions): Promise<PaginatedListResultDto<UserBaseDto>> {
    const response = await UsersClient.getUsers(page);
    return response;
}

// reset the values when the dialog is opening
watch(showDialog, shouldShow => {
    if (!shouldShow) return;

    resetApiError();
    selectedCustomers.value = [];
    organizationName.value = '';
    owner.value = undefined;
});

const emit = defineEmits(['created']);
</script>
