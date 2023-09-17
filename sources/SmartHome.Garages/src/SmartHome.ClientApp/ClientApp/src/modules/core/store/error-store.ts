import { useCountdownTimer } from '@/modules/shared/composables/countdown-timer';
import { acceptHMRUpdate, defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { AccountClient } from '../services/api-clients/account-client';
import { type ErrorResponse, type ProblemDetails } from '../services/errors/error.models';

export interface ProcessErrorOptions {
    /**
     * If true (default), then error dialog will be always displayed with the error details.
     *
     * If false, then error dialog will be displayed only when redirectToLoginWhenUnauthorized is true
     **/
    displayErrorDialog: boolean;
    /** If true (default), then redirection to login page will happen after some delay. */
    redirectToLoginWhenUnauthorized: boolean;
}

const defaultOptions: ProcessErrorOptions = {
    displayErrorDialog: true,
    redirectToLoginWhenUnauthorized: true,
};

export const useErrorStore = defineStore('error-store', () => {
    const { initializeCountdown, seconds, startCountdown, isCounting, skipCountdown } = useCountdownTimer();

    const error = ref<ErrorResponse<ProblemDetails> | null>(null);
    const errorDialogVisible = ref<boolean>(false);

    const errorDialogValue = computed(() => (errorDialogVisible.value ? error.value : null));

    const clearErrorDialogValue = () => {
        // dialog cannot be closed, when the countdown to redirection is in progress
        if (isCounting.value) return;

        error.value = null;
        errorDialogVisible.value = false;
    };

    /** Process the error */
    const processError = async (payload: ErrorResponse<ProblemDetails>, options?: Partial<ProcessErrorOptions>) => {
        // if there is already an error being processed, then skip the processing of other errors
        if (error.value != null) return;

        error.value = payload;

        const { displayErrorDialog, redirectToLoginWhenUnauthorized }: ProcessErrorOptions = {
            ...defaultOptions,
            ...(options ?? {}),
        };

        if (displayErrorDialog) errorDialogVisible.value = true;
        if (redirectToLoginWhenUnauthorized && payload.isUnauthorizedError()) {
            errorDialogVisible.value = true;

            initializeCountdown({
                seconds: 5,
                callback: () => AccountClient.login(),
            });

            startCountdown();
        }

        // if error is not displayed in the dialog, then clear the value, so other errors can be processed
        if (!errorDialogVisible.value) error.value = null;
    };

    return {
        errorDialogValue,
        processError,
        clearErrorDialogValue,
        redirectionInSeconds: seconds,
        redirectionTimerEnabled: isCounting,
        skipRedirectionTimer: skipCountdown,
    };
});

if (import.meta.hot) {
    import.meta.hot.accept(acceptHMRUpdate(useErrorStore, import.meta.hot));
}
