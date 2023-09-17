import { useAccountStore } from '../../store/account-store';
import { useErrorStore } from '../../store/error-store';
import type { InviteUserDto, PaginatedListResultDto, PaginationOptions, UserDto } from '../api/api.models';
import { createApiResponse, type ApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';

export class UsersClient {
    private static readonly urlBase = '/api/mgmt/users';

    public static async inviteUser(payload: InviteUserDto): Promise<ApiResponse<any>> {
        const url = `${this.urlBase}/invite`;
        const request = httpClient.post(url, { json: payload }).json();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error, { displayErrorDialog: false });
        return apiResponse;
    }

    public static async getUsers(
        paginationOptions: PaginationOptions,
        includeOrganizations = false,
        filterByUsersOrganizationId = false
    ): Promise<PaginatedListResultDto<UserDto>> {
        const url = this.createGetUsersUrl(paginationOptions, includeOrganizations, filterByUsersOrganizationId);
        const request = httpClient.get(url).json<PaginatedListResultDto<UserDto>>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse.data;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return { items: [], hasNextPage: false, hasPreviousPage: false, pageNumber: 1, totalCount: 0, totalPages: 1 };
    }

    private static createGetUsersUrl(
        paginationOptions: PaginationOptions | null = null,
        includeOrganizations = false,
        filterByUsersOrganizationId: boolean
    ): string {
        let url = this.urlBase;

        const queryParams = new Map<string, string>();
        if (paginationOptions != null) {
            queryParams.set('pageNumber', paginationOptions.page.toString());
            queryParams.set('pageSize', paginationOptions.itemsPerPage.toString());
        }

        if (includeOrganizations) {
            queryParams.set('includeOrganizations', 'true');
        }

        if (filterByUsersOrganizationId) {
            const { organizationId } = useAccountStore();
            if (organizationId != null) {
                queryParams.set('organizationId', encodeURIComponent(organizationId));
            }
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
