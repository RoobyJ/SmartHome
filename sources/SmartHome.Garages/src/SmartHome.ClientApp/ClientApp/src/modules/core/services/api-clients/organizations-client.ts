import { StringUtils } from '@cmpl/core';
import { useAccountStore } from '../../store/account-store';
import { useErrorStore } from '../../store/error-store';
import type { CreateOrganizationDto, ItemsResultDto, ScopeBaseDto } from '../api/api.models';
import { createApiResponse, type ApiResponse } from '../api/api.utils';
import { httpClient } from '../api/http-client';

export class OrganizationsClient {
    private static readonly urlBase = '/api/mgmt/orgs';

    /**
     * Load scopes available for the current organization.
     */
    public static async getScopes(): Promise<ItemsResultDto<ScopeBaseDto>> {
        const url = this.createUrl('scopes', true);
        const request = httpClient.get(url).json<ItemsResultDto<ScopeBaseDto>>();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse.data;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return { items: [] };
    }

    public static async createOrganization(payload: CreateOrganizationDto): Promise<ApiResponse<any>> {
        const url = this.createUrl();
        const request = httpClient.post(url, { json: payload });
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return apiResponse;

        const { processError } = useErrorStore();
        await processError(apiResponse.error, { displayErrorDialog: false });
        return apiResponse;
    }

    private static createUrl(path: string = '', includeOrganizationId = false): string {
        let url = this.urlBase;

        if (includeOrganizationId) {
            const { organizationId } = useAccountStore();
            url += `/${organizationId ?? -1}`;
        }

        return StringUtils.isEmpty(path) ? url : `${url}/${path}`;
    }
}
