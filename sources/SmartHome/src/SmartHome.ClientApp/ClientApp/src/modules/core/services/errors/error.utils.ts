import type { ComputedRef, WritableComputedRef } from 'vue';
import type { ErrorResponse, ProblemDetails } from './error.models';

export type StringKeys<TObject extends object> = Extract<keyof TObject, string>;

export type ApiErrorHandlerFieldErrors<TDto extends object> = {
    [TProp in StringKeys<TDto> as `${TProp}ApiErrors`]: ComputedRef<string[]>;
};

export type ApiErrorHandler<TDto extends object> = {
    resetApiError: () => void;
    setApiError: (potentialApiError: unknown) => void;
    apiErrorAlert: WritableComputedRef<ApiErrorAlert | null>;
} & ApiErrorHandlerFieldErrors<TDto>;

export interface ApiErrorAlert {
    messages: string[];
    response: ErrorResponse<ProblemDetails>;
    showDetails: boolean;
}
