import i18n from '@/plugins/i18n';
import { StringUtils } from '@cmpl/core';
import type { IdTokenClaims } from './api.models';
import { NotFoundError } from './error.models';
import { httpClient } from './http-client';

const urlBase = '/account';

const getAccountRedirectUrl = (path: string) => {
    const url = location.origin + urlBase + `/${path}`;

    if (StringUtils.isEmpty(i18n.global.locale.value)) return url;
    return `${url}?ui_locales=${i18n.global.locale.value}`;
};

export const accountClient = {
    /**
     * This will navigate to the login page and later redirect back to SPA app
     */
    login: async () => {
        window.location.href = getAccountRedirectUrl('login');
    },
    /**
     * This will logout the user from this application and in the Identity Provider
     */
    logout: async () => {
        window.location.href = getAccountRedirectUrl('end-session');
    },
    /**
     * This will get the user details based on the user session
     */
    getDetails: async () => {
        const claims = await httpClient
            .get(`${urlBase}/me`)
            .json<IdTokenClaims>()
            .catch(error => {
                if (error instanceof NotFoundError) return null;

                throw error;
            });

        if (claims == null) return null;

        // we need to process the details a little bit, so the keys matches the interface
        const anyClaims = claims as any;

        anyClaims.givenName = anyClaims.given_name;
        anyClaims.given_name = undefined;

        anyClaims.familyName = anyClaims.family_name;
        anyClaims.family_name = undefined;

        anyClaims.picture = StringUtils.isEmpty(anyClaims.picture) ? null : anyClaims.picture;

        return claims;
    },
} as const;
