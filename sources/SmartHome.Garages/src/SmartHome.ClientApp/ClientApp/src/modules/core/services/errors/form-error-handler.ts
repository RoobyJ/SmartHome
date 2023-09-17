import { useDev } from '@/modules/shared/composables/is-dev';
import type { ComputedRef } from 'vue';
import { computed, ref, toValue } from 'vue';
import type { ValueProvider } from '../api/api.utils';
import {
    BadRequestError,
    ErrorResponse,
    ForbiddenError,
    InternalServerError,
    NotFoundError,
    UnauthorizedError,
    type ProblemDetails,
} from './error.models';
import type { ApiErrorAlert, ApiErrorHandler, StringKeys } from './error.utils';

type NullablePartial<T> = {
    [P in keyof T]?: T[P] | null;
};

export interface ApiErrorHandlerOptions<TDto extends object> {
    valueProvider: ValueProvider<TDto>;
    /** Error messages related to these props will not be included in the alert */
    apiAlertIgnoredProps?: StringKeys<TDto>[];
}

/** Error handler that provides some easy to use elements in the forms */
export function useFormErrorHandler<TDto extends object>({
    valueProvider,
    apiAlertIgnoredProps,
}: ApiErrorHandlerOptions<TDto>): ApiErrorHandler<TDto> {
    let fieldValuesWhenErrorOccurred: NullablePartial<TDto> = {};

    // ref is making a proxy object, so error.value instanceof BadRequestError will not work
    const error = ref<ErrorResponse<ProblemDetails> | null>(null);

    const alertHidden = ref(false);
    const { isDev } = useDev();

    const apiErrorAlert = computed<ApiErrorAlert | null>({
        get() {
            if (alertHidden.value || error.value == null) return null;

            const result: ApiErrorAlert = {
                messages: [],
                response: error.value as ErrorResponse<ProblemDetails>,
                showDetails: isDev,
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
        fieldValuesWhenErrorOccurred = {};
    }

    function setApiError(potentialApiError: unknown) {
        resetApiError();

        if (
            potentialApiError instanceof BadRequestError ||
            potentialApiError instanceof NotFoundError ||
            potentialApiError instanceof UnauthorizedError ||
            potentialApiError instanceof ForbiddenError ||
            potentialApiError instanceof InternalServerError
        ) {
            error.value = potentialApiError;
        }
    }

    function createFieldErrorsGetter(field: keyof TDto): ComputedRef<string[]> {
        return computed<string[]>(() => {
            if (error.value == null || !error.value.isBadRequestError()) return [];

            const fieldValueWhenErrorOccurred = fieldValuesWhenErrorOccurred[field];
            const currentFieldValue = toValue(valueProvider[field]());

            // If user typed something different into the fieldValue, then don't display the error
            if (fieldValueWhenErrorOccurred != null && fieldValueWhenErrorOccurred !== currentFieldValue) {
                return [];
            }

            const messages = error.value.messages.get(field as any);

            // if no error messages associated with the fieldValue, then return empty array
            if (messages == null) return [];

            // store the fieldValue when the error has occurred and return the error messages
            fieldValuesWhenErrorOccurred[field] = currentFieldValue;

            // create a copy, so if the array is mutated, it won't affect the error
            return [...messages];
        });
    }

    const fields: any = {};
    for (const prop in valueProvider) {
        fields[`${prop}ApiErrors`] = createFieldErrorsGetter(prop);
    }

    return {
        resetApiError,
        setApiError,
        apiErrorAlert,
        ...fields,
    };
}
