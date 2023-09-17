<template>
    <v-dialog v-model="showDialog">
        <template #activator="{ props }">
            <v-btn-secondary v-bind="props">
                <v-icon size="20" start>mdi-account-plus</v-icon>
                {{ t('Core.SendInvitation') }}
            </v-btn-secondary>
        </template>

        <v-card>
            <v-card-title>{{ t('Core.SendInvitation') }}</v-card-title>
            <v-card-text>
                <v-form ref="form">
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Email') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field
                                ref="emailInputRef"
                                v-model="emailValue"
                                :rules="emailValidators"
                                :error-messages="emailApiErrors"
                            />
                        </v-col>
                    </v-row>
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Role') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <v-select
                                v-model="roleValue"
                                :items="availableOrganizationRolesToAssign"
                                :rules="roleValidators"
                            >
                                <template #selection="{ item }">
                                    {{ t('Core.Roles.' + item.value) }}
                                </template>
                                <template #item="{ item, props }">
                                    <v-list-item v-bind="props" :title="t('Core.Roles.' + item.value)" />
                                </template>
                            </v-select>
                        </v-col>
                    </v-row>
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Customers') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <v-select
                                :model-value="selectedCustomersReadonly ? customers : selectedCustomerIds"
                                :items="customers"
                                item-value="id"
                                item-title="name"
                                :readonly="selectedCustomersReadonly"
                                :hint="selectedCustomersHint != null ? t(selectedCustomersHint) : undefined"
                                :clearable="!selectedCustomersReadonly"
                                multiple
                                chips
                                @update:model-value="selectedCustomerIds = $event"
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
                <v-btn-primary :loading="isSaving" @click="sendInvitation">{{ t('Core.Send') }}</v-btn-primary>
                <v-btn-secondary @click="showDialog = false">{{ t('Core.Cancel') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { UsersClient } from '@/modules/core/services/api-clients/users-client';
import { RoleRank, type CustomerBaseDto, type InviteUserDto } from '@/modules/core/services/api/api.models';
import { createObject, type ValueProvider } from '@/modules/core/services/api/api.utils';
import { useFormErrorHandler } from '@/modules/core/services/errors/form-error-handler';
import { useAccountStore } from '@/modules/core/store/account-store';
import { storeToRefs } from 'pinia';
import type { PropType } from 'vue';
import { computed, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { VForm, VTextField } from 'vuetify/components';
import { useValidators } from '../composables/validators';

const props = defineProps({
    customers: {
        type: Array as PropType<CustomerBaseDto[]>,
        required: true,
    },
    organizationId: {
        type: Number,
        required: true,
    },
});

const { t } = useI18n();
const { availableOrganizationRolesToAssign } = storeToRefs(useAccountStore());

// flags
const showDialog = ref(false);
const isSaving = ref(false);

// form
const form = ref<VForm | null>(null);
const { required, email } = useValidators();

const emailValue = ref('');
const emailValidators = [required, email];
const emailInputRef = ref<VTextField | null>(null);

const roleValue = ref<RoleRank | null>(null);
const roleValidators = [required];

const selectedCustomerIds = ref<number[]>([]);
const selectedCustomersReadonly = computed(() => roleValue.value === RoleRank.owner);
const selectedCustomersHint = computed(() =>
    selectedCustomersReadonly.value ? 'Core.OwnersHaveAccessToAllCustomers' : null
);

const dtoValueProvider: ValueProvider<InviteUserDto> = {
    customerIds: () => (selectedCustomersReadonly.value ? [] : selectedCustomerIds.value),
    email: () => emailValue,
    organizationId: () => props.organizationId,
    role: () => roleValue.value,
};

const { setApiError, resetApiError, apiErrorAlert, emailApiErrors } = useFormErrorHandler({
    valueProvider: dtoValueProvider,
    apiAlertIgnoredProps: ['email'],
});

async function sendInvitation() {
    resetApiError();

    if (isSaving.value || form.value == null) return;

    const validationResult = await form.value.validate();
    if (!validationResult.valid) return;

    isSaving.value = true;
    const payload = createObject(dtoValueProvider);
    const response = await UsersClient.inviteUser(payload);

    if (response.isSuccess) {
        emit('invited');
        showDialog.value = false;
    } else {
        setApiError(response.error);
        if (emailApiErrors.value.length > 0) {
            emailInputRef.value?.focus();
        }
    }

    isSaving.value = false;
}

// reset the values when the dialog is opening
watch(showDialog, shouldShow => {
    if (!shouldShow) return;

    resetApiError();
    emailValue.value = '';
    roleValue.value = null;
    selectedCustomerIds.value = [];
});

const emit = defineEmits(['invited']);
</script>
