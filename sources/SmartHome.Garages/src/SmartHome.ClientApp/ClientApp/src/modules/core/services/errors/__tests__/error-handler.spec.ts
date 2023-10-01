import { describe, expect, it, vi } from 'vitest';
import { ref } from 'vue';
import { BadRequestError } from '../error.models';
import { useFormErrorHandler } from '../form-error-handler';
import { createHttpError } from './error-tests.utils';

interface TestDto {
    name: string;
    value: number;
}

const isDevFlag = ref(false);
vi.mock('@/modules/shared/composables/is-dev', () => ({
    useDev: vi.fn(() => ({ isDev: isDevFlag.value })),
}));

describe('Error handler', () => {
    it('should provide named api errors when BadRequestError was set with DTO props errors', () => {
        const { setApiError, nameApiErrors, valueApiErrors, apiErrorAlert } = useFormErrorHandler<TestDto>({
            valueProvider: {
                name: () => 'test',
                value: () => 123,
            },
        });

        setApiError(
            new BadRequestError(createHttpError(400), {
                errors: {
                    Name: ['test1', 'test2'],
                    Value: ['message'],
                },
            })
        );

        expect(apiErrorAlert.value?.messages).toEqual(['test1', 'test2', 'message']);
        expect(nameApiErrors.value).toEqual(['test1', 'test2']);
        expect(valueApiErrors.value).toEqual(['message']);
    });

    it('should hide named api error when the value was updated', () => {
        const nameValue = ref('test');

        const { setApiError, nameApiErrors, apiErrorAlert } = useFormErrorHandler<TestDto>({
            valueProvider: {
                name: () => nameValue.value,
                value: () => 123,
            },
        });

        setApiError(
            new BadRequestError(createHttpError(400), {
                errors: {
                    Name: ['test1', 'test2'],
                    Value: ['message'],
                },
            })
        );

        expect(apiErrorAlert.value?.messages).toEqual(['test1', 'test2', 'message']);
        expect(nameApiErrors.value).toEqual(['test1', 'test2']);

        nameValue.value = 'otherValue';
        expect(nameApiErrors.value).toEqual([]);

        nameValue.value = 'test';
        expect(nameApiErrors.value).toEqual(['test1', 'test2']);
    });

    it('should not provide alert data when apiAlertIgnoredProps is provided and it is not dev environment', () => {
        const { setApiError, apiErrorAlert } = useFormErrorHandler<TestDto>({
            valueProvider: {
                name: () => 'test',
                value: () => 123,
            },
            apiAlertIgnoredProps: ['name', 'value'],
        });

        setApiError(
            new BadRequestError(createHttpError(400), {
                errors: {
                    Name: ['test1'],
                },
            })
        );

        expect(apiErrorAlert.value).toBe(null);
    });

    it('should provide alert data when apiAlertIgnoredProps is provided and it is dev environment', () => {
        isDevFlag.value = true;
        const { setApiError, apiErrorAlert } = useFormErrorHandler<TestDto>({
            valueProvider: {
                name: () => 'test',
                value: () => 123,
            },
            apiAlertIgnoredProps: ['name', 'value'],
        });

        setApiError(
            new BadRequestError(createHttpError(400), {
                errors: {
                    Name: ['test1'],
                },
            })
        );

        expect(apiErrorAlert.value).not.toBe(null);
        expect(apiErrorAlert.value?.messages.length).toBe(1);
    });
});
