import { HTTPError } from 'ky';

export function createHttpError(status: number): HTTPError {
    const request = new Request('https://localhost:3010/api', {
        method: 'GET',
    });

    const response = new Response(null, { status });

    return new HTTPError(response, request, {
        method: 'GET',
        credentials: undefined,
        onDownloadProgress: undefined,
        prefixUrl: '',
        retry: {},
    });
}
