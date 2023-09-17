import { useErrorStore } from '../../store/error-store';
import type { CustomerDto, PaginatedListResultDto, PaginationOptions } from '../api/api.models';
import { createApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';

export class CustomersClient {
    private static readonly urlBase = '/api/mgmt/customers';
    /**
     * gets customers with pagination
     */
    public static async getCustomers(
        paginationOptions: PaginationOptions | null
    ): Promise<PaginatedListResultDto<CustomerDto>> {
        const url = this.createGetCustomersUrl(paginationOptions);
        const request = httpClient.get(url).json<PaginatedListResultDto<CustomerDto>>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse.data;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return { items: [], hasNextPage: false, hasPreviousPage: false, pageNumber: 1, totalCount: 0, totalPages: 1 };
    }

    private static createGetCustomersUrl(paginationOptions: PaginationOptions | null = null): string {
        let url = this.urlBase;

        const queryParams = new Map<string, string>();
        if (paginationOptions != null) {
            queryParams.set('pageNumber', paginationOptions.page.toString());
            queryParams.set('pageSize', paginationOptions.itemsPerPage.toString());
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
