import type { UserClaimsDto } from '@/modules/core/services/api/api.models';
import { httpClient } from '@/modules/core/services/api/http-client';
import i18n from '@/plugins/i18n';
import { StringUtils } from '@cmpl/core';
import { createApiResponse } from '../api/api.utils';

export class AccountClient {
    private static readonly urlBase = '/account';

    /** This will navigate to the login page and later redirect back to SPA app */
    public static login() {
        window.location.href = this.getAccountRedirectUrl('login');
    }

    /** This will logout the user from this application and in the Identity Provider */
    public static logout() {
        window.location.href = this.getAccountRedirectUrl('end-session');
    }

    /** This will get the user details based on the user session */
    public static async getDetails(): Promise<UserClaimsDto | null> {
        const request = httpClient.get(`${this.urlBase}/me`).json<UserClaimsDto>();
        const apiResponse = await createApiResponse(request);

        if (!apiResponse.isSuccess) return null;

        // we need to process the details a little bit, so the keys matches the interface
        const anyClaims = apiResponse.data.claims as any;

        // by default claims are string values, so we cast it to number so it matches the interface
        anyClaims.sub = Number(anyClaims.sub);

        anyClaims.givenName = anyClaims.given_name;
        anyClaims.given_name = undefined;

        anyClaims.familyName = anyClaims.family_name;
        anyClaims.family_name = undefined;

        anyClaims.picture = StringUtils.isEmpty(anyClaims.picture) ? null : anyClaims.picture;

        return apiResponse.data;
    }

    private static getAccountRedirectUrl(path: string) {
        const url = location.origin + this.urlBase + `/${path}`;

        if (StringUtils.isEmpty(i18n.global.locale.value)) return url;
        return `${url}?ui_locales=${i18n.global.locale.value}`;
    }
}
