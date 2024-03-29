import { HTTPError } from 'ky';
import { StringUtils } from '../utils'

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

    public isBadRequestError(): this is BadRequestError {
        return this.response.status === 400;
    }

    public isUnauthorizedError(): this is UnauthorizedError {
        return this.response.status === 401;
    }

    public isForbiddenError(): this is ForbiddenError {
        return this.response.status === 403;
    }

    public isNotFoundError(): this is NotFoundError {
        return this.response.status === 404;
    }

    public isInternalServerError(): this is InternalServerError {
        return this.response.status === 500;
    }

    constructor({ response, request, options }: HTTPError, problem: TProblem) {
        super(response, request, options);
        this.problem = problem;
        this.message = this.getCustomMessage(problem) ?? 'AnUnexpectedErrorOccurred';
    }

    protected getCustomMessage(problem: TProblem): string | null {
        return null;
    }
}

export class BadRequestError extends ErrorResponse<ValidationProblemDetails> {
    public readonly messages: Map<string, string[]>;
    public static readonly genericMessageKey = 'generic';

    constructor(error: HTTPError, problem: ValidationProblemDetails) {
        super(error, problem);
        this.messages = BadRequestError.processErrorMessages(problem);
    }

    private static processErrorMessages(problem: ValidationProblemDetails): Map<string, string[]> {
        const result = new Map<string, string[]>();

        const errors = problem.errors ?? {};
        for (const errorKey in errors) {
            // .NET props comes in PascalCase, so we map them to camelCase.

            let propName = errorKey.trim();
            if (StringUtils.isEmpty(propName)) {
                propName = this.genericMessageKey;
            }

            propName = propName[0].toLowerCase() + propName.slice(1);

            const messages = errors[errorKey].filter(i => StringUtils.isNotEmpty(i));
            if (messages.length === 0) continue;

            if (!result.has(propName)) result.set(propName, []);
            result.get(propName)?.push(...messages);
        }

        return result;
    }

    protected override getCustomMessage(problem: ProblemDetails): string | null {
        const title = problem.title?.toLowerCase() ?? '';
        return title.includes('validation') ? 'ValidationErrorOccurred' : null;
    }
}

export class NotFoundError extends ErrorResponse<ProblemDetails> {
    protected override getCustomMessage(problem: ProblemDetails): string | null {
        return problem.detail ?? null;
    }
}

export class UnauthorizedError extends ErrorResponse<ProblemDetails> {
    protected override getCustomMessage(problem: ProblemDetails): string | null {
        return 'YourSessionHasExpired';
    }
}

export class ForbiddenError extends ErrorResponse<ProblemDetails> {
    protected override getCustomMessage(problem: ProblemDetails): string | null {
        return 'NoRequiredPermissionsToTheResource';
    }
}

export class InternalServerError extends ErrorResponse<ProblemDetails> {}
