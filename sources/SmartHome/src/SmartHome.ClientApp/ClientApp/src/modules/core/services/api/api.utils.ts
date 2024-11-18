import { toValue, type MaybeRefOrGetter } from 'vue';
import type { ErrorResponse, ProblemDetails } from '../errors/error.models';

export type ValueProvider<TObject extends object> = {
    [TProp in keyof TObject]: () => MaybeRefOrGetter<TObject[TProp] | null | undefined>;
};

export interface SuccessResponse<TData> {
    readonly isSuccess: true;
    readonly data: TData;
}

export interface FailedResponse {
    readonly isSuccess: false;
    readonly error: ErrorResponse<ProblemDetails>;
}

export type ApiResponse<TData> = SuccessResponse<TData> | FailedResponse;

export function createObject<TObject extends object>(valueProvider: ValueProvider<TObject>): TObject {
    const result: any = {};

    for (const key in valueProvider) {
        result[key] = toValue(valueProvider[key]());
    }

    return result;
}

export async function createApiResponse<TData>(request: Promise<TData>): Promise<ApiResponse<TData>> {
    let data: TData | null = null;
    let error: ErrorResponse<ProblemDetails> | null = null;

    await request.then(i => (data = i)).catch(i => (error = i));

    if (error != null) return { isSuccess: false, error };
    return { isSuccess: true, data: data as TData };
}
