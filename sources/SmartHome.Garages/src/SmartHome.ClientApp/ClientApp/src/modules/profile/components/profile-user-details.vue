<template>
    <app-panel :card-title="t('Profile.ProfileDetails')">
        <div class="d-flex align-center pl-8 py-7">
            <div>
                <v-avatar color="primary" size="120" class="user-initials">{{ initials }}</v-avatar>
            </div>
            <div class="ml-13 mb-5 user-avatar-details">
                <div class="mb-2 text-truncate user-name">{{ name ?? '' }}</div>
                <div class="d-flex align-center user-avatar-email">
                    <v-icon color="readonly" size="13" class="mr-1">mdi-email-outline</v-icon>
                    <div class="font-weight-medium text-truncate">{{ email }}</div>
                </div>
            </div>
        </div>
        <v-divider :thickness="2" />
        <v-form ref="form">
            <v-container class="pb-4 details-container">
                <div class="d-flex align-center justify-space-between mb-4 panel-name">
                    <div class="font-weight-medium mt-2">{{ t('Profile.Details') }}</div>
                    <div v-if="!editDetails">
                        <v-btn-secondary density="comfortable" @click="setDetailsFields">
                            {{ t('Core.Edit') }}
                        </v-btn-secondary>
                    </div>
                </div>
                <app-panel-row
                    v-model="changeableFirstName"
                    :title="t('Core.FirstName')"
                    :edit="editDetails"
                    :rules="nameFieldValidators"
                />
                <app-panel-row
                    v-model="changeableLastName"
                    :title="t('Core.LastName')"
                    :edit="editDetails"
                    :rules="nameFieldValidators"
                />
                <app-panel-row :title="t('Core.Email')" :model-value="email" :rules="nameFieldValidators" />
                <div v-if="editDetails" class="d-flex align-center justify-end bottom-buttons">
                    <div>
                        <v-btn-primary
                            :loading="isSaving"
                            :disabled="!allowSaveChanges"
                            density="comfortable"
                            @click="changeDetails"
                        >
                            {{ t('Core.Save') }}
                        </v-btn-primary>
                    </div>
                    <div>
                        <v-btn-secondary density="comfortable" @click="setDetailsFields">
                            {{ t('Core.Cancel') }}
                        </v-btn-secondary>
                    </div>
                </div>
            </v-container>
        </v-form>
    </app-panel>
</template>

<script setup lang="ts">
import appPanel from '@/modules/core/components/app-panel.vue';
import { MeClient } from '@/modules/core/services/api-clients/me-client';
import type { EditUserDetailsDto } from '@/modules/core/services/api/api.models';
import { useAccountStore } from '@/modules/core/store/account-store';
import AppPanelRow from '@/modules/profile/components/app-panel-row.vue';
import { useValidators } from '@/modules/shared/composables/validators';
import { storeToRefs } from 'pinia';
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { VForm } from 'vuetify/components';

const { t } = useI18n();
const { name, initials, email, firstName, lastName } = storeToRefs(useAccountStore());

const isSaving = ref(false);
const changeableFirstName = ref('');
const changeableLastName = ref('');
const editDetails = ref(false);
const form = ref<VForm | null>(null);

const { required, length } = useValidators();
const nameFieldValidators = [required, length(1, 128)];

const allowSaveChanges = computed(() => {
    return changeableFirstName.value !== firstName.value || changeableLastName.value !== lastName.value;
});

onMounted(() => {
    changeableFirstName.value = firstName.value ?? '';
    changeableLastName.value = lastName.value ?? '';
});

function setDetailsFields() {
    editDetails.value = !editDetails.value;

    changeableFirstName.value = firstName.value ?? '';
    changeableLastName.value = lastName.value ?? '';
}

async function changeDetails() {
    if (form.value == null || isSaving.value) return;

    const validationResult = await form.value.validate();
    if (!validationResult.valid) return;

    const details: EditUserDetailsDto = {
        firstName: changeableFirstName.value,
        lastName: changeableLastName.value,
    };

    isSaving.value = true;
    const success = await MeClient.editUserDetails(details);
    isSaving.value = false;

    if (!success) return;

    const { setUserDetails } = useAccountStore();
    setUserDetails(details);

    editDetails.value = false;
}
</script>

<style lang="scss">
.user-initials {
    font-size: 40px;
    font-weight: 600;
}

.user-avatar-details {
    color: #001a31de;
    min-width: 0;
}

.user-name {
    font-size: 24px;
    line-height: 33px;
    font-weight: 600;
}

.user-avatar-email {
    font-size: 12px;
    height: 17px;
}

.details-container {
    padding-top: 17px;
    padding-left: 30px;
    padding-right: 25px;
}

.panel-name {
    font-size: 14px;
    min-height: 36px;
}

.bottom-buttons {
    gap: 16px;
    padding-top: 26px;
    padding-bottom: 14px;
}
</style>
