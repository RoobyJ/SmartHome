import { StringUtils } from '@cmpl/core';
import { useI18n } from 'vue-i18n';

// NOTE: these types are copied from Vuetify's source code as they are not exported publicly by Vuetify
// src: https://github.com/vuetifyjs/vuetify/blob/aa6525ade2554aa50e916e192b995e3f0251bd0f/packages/vuetify/src/composables/validation.ts#L16
type ValidationResult = string | boolean;
export type ValidationRule =
    | ValidationResult
    | PromiseLike<ValidationResult>
    | ((value: any) => ValidationResult)
    | ((value: any) => PromiseLike<ValidationResult>);

interface Validators {
    required: ValidationRule;
    email: ValidationRule;
    minArrayLength: (minLength: number) => ValidationRule;
    minLength: (minLength: number) => ValidationRule;
    maxLength: (maxLength: number) => ValidationRule;
    length: (minLength: number | null, maxLength: number | null) => ValidationRule;
}

export function useValidators(): Validators {
    const { t } = useI18n();

    /** The same validator logic that is used in backend */
    function emailValidator(value: any): ValidationResult {
        const stringValue = convertToString(value);
        if (StringUtils.isEmpty(stringValue)) return true;

        const index = stringValue.indexOf('@');

        // it must contain only one @ character and it must not be the first or the last character
        const isValidEmail = index > 0 && index < stringValue.length - 1 && index === stringValue.lastIndexOf('@');
        return isValidEmail || t('Core.Validators.InvalidEmailAddress');
    }

    function requiredValidator(value: any): ValidationResult {
        const stringValue = convertToString(value);
        return stringValue.length > 0 || t('Core.Validators.RequiredField');
    }

    function minLengthValidator(value: any, minLength: number | null): ValidationResult {
        if (minLength == null || minLength < 0) {
            const minLen = minLength ?? 'null';
            throw new Error(`Invalid arguments for minLength validator. Values { minLength: ${minLen} }`);
        }

        const stringValue = convertToString(value);
        return stringValue.length >= minLength || t('Core.Validators.MinimumNumberOfCharactersX', [minLength]);
    }

    function maxLengthValidator(value: any, maxLength: number | null): ValidationResult {
        if (maxLength == null || maxLength < 0) {
            const maxLen = maxLength ?? 'null';
            throw new Error(`Invalid arguments for maxLength validator. Values { maxLength: ${maxLen} }`);
        }

        const stringValue = convertToString(value);
        return stringValue.length <= maxLength || t('Core.Validators.MaximumNumberOfCharactersX', [maxLength]);
    }

    function lengthValidator(value: any, minLength: number | null, maxLength: number | null): ValidationResult {
        if (
            (minLength == null && maxLength == null) ||
            (minLength != null && minLength < 0) ||
            (maxLength != null && maxLength < 0) ||
            (minLength != null && maxLength != null && minLength > maxLength)
        ) {
            const minLen = minLength ?? 'null';
            const maxLen = maxLength ?? 'null';

            throw new Error(
                `Invalid arguments for length validator. Values { minLength: ${minLen}; maxLength: ${maxLen} }`
            );
        }

        if (minLength == null) return maxLengthValidator(value, maxLength);
        if (maxLength == null) return minLengthValidator(value, minLength);

        const stringValue = convertToString(value);
        return (
            (minLength <= stringValue.length && stringValue.length <= maxLength) ||
            t('Core.Validators.ValueMustContainXToYCharacters', [minLength, maxLength])
        );
    }

    function minArrayLength(value: any, minLength: number | null): ValidationResult {
        if (minLength == null || minLength < 0) {
            const minLen = minLength ?? 'null';
            throw new Error(`Invalid arguments for minArrayLength validator. Values { minLength: ${minLen} }`);
        }

        const arrayValue = value == null || !Array.isArray(value) ? [] : value;
        return arrayValue.length >= minLength || t('Core.Validators.RequiredField');
    }

    return {
        required: requiredValidator,
        email: emailValidator,
        minArrayLength: minLength => value => minArrayLength(value, minLength),
        minLength: minLength => value => minLengthValidator(value, minLength),
        maxLength: maxLength => value => maxLengthValidator(value, maxLength),
        length: (minLength, maxLength) => value => lengthValidator(value, minLength, maxLength),
    };
}

/** Converts any value to string. Value is trimmed. */
function convertToString(value: any) {
    if (typeof value === 'string') return value.trim();
    if (value == null) return '';

    return String(value).trim();
}
