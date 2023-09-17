import { useErrorStore } from '../../store/error-store';
import type {
    CreateApplicationDto,
    CreatedApplicationDto,
    PaginationOptions,
    PaginatedListResultDto,
    ApplicationDto,
} from '../api/api.models';
import { createApiResponse, type ApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';
import { useAccountStore } from '@/modules/core/store/account-store';

export class ApplicationsClient {
    private static readonly urlBase = '/api/mgmt/apps';

    /** Creates a new application */
    public static async createApplication(payload: CreateApplicationDto): Promise<ApiResponse<CreatedApplicationDto>> {
        const request = httpClient.post(this.urlBase, { json: payload }).json<CreatedApplicationDto>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error, { displayErrorDialog: false });
        return apiResponse;
    }

    /** Gets application for the current organization */
    public static async getApplications(
        paginationOptions: PaginationOptions | null
    ): Promise<PaginatedListResultDto<ApplicationDto>> {
        const url = this.createGetApplicationsUrl(paginationOptions, true);
        const request = httpClient.get(url).json<PaginatedListResultDto<ApplicationDto>>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse.data;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return { items: [], hasNextPage: false, hasPreviousPage: false, pageNumber: 1, totalCount: 0, totalPages: 1 };
    }

    private static createGetApplicationsUrl(
        paginationOptions: PaginationOptions | null = null,
        searchByOrganizationId: boolean
    ): string {
        let url = this.urlBase;

        const queryParams = new Map<string, string>();
        if (paginationOptions != null) {
            queryParams.set('pageNumber', paginationOptions.page.toString());
            queryParams.set('pageSize', paginationOptions.itemsPerPage.toString());
        }

        const { organizationId } = useAccountStore();
        if (organizationId != null) {
            queryParams.set('organizationId', encodeURIComponent(organizationId));
        }

        let queryParamCharacter = '?';
        for (const [key, value] of queryParams) {
            const encodedValue = encodeURIComponent(value);
            url += `${queryParamCharacter}${key}=${encodedValue}`;

            queryParamCharacter = '&';
        }

        return url;
    }
}
