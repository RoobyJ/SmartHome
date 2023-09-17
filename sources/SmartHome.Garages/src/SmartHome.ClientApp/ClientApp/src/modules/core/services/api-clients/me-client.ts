import type { EditUserDetailsDto } from '@/modules/core/services/api/api.models';
import { httpClient } from '@/modules/core/services/api/http-client';
import { useErrorStore } from '../../store/error-store';
import { createApiResponse } from '../api/api.utils';

export class MeClient {
    /** Edits user's details. Returns true if the request succeeded. */
    public static async editUserDetails(payload: EditUserDetailsDto): Promise<boolean> {
        const url = `/api/mgmt/me`;
        const request = httpClient.put(url, { json: payload }).json();
        const apiResponse = await createApiResponse(request);

        if (apiResponse.isSuccess) return true;

        const { processError } = useErrorStore();
        await processError(apiResponse.error);
        return false;
    }
}
