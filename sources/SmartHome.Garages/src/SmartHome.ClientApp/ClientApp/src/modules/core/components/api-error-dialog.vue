<template>
    <v-dialog
        :model-value="errorDialogValue != null"
        :max-width="displayException ? 720 : 480"
        @update:model-value="clearErrorDialogValue()"
    >
        <v-card>
            <v-card-title class="center">
                <v-icon size="80" color="error" class="mb-2">mdi-close-circle</v-icon>
                {{ t('Core.ErrorDetails') }}
            </v-card-title>
            <v-card-text>
                <div v-for="message in messages" :key="message" class="message">{{ message }}</div>

                <div v-if="redirectionTimerEnabled" class="message font-weight-regular">
                    {{
                        t(
                            'Core.InXSecondsARedirectionToTheLoginPageWillTakePlaceClickTheButtonBelowToLogInAgainImmediately',
                            [redirectionInSeconds]
                        )
                    }}
                </div>
            </v-card-text>
            <v-card-text v-if="displayException">
                <v-sheet class="error-dialog-details" color="error-dialog-details">
                    <template v-if="endpoint != null">
                        <div class="error-dialog-details__title">Endpoint</div>
                        <div class="error-dialog-details__value">{{ endpoint }}</div>
                    </template>

                    <template v-if="path != null">
                        <div class="error-dialog-details__title">Path</div>
                        <div class="error-dialog-details__value">{{ method + ' ' + path }}</div>
                    </template>

                    <template v-if="stackTrace != null">
                        <div class="error-dialog-details__title">StackTrace</div>
                        <div class="error-dialog-details__value">{{ stackTrace }}</div>
                    </template>
                </v-sheet>
            </v-card-text>
            <v-card-actions>
                <v-btn-primary v-if="displayException" @click="copy">
                    <v-icon :icon="btnIcon" start />
                    {{ t(btnText) }}
                </v-btn-primary>
                <v-btn-secondary v-if="redirectionTimerEnabled" @click="skipRedirectionTimer()">
                    {{ t('Core.Redirect') }}
                </v-btn-secondary>
                <v-btn-secondary v-else @click="clearErrorDialogValue()">{{ t('Core.Close') }}</v-btn-secondary>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { useDev } from '@/modules/shared/composables/is-dev';
import { storeToRefs } from 'pinia';
import { computed, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useErrorStore } from '../store/error-store';

const { t } = useI18n();

const { clearErrorDialogValue, skipRedirectionTimer } = useErrorStore();
const { errorDialogValue, redirectionInSeconds, redirectionTimerEnabled } = storeToRefs(useErrorStore());

watch(errorDialogValue, (value, oldValue) => {
    if (value != null && value !== oldValue) isCopied.value = false;
});

const { isDev } = useDev();

const exception = computed(() => errorDialogValue.value?.problem.exception);
const displayException = computed(() => isDev && exception.value != null);

const endpoint = computed(() => exception.value?.endpoint);
const method = computed(() => errorDialogValue.value?.request.method.toUpperCase() ?? '');
const path = computed(() => exception.value?.path);
const stackTrace = computed(() => exception.value?.details);

const isCopied = ref(false);
const btnIcon = computed(() => (isCopied.value ? 'mdi-check' : 'mdi-content-copy'));
const btnText = computed(() => (isCopied.value ? 'Core.Copied' : 'Core.CopyText'));

const messages = computed(() => {
    const result: string[] = [];
    if (errorDialogValue.value == null) return result;

    if (errorDialogValue.value.isBadRequestError()) {
        for (const propMessages of errorDialogValue.value.messages.values()) {
            result.push(...propMessages);
        }
    }

    if (result.length === 0) result.push(errorDialogValue.value.message);
    return result;
});

const copy = async () => {
    const message = `Messages
${messages.value.map(i => '* ' + i).join('\r\n')}

Endpoint
${endpoint.value ?? '-'}

Path
${method.value} ${path.value ?? '-'}

StackTrace
${stackTrace.value ?? '-'}`;

    await navigator.clipboard.writeText(message);
    isCopied.value = true;
};
</script>

<style lang="scss" scoped>
.message {
    display: flex;
    justify-content: center;
    font-weight: 500;
    text-align: center;
    margin: 4px 0;
}

.error-dialog-details {
    padding: 12px;
    font-size: 12px;

    overflow-x: scroll;
    white-space: pre;

    &__title {
        font-weight: 500;

        &:not(:first-child) {
            margin-top: 16px;
        }
    }
}
</style>
