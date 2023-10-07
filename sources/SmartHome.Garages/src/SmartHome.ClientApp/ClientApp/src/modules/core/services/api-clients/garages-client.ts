import { useErrorStore } from '../../store/error-store';
import type {
    GarageDetailsDto
} from '../api/api.models';
import { createApiResponse, type ApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';

export class GaragesClient {
    private static readonly urlBase = '/api';

    /** Gets application for the current organization */
    public static async getGarages(): Promise<ApiResponse<GarageDetailsDto[]>> {
        const url = GaragesClient.urlBase + '/garages';
        const request = httpClient.get(url).json<GarageDetailsDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }
}
