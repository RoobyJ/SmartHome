import i18n from '@/plugins/i18n';
import { HTTPError } from 'ky';

export interface ExceptionDetails {
    /** Contains exception name and stacktrace */
    details: string;
    endpoint: string;
    path: string;
    headers: Record<string, string[]>;
}

export interface ProblemDetails {
    /** A URI reference [RFC3986] that identifies the problem type. */
    type?: string;
    /** A short, human-readable summary of the problem type */
    title?: string;
    /** The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem. */
    status?: number;
    /**
     * A human-readable explanation specific to this occurrence of the problem.
     * If correctly handled on backend, this property should already be localized.
     **/
    detail?: string;
    /** A URI reference that identifies the specific occurrence of the problem. */
    instance?: string;
    traceId?: string;
    /** Its only set on dev environment */
    exception?: ExceptionDetails;
}

export interface ValidationProblemDetails extends ProblemDetails {
    /**
     * Gets the validation errors associated with this instance.
     * If correctly handled on backend, this property should already be localized.
     **/
    errors?: Record<string, string[]>;
}

export abstract class ErrorResponse<TProblem extends ProblemDetails> extends HTTPError {
    public readonly problem: TProblem;
    public readonly message: string;

    constructor({ response, request, options }: HTTPError, problem: TProblem) {
        super(response, request, options);
        this.problem = problem;

        switch (problem.title) {
            case 'BadRequestException':
            case 'NotFoundException':
                this.message = problem.detail ?? i18n.global.t('Core.AnUnexpectedErrorOccurred');
                break;
            default:
                this.message = i18n.global.t('Core.AnUnexpectedErrorOccurred');
                break;
        }
    }
}

export class BadRequestError extends ErrorResponse<ValidationProblemDetails> {}
export class NotFoundError extends ErrorResponse<ProblemDetails> {}
export class UnauthorizedError extends ErrorResponse<ProblemDetails> {}
export class ForbiddenError extends ErrorResponse<ProblemDetails> {}
export class InternalServerError extends ErrorResponse<ProblemDetails> {}
