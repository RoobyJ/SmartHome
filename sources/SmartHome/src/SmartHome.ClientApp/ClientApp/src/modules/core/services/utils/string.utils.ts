/**
 * Checks whether provided value is null or in case of a string, whether it consists of only white-spaces.
 * @param value the value that is checked
 */
function isEmpty(value: string | null | undefined): value is null | undefined {
    if (value == null || typeof value !== 'string') return true;

    return value.trim().length <= 0;
}

/**
 * Checks whether provided value is not null and in case of a string, whether it contains at least one non white-space character.
 * @param value the value that is checked
 */
function isNotEmpty(value: string | null | undefined): value is string {
    if (value == null || typeof value !== 'string') return false;

    return value.trim().length > 0;
}

export default {
    isEmpty,
    isNotEmpty,
};
