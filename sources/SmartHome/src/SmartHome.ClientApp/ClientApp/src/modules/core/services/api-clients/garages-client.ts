import { useErrorStore } from '../../store/error-store';
import type {
    NewCyclicHeatTaskDto,
    NewCustomHeatTaskDto,
    CyclicHeatTaskDto,
    GarageDetailsDto,
    TemperatureDto,
    CustomHeatTaskDto
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
        const url = this.urlBase + `/${id}/cyclicHeatTasks`;
        const request = httpClient.get(url).json<CyclicHeatTaskDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets all custom heat request for garage */
    public static async getCustomHeatRequests(id: string): Promise<ApiResponse<CustomHeatTaskDto[]>> {
        const url = this.urlBase + `/${id}/customHeatTasks`;
        const request = httpClient.get(url).json<CustomHeatTaskDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets temperatures for last 30 days for garage */
    public static async getGarageTemperatures(id: string): Promise<ApiResponse<TemperatureDto[]>> {
        const url = this.urlBase + `/${id}/temperatures`;
        const request = httpClient.get(url).json<TemperatureDto[]>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given custom heat request to db */
    public static async saveCustomHeatRequest(id: string, payload: NewCustomHeatTaskDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/customHeatTasks`;
        const request = httpClient.post(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given custom heat request to db */
    public static async editCustomHeatRequest(id: string, payload: CustomHeatTaskDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/customHeatTasks`;
        const request = httpClient.put(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given cyclic heat request to db */
    public static async saveCyclicHeatRequest(id: string, payload: NewCyclicHeatTaskDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/cyclicHeatTasks`;
        const request = httpClient.post(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Saves the given cyclic heat request to db */
    public static async editCyclicHeatRequest(id: string, payload: CyclicHeatTaskDto): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/cyclicHeatTasks`;
        const request = httpClient.put(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Deletes given cyclic heat task */
    public static async deleteCyclicHeatRequest(id: string, taskId: number): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/cyclicHeatTasks?requestId=${taskId}`;
        const request = httpClient.delete(url);
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Deletes given custom heat task */
    public static async deleteCustomHeatRequest(id: string, taskId: number): Promise<ApiResponse<any>> {
        const url = this.urlBase + `/${id}/customHeatTasks?requestId=${taskId}`;
        const request = httpClient.delete(url);
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets specified garages heater status */
    public static async getGarageHeaterStatus(garageId: number): Promise<ApiResponse<boolean | null>> {
        const url = this.urlBase + `/garage/heaterstatus?garageId=${garageId}`;
        const request = httpClient.get(url).json<boolean | null>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Gets specified garages heater status */
    public static async setGarageHeaterStatus(garageId: number, statusToSet: boolean): Promise<ApiResponse<void>> {
        const url = this.urlBase + `/garage/${garageId}/heater`;
        const request = httpClient.patch(url, { json: statusToSet }).json<void>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

    /** Sets task status to the opposite */
    public static async changeStatusOfCyclicHeatTask(taskId: number): Promise<ApiResponse<boolean>> {
        const url = this.urlBase + `/cyclicHeatTasks/${taskId}/status`;
        const request = httpClient.patch(url).json<boolean>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }

     /** Sets task status to the opposite */
    public static async changeStatusOfHeatTask(taskId: number): Promise<ApiResponse<boolean>> {
        const url = this.urlBase + `/heatTasks/${taskId}/status`;
        const request = httpClient.patch(url).json<boolean>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return apiResponse;
    }
}
