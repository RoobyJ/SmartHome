import { useErrorStore } from '../../store/error-store';
import type {
    CreateCyclicHeatTaskDto,
    CustomHeatTaskDto,
    CyclicHeatTaskDto,
    GarageDetailsDto,
    HeatRequestDto,
    TemperatureDto
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

    /** Gets all cyclic heat request for garage */
    public static async getCyclicHeatRequests(id: string): Promise<ApiResponse<CyclicHeatTaskDto[]>> {
        const url = this.urlBase + `/${id}/CyclicHeatTimes`;
        const request = httpClient.get(url).json<CyclicHeatTaskDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets all custom heat request for garage */
    public static async getCustomHeatRequests(id: string): Promise<ApiResponse<CustomHeatTaskDto[]>> {
        const url = this.urlBase + `/${id}/heatTimeRequests`;
        const request = httpClient.get(url).json<CustomHeatTaskDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets temperatures for last 30 days for garage */
    public static async getGarageTemperatures(id: string): Promise<ApiResponse<TemperatureDto[]>> {
        const url = this.urlBase + `/${id}/Temperatures`;
        const request = httpClient.get(url).json<TemperatureDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given custom heat request to db */
    public static async saveCustomHeatRequest(id: string, payload: HeatRequestDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/heatTimeRequests`;
        const request = httpClient.post(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given cyclic heat request to db */
    public static async saveCyclicHeatRequest(id: string, payload: CreateCyclicHeatTaskDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/CyclicHeatTimes`;
        const request = httpClient.post(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Deletes given cyclic heat task */
    public static async deleteCyclicHeatRequest(id: string, taskId: number): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/CyclicHeatTimes?requestId=${taskId}`;
        const request = httpClient.delete(url);
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }
}
