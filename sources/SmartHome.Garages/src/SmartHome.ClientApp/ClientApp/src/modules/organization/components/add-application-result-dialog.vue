<template>
    <v-dialog :model-value="true" max-width="460px" @update:model-value="shouldClose => shouldClose || emit('close')">
        <v-card>
            <v-card-title class="center">
                <v-icon size="80" color="primary" class="mb-2">dm:success</v-icon>
                {{ t('Applications.AddedApplication') }}
            </v-card-title>
            <v-card-text class="result-dialog-subtitle d-flex justify-center">
                {{ t('Applications.SaveTheGeneratedIDAndKeyInASavePlace') }}
            </v-card-text>
            <v-card-text class="result-dialog-text d-flex justify-center">
                {{ t('Applications.TheKeyWillNoLongerBeAvailableWhenThisWindowIsClosed') }}
            </v-card-text>
            <v-card-item>
                <v-sheet class="input-sheet px-1" width="100%">
                    <v-container fluid class="client-secret-container">
                        <v-row>
                            <v-col align-self="center" class="message pl-6 text-uppercase">
                                {{ t('Applications.Id') }}
                            </v-col>
                            <v-col align-self="center" cols="10" class="message">
                                {{ app?.clientId }}
                            </v-col>
                        </v-row>
                        <v-row class="mt-0">
                            <v-col align-self="center" class="message pl-6 pr-0" color="primary">
                                {{ t('Applications.Key') }}
                            </v-col>
                            <v-col align-self="center" cols="10" class="message">
                                <div color="primary">{{ app?.clientSecret }}</div>
                            </v-col>
                        </v-row>
                    </v-container>
                </v-sheet>
            </v-card-item>
            <v-card-actions>
                <v-btn-primary @click="copyText()">
                    <v-icon :icon="btnIcon" start color="white" />
                    {{ t(btnText) }}
                </v-btn-primary>
                <v-btn-secondary @click="emit('close')">{{ t('Core.Close') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import type { CreatedApplication } from '@/modules/core/core.models';
import { computed, ref, type PropType } from 'vue';
import { useI18n } from 'vue-i18n';

const emit = defineEmits(['close']);
const props = defineProps({
    app: { type: Object as PropType<CreatedApplication | null>, required: true },
});

const { t } = useI18n();

const isCopied = ref(false);
const btnIcon = computed(() => (isCopied.value ? 'mdi-check' : 'mdi-content-copy'));
const btnText = computed(() => (isCopied.value ? 'Core.Copied' : 'Core.CopyText'));

const copyText = async () => {
    if (props.app == null) return;
    const textToCopy = `${t('Applications.Application')}: ${props.app.name}
${t('Applications.Id')}: ${props.app.clientId}
${t('Applications.Key')}: ${props.app.clientSecret}`;
    await navigator.clipboard.writeText(textToCopy);
    isCopied.value = true;
};
</script>

<style lang="scss" scoped>
.result-dialog-title {
    font-size: 26px;
    max-width: 400px;
    display: flex;
    justify-self: center;
    white-space: wrap;
    text-align: center;
}

.result-dialog-subtitle {
    font-size: 14px;
    display: flex;
    color: black;
    text-align: center;
    white-space: wrap;
}

.result-dialog-text {
    font-size: 12px !important;
    white-space: nowrap;
}

.v-card.v-card--in-dialog .v-card-actions {
    padding-right: 29px !important;
}

.v-card.v-card--in-dialog .v-card-title {
    padding-top: 20px !important;
}

.v-card.v-card--in-dialog .v-card-text {
    padding-bottom: 8px !important;
}

.v-sheet.input-sheet {
    .client-secret-container {
        border: 1px solid #0288d1;
        border-radius: 8px;
        background-color: #f4f4f5;
        opacity: 1;

        .message {
            font-size: 14px;
            white-space: nowrap;
            color: #0288d1;
        }
    }
}
</style>
