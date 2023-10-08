import { useErrorStore } from '../../store/error-store';
import type {
    CyclicHeatRequestDto,
    GarageDetailsDto
} from '../api/api.models';
import { createApiResponse, type ApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';

export class GarageClient {
    private static readonly urlBase = '/api';

    /** Gets all garages */
    public static async getGarages(): Promise<ApiResponse<GarageDetailsDto[]>> {
        const url = this.urlBase + '/garages';
        const request = httpClient.get(url).json<GarageDetailsDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

     /** Gets all heat request for garage */
     public static async getCyclicHeatRequests(id: string): Promise<ApiResponse<CyclicHeatRequestDto[]>> {
        const url = this.urlBase + `/${id}/CyclicHeatTimes`;
        const request = httpClient.get(url).json<CyclicHeatRequestDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }
}
