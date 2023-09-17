<template>
    <v-dialog v-model="showDialog">
        <template #activator="{ props }">
            <v-btn-secondary v-bind="props" :loading="isLoading" :disabled="!isLoading && scopeViewModels.length === 0">
                <v-icon size="20" start>dm:add-app</v-icon>
                {{ t('Applications.AddApplication') }}
            </v-btn-secondary>
        </template>

        <v-card>
            <v-card-title>{{ t('Applications.AddingAnApplication') }}</v-card-title>
            <v-card-text>
                <v-progress-circular v-if="isLoading" />
                <v-form v-else ref="form">
                    <v-row dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Core.Name') }}</v-form-label>
                        </v-col>
                        <v-col cols="12">
                            <v-text-field
                                ref="appNameInputRef"
                                v-model="appName"
                                :rules="appNameValidators"
                                :error-messages="nameApiErrors"
                            />
                        </v-col>
                    </v-row>
                    <v-row v-if="scopeViewModels.length > 0" dense>
                        <v-col cols="12">
                            <v-form-label>{{ t('Applications.Scopes') }}</v-form-label>
                        </v-col>
                        <v-col cols="12" class="checkbox-list">
                            <v-checkbox
                                v-for="(loopItem, index) in scopeViewModels"
                                :key="loopItem.value"
                                :model-value="loopItem.checked"
                                :label="loopItem.value"
                                :hide-details="index !== scopeViewModels.length - 1"
                                :rules="index === scopeViewModels.length - 1 ? selectedScopesValidators : []"
                                :validation-value="selectedScopes"
                                @update:model-value="toggleScope(loopItem.value)"
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
                <v-btn-primary :loading="isSaving" @click="addApplication">{{ t('Core.Add') }}</v-btn-primary>
                <v-btn-secondary @click="showDialog = false">{{ t('Core.Cancel') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import type { CreatedApplication } from '@/modules/core/core.models';
import { ApplicationsClient } from '@/modules/core/services/api-clients/applications-client';
import type { CreateApplicationDto } from '@/modules/core/services/api/api.models';
import { createObject, type ValueProvider } from '@/modules/core/services/api/api.utils';
import { useFormErrorHandler } from '@/modules/core/services/errors/form-error-handler';
import { useValidators } from '@/modules/shared/composables/validators';
import { storeToRefs } from 'pinia';
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { VForm, VTextField } from 'vuetify/components';
import { useAppsStore } from '../store/apps-store';

interface ScopeViewModel {
    value: string;
    checked: boolean;
}

// composables
const { t } = useI18n();

const { scopes } = storeToRefs(useAppsStore());
const scopeViewModels = computed<ScopeViewModel[]>(() =>
    scopes.value.map(i => ({
        value: i.value,
        checked: selectedScopes.value.includes(i.value),
    }))
);

// flags
const showDialog = ref(false);
const isLoading = ref(false);
const isSaving = ref(false);

// form
const form = ref<VForm | null>(null);
const { required, length, minArrayLength } = useValidators();

const appName = ref('');
const appNameValidators = [required, length(4, 256)];
const appNameInputRef = ref<VTextField | null>(null);

const selectedScopes = ref<string[]>([]);
const selectedScopesValidators = [minArrayLength(1)];

const dtoValueProvider: ValueProvider<CreateApplicationDto> = {
    expirationDate: () => null,
    name: () => appName,
    scopes: () => selectedScopes,
};

const { setApiError, resetApiError, apiErrorAlert, nameApiErrors } = useFormErrorHandler({
    valueProvider: dtoValueProvider,
    apiAlertIgnoredProps: ['name'],
});

function toggleScope(scope: string) {
    // making a copy of the array, so the UI correctly shows the validation errors
    const newScopes = [...selectedScopes.value];

    const index = newScopes.indexOf(scope);
    if (index > -1) newScopes.splice(index, 1);
    else newScopes.push(scope);

    selectedScopes.value = newScopes;
}

async function addApplication() {
    resetApiError();

    if (isSaving.value || form.value == null) return;

    const validationResult = await form.value.validate();
    if (!validationResult.valid) return;

    isSaving.value = true;
    const payload = createObject(dtoValueProvider);
    const response = await ApplicationsClient.createApplication(payload);

    if (response.isSuccess) {
        emit('created', { name: appName.value, ...response.data });
        showDialog.value = false;
    } else {
        setApiError(response.error);
        if (nameApiErrors.value.length > 0) {
            appNameInputRef.value?.focus();
        }
    }

    isSaving.value = false;
}

// reset the values when the dialog is opening
watch(showDialog, shouldShow => {
    if (!shouldShow) return;

    resetApiError();
    selectedScopes.value = [];
    appName.value = '';
});

const { loadScopes } = useAppsStore();
onMounted(async () => {
    isLoading.value = true;
    await loadScopes();
    isLoading.value = false;
});

const emit = defineEmits<{
    created: [result: CreatedApplication];
}>();
</script>
