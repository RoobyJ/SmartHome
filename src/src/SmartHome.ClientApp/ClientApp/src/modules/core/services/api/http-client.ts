import i18n from '@/plugins/i18n';
import { StringUtils } from '@cmpl/core';
import ky from 'ky';
import type { KyInstance } from 'node_modules/ky/distribution/types/ky';
import {
    BadRequestError,
    ForbiddenError,
    InternalServerError,
    NotFoundError,
    UnauthorizedError,
} from '../errors/error.models';

export const httpClient: KyInstance = ky.create({
    redirect: 'follow',
    hooks: {
        beforeRequest: [
            request => {
                if (StringUtils.isNotEmpty(i18n.global.locale.value)) {
                    request.headers.set('Accept-Language', i18n.global.locale.value);
                }
            },
        ],
        beforeError: [
            async error => {
                const requestPath = new URL(error.request.url).pathname;
                if (requestPath.startsWith('/account')) {
                    if (error.response.status === 404) {
                        return new NotFoundError(error, {
                            type: 'https://tools.ietf.org/html/rfc7231#section-6.5.4',
                            status: 404,
                            title: 'NotFoundException',
                        });
                    }
                }

                if (requestPath.startsWith('/api')) {
                    const errorResponseBody = await error.response.json();
                    if (error.response.status === 400) return new BadRequestError(error, errorResponseBody);
                    if (error.response.status === 401) return new UnauthorizedError(error, errorResponseBody);
                    if (error.response.status === 403) return new ForbiddenError(error, errorResponseBody);
                    if (error.response.status === 404) return new NotFoundError(error, errorResponseBody);
                    if (error.response.status === 500) return new InternalServerError(error, errorResponseBody);
                }

                return error;
            },
        ],
    },
});
