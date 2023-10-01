import { StringUtils } from '@cmpl/core';
import { describe, expect, it } from 'vitest';
import {
    BadRequestError,
    ForbiddenError,
    InternalServerError,
    NotFoundError,
    UnauthorizedError,
    type ProblemDetails,
    type ValidationProblemDetails,
} from '../error.models';
import { createHttpError } from './error-tests.utils';

describe('Error responses mapping', () => {
    it('should correctly create NotFoundError when problem detail is defined', () => {
        const httpError = createHttpError(404);

        // details is translated by the API for NotFoundError, so it can be used as a message
        const problem: ProblemDetails = { detail: 'test' };

        const error = new NotFoundError(httpError, problem);

        expect(error.message).toBe(problem.detail);
        expect(error.isBadRequestError()).toBe(false);
        expect(error.isUnauthorizedError()).toBe(false);
        expect(error.isForbiddenError()).toBe(false);
        expect(error.isInternalServerError()).toBe(false);
        expect(error.isNotFoundError()).toBe(true);
    });

    it('should correctly create NotFoundError when problem detail is not defined', () => {
        const httpError = createHttpError(404);
        const problem: ProblemDetails = {};

        const error = new NotFoundError(httpError, problem);

        expect(StringUtils.isNotEmpty(error.message)).toBe(true);
        expect(error.isBadRequestError()).toBe(false);
        expect(error.isUnauthorizedError()).toBe(false);
        expect(error.isForbiddenError()).toBe(false);
        expect(error.isInternalServerError()).toBe(false);
        expect(error.isNotFoundError()).toBe(true);
    });

    it('should correctly create UnauthorizedError', () => {
        const httpError = createHttpError(401);

        // detail will not be translated by the API for UnauthorizedError
        const problem: ProblemDetails = { detail: 'test' };

        const error = new UnauthorizedError(httpError, problem);

        expect(error.message).not.toBe(problem.detail);
        expect(error.isBadRequestError()).toBe(false);
        expect(error.isUnauthorizedError()).toBe(true);
        expect(error.isForbiddenError()).toBe(false);
        expect(error.isInternalServerError()).toBe(false);
        expect(error.isNotFoundError()).toBe(false);
    });

    it('should correctly create ForbiddenError', () => {
        const httpError = createHttpError(403);

        // detail will not be translated by the API for ForbiddenError
        const problem: ProblemDetails = { detail: 'test' };

        const error = new ForbiddenError(httpError, problem);

        expect(error.message).not.toBe(problem.detail);
        expect(error.isBadRequestError()).toBe(false);
        expect(error.isUnauthorizedError()).toBe(false);
        expect(error.isForbiddenError()).toBe(true);
        expect(error.isInternalServerError()).toBe(false);
        expect(error.isNotFoundError()).toBe(false);
    });

    it('should correctly create InternalServerError', () => {
        const httpError = createHttpError(500);

        // detail will not be translated by the API for InternalServerError
        const problem: ProblemDetails = { detail: 'test' };

        const error = new InternalServerError(httpError, problem);

        expect(error.message).not.toBe(problem.detail);
        expect(error.isBadRequestError()).toBe(false);
        expect(error.isUnauthorizedError()).toBe(false);
        expect(error.isForbiddenError()).toBe(false);
        expect(error.isInternalServerError()).toBe(true);
        expect(error.isNotFoundError()).toBe(false);
    });

    it('should correctly create BadRequestError', () => {
        const httpError = createHttpError(400);
        const problem: ValidationProblemDetails = {
            detail: 'test',
            errors: {
                Name: ['test1', 'test2'],
                Age: ['message'],
            },
        };

        const error = new BadRequestError(httpError, problem);

        expect(error.message).not.toBe(problem.detail);
        expect(error.isBadRequestError()).toBe(true);
        expect(error.isUnauthorizedError()).toBe(false);
        expect(error.isForbiddenError()).toBe(false);
        expect(error.isInternalServerError()).toBe(false);
        expect(error.isNotFoundError()).toBe(false);
        expect(error.messages.size).toBe(2);
        expect(error.messages.has('name')).toBe(true);
        expect(error.messages.has('age')).toBe(true);
        expect(error.messages.get('name')).toEqual(['test1', 'test2']);
        expect(error.messages.get('age')).toEqual(['message']);
    });
});
