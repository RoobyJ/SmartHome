import { computed, ref } from 'vue';
import type { ErrorResponse, ProblemDetails } from './error.models';
import type { ApiErrorAlert, StringKeys } from './error.utils';

export function useApiErrorAlert<TDto extends object>(apiAlertIgnoredProps: StringKeys<TDto>[] | null = null) {
    const error = ref<ErrorResponse<ProblemDetails> | null>(null);
    const alertHidden = ref(false);

    const apiErrorAlert = computed<ApiErrorAlert | null>({
        get() {
            if (alertHidden.value || error.value == null) return null;

            const result: ApiErrorAlert = {
                messages: [],
                response: error.value as ErrorResponse<ProblemDetails>,
                showDetails: false,
            };

            if (error.value.isBadRequestError()) {
                for (const [propKey, propMessages] of error.value.messages) {
                    if (apiAlertIgnoredProps?.includes(propKey as any)) continue;
                    result.messages.push(...propMessages);
                }

                if (result.messages.length === 0 && result.showDetails) {
                    // if no custom messages, and the show details btn should be visible
                    // then display the generic message as well, so the alert will have some text
                    result.messages.push(error.value.message);
                }
            } else {
                result.messages.push(error.value.message);
            }

            if (result.messages.length === 0) return null;

            return result;
        },
        set(value) {
            alertHidden.value = true;
        },
    });

    function resetApiError() {
        error.value = null;
        alertHidden.value = false;
    }

    function setApiError(potentialApiError: unknown) {
        resetApiError();
        error.value = potentialApiError as ErrorResponse<ProblemDetails>;
    }

    return { apiErrorAlert, resetApiError, setApiError };
}
