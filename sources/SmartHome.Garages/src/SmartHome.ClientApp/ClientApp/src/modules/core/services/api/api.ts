/// <reference path="./custom.d.ts" />
// tslint:disable
/**
 * SmartHome.API
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0
 * 
 *
 * NOTE: This file is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the file manually.
 */

import * as url from "url";
import * as isomorphicFetch from "isomorphic-fetch";
import { Configuration } from "./configuration";

const BASE_PATH = "/".replace(/\/+$/, "");

/**
 *
 * @export
 */
export const COLLECTION_FORMATS = {
    csv: ",",
    ssv: " ",
    tsv: "\t",
    pipes: "|",
};

/**
 *
 * @export
 * @interface FetchAPI
 */
export interface FetchAPI {
    (url: string, init?: any): Promise<Response>;
}

/**
 *
 * @export
 * @interface FetchArgs
 */
export interface FetchArgs {
    url: string;
    options: any;
}

/**
 *
 * @export
 * @class BaseAPI
 */
export class BaseAPI {
    protected configuration: Configuration;

    constructor(configuration?: Configuration, protected basePath: string = BASE_PATH, protected fetch: FetchAPI = isomorphicFetch) {
        if (configuration) {
            this.configuration = configuration;
            this.basePath = configuration.basePath || this.basePath;
        }
    }
};

/**
 *
 * @export
 * @class RequiredError
 * @extends {Error}
 */
export class RequiredError extends Error {
    name: "RequiredError"
    constructor(public field: string, msg?: string) {
        super(msg);
    }
}

/**
 * 
 * @export
 * @interface CyclicHeatRequest
 */
export interface CyclicHeatRequest {
    /**
     * 
     * @type {number}
     * @memberof CyclicHeatRequest
     */
    garageId?: number;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    monday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    tuesday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    wednesday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    thursday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    friday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    saturday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequest
     */
    sunday?: TimeSpan;
    /**
     * 
     * @type {Garage}
     * @memberof CyclicHeatRequest
     */
    garage?: Garage;
    /**
     * 
     * @type {number}
     * @memberof CyclicHeatRequest
     */
    id?: number;
}
/**
 * 
 * @export
 * @interface CyclicHeatRequests
 */
export interface CyclicHeatRequests {
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    sunday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    monday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    tuesday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    wednesday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    thursday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    friday?: TimeSpan;
    /**
     * 
     * @type {TimeSpan}
     * @memberof CyclicHeatRequests
     */
    saturday?: TimeSpan;
}
/**
 * 
 * @export
 * @interface CyclicHeatRequestsDto
 */
export interface CyclicHeatRequestsDto {
    /**
     * 
     * @type {CyclicHeatRequests}
     * @memberof CyclicHeatRequestsDto
     */
    requests?: CyclicHeatRequests;
}
/**
 * 
 * @export
 * @interface Garage
 */
export interface Garage {
    /**
     * 
     * @type {string}
     * @memberof Garage
     */
    name?: string;
    /**
     * 
     * @type {string}
     * @memberof Garage
     */
    ip?: string;
    /**
     * 
     * @type {Array<CyclicHeatRequest>}
     * @memberof Garage
     */
    cyclicHeatRequests?: Array<CyclicHeatRequest>;
    /**
     * 
     * @type {Array<HeatRequest>}
     * @memberof Garage
     */
    heatRequests?: Array<HeatRequest>;
    /**
     * 
     * @type {Array<OutsideTemperature>}
     * @memberof Garage
     */
    outsideTemperatures?: Array<OutsideTemperature>;
    /**
     * 
     * @type {number}
     * @memberof Garage
     */
    id?: number;
}
/**
 * 
 * @export
 * @interface GarageCyclicHeatRequestsDto
 */
export interface GarageCyclicHeatRequestsDto {
    /**
     * 
     * @type {number}
     * @memberof GarageCyclicHeatRequestsDto
     */
    garageId?: number;
    /**
     * 
     * @type {CyclicHeatRequests}
     * @memberof GarageCyclicHeatRequestsDto
     */
    requests?: CyclicHeatRequests;
}
/**
 * 
 * @export
 * @interface GarageDetailsDto
 */
export interface GarageDetailsDto {
    /**
     * 
     * @type {number}
     * @memberof GarageDetailsDto
     */
    id?: number;
    /**
     * 
     * @type {string}
     * @memberof GarageDetailsDto
     */
    name?: string;
    /**
     * 
     * @type {boolean}
     * @memberof GarageDetailsDto
     */
    heaterStatus?: boolean;
}
/**
 * 
 * @export
 * @interface HeatRequest
 */
export interface HeatRequest {
    /**
     * 
     * @type {Date}
     * @memberof HeatRequest
     */
    heatRequest1?: Date;
    /**
     * 
     * @type {number}
     * @memberof HeatRequest
     */
    garageId?: number;
    /**
     * 
     * @type {Garage}
     * @memberof HeatRequest
     */
    garage?: Garage;
    /**
     * 
     * @type {number}
     * @memberof HeatRequest
     */
    id?: number;
}
/**
 * 
 * @export
 * @interface HeatRequestDto
 */
export interface HeatRequestDto {
    /**
     * 
     * @type {Date}
     * @memberof HeatRequestDto
     */
    date?: Date;
}
/**
 * 
 * @export
 * @interface OutsideTemperature
 */
export interface OutsideTemperature {
    /**
     * 
     * @type {Date}
     * @memberof OutsideTemperature
     */
    date?: Date;
    /**
     * 
     * @type {number}
     * @memberof OutsideTemperature
     */
    temperature?: number;
    /**
     * 
     * @type {number}
     * @memberof OutsideTemperature
     */
    garageId?: number;
    /**
     * 
     * @type {Garage}
     * @memberof OutsideTemperature
     */
    garage?: Garage;
    /**
     * 
     * @type {number}
     * @memberof OutsideTemperature
     */
    id?: number;
}
/**
 * 
 * @export
 * @interface TimeSpan
 */
export interface TimeSpan {
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    ticks?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    days?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    hours?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    milliseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    microseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    nanoseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    minutes?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    seconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalDays?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalHours?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalMilliseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalMicroseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalNanoseconds?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalMinutes?: number;
    /**
     * 
     * @type {number}
     * @memberof TimeSpan
     */
    totalSeconds?: number;
}
/**
 * GarageApi - fetch parameter creator
 * @export
 */
export const GarageApiFetchParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesGet(id: number, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdCyclicHeatTimesGet.');
            }
            const localVarPath = `/garage/{id}/CyclicHeatTimes`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'GET' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {CyclicHeatRequestsDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesSavePut(id: number, body?: CyclicHeatRequestsDto, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdCyclicHeatTimesSavePut.');
            }
            const localVarPath = `/garage/{id}/CyclicHeatTimes/save`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'PUT' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarHeaderParameter['Content-Type'] = 'application/json';

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);
            const needsSerialization = (<any>"CyclicHeatRequestsDto" !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.body =  needsSerialization ? JSON.stringify(body || {}) : (body || "");

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdGaragesGet(id: number, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdGaragesGet.');
            }
            const localVarPath = `/garage/{id}/garages`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'GET' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestGet(id: number, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdHeatTimeRequestGet.');
            }
            const localVarPath = `/garage/{id}/heatTimeRequest`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'GET' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {HeatRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestSavePost(id: number, body?: HeatRequestDto, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdHeatTimeRequestSavePost.');
            }
            const localVarPath = `/garage/{id}/heatTimeRequest/save`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'POST' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarHeaderParameter['Content-Type'] = 'application/json';

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);
            const needsSerialization = (<any>"HeatRequestDto" !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.body =  needsSerialization ? JSON.stringify(body || {}) : (body || "");

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeaterPatch(id: number, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdHeaterPatch.');
            }
            const localVarPath = `/garage/{id}/Heater`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'PATCH' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdTemperaturesGet(id: number, options: any = {}): FetchArgs {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling garageIdTemperaturesGet.');
            }
            const localVarPath = `/garage/{id}/Temperatures`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            const localVarUrlObj = url.parse(localVarPath, true);
            const localVarRequestOptions = Object.assign({ method: 'GET' }, options);
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarUrlObj.query = Object.assign({}, localVarUrlObj.query, localVarQueryParameter, options.query);
            // fix override query string Detail: https://stackoverflow.com/a/7517673/1077943
            delete localVarUrlObj.search;
            localVarRequestOptions.headers = Object.assign({}, localVarHeaderParameter, options.headers);

            return {
                url: url.format(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * GarageApi - functional programming interface
 * @export
 */
export const GarageApiFp = function(configuration?: Configuration) {
    return {
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesGet(id: number, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<GarageCyclicHeatRequestsDto> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdCyclicHeatTimesGet(id, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response.json();
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {CyclicHeatRequestsDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesSavePut(id: number, body?: CyclicHeatRequestsDto, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Response> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdCyclicHeatTimesSavePut(id, body, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response;
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdGaragesGet(id: number, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Array<GarageDetailsDto>> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdGaragesGet(id, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response.json();
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestGet(id: number, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Array<HeatRequestDto>> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdHeatTimeRequestGet(id, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response.json();
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {HeatRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestSavePost(id: number, body?: HeatRequestDto, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Response> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdHeatTimeRequestSavePost(id, body, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response;
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeaterPatch(id: number, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Response> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdHeaterPatch(id, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response;
                    } else {
                        throw response;
                    }
                });
            };
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdTemperaturesGet(id: number, options?: any): (fetch?: FetchAPI, basePath?: string) => Promise<Array<OutsideTemperature>> {
            const localVarFetchArgs = GarageApiFetchParamCreator(configuration).garageIdTemperaturesGet(id, options);
            return (fetch: FetchAPI = isomorphicFetch, basePath: string = BASE_PATH) => {
                return fetch(basePath + localVarFetchArgs.url, localVarFetchArgs.options).then((response) => {
                    if (response.status >= 200 && response.status < 300) {
                        return response.json();
                    } else {
                        throw response;
                    }
                });
            };
        },
    }
};

/**
 * GarageApi - factory interface
 * @export
 */
export const GarageApiFactory = function (configuration?: Configuration, fetch?: FetchAPI, basePath?: string) {
    return {
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesGet(id: number, options?: any) {
            return GarageApiFp(configuration).garageIdCyclicHeatTimesGet(id, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {CyclicHeatRequestsDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdCyclicHeatTimesSavePut(id: number, body?: CyclicHeatRequestsDto, options?: any) {
            return GarageApiFp(configuration).garageIdCyclicHeatTimesSavePut(id, body, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdGaragesGet(id: number, options?: any) {
            return GarageApiFp(configuration).garageIdGaragesGet(id, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestGet(id: number, options?: any) {
            return GarageApiFp(configuration).garageIdHeatTimeRequestGet(id, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {HeatRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeatTimeRequestSavePost(id: number, body?: HeatRequestDto, options?: any) {
            return GarageApiFp(configuration).garageIdHeatTimeRequestSavePost(id, body, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdHeaterPatch(id: number, options?: any) {
            return GarageApiFp(configuration).garageIdHeaterPatch(id, options)(fetch, basePath);
        },
        /**
         * 
         * @param {number} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        garageIdTemperaturesGet(id: number, options?: any) {
            return GarageApiFp(configuration).garageIdTemperaturesGet(id, options)(fetch, basePath);
        },
    };
};

/**
 * GarageApi - object-oriented interface
 * @export
 * @class GarageApi
 * @extends {BaseAPI}
 */
export class GarageApi extends BaseAPI {
    /**
     * 
     * @param {number} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdCyclicHeatTimesGet(id: number, options?: any) {
        return GarageApiFp(this.configuration).garageIdCyclicHeatTimesGet(id, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {CyclicHeatRequestsDto} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdCyclicHeatTimesSavePut(id: number, body?: CyclicHeatRequestsDto, options?: any) {
        return GarageApiFp(this.configuration).garageIdCyclicHeatTimesSavePut(id, body, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdGaragesGet(id: number, options?: any) {
        return GarageApiFp(this.configuration).garageIdGaragesGet(id, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdHeatTimeRequestGet(id: number, options?: any) {
        return GarageApiFp(this.configuration).garageIdHeatTimeRequestGet(id, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {HeatRequestDto} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdHeatTimeRequestSavePost(id: number, body?: HeatRequestDto, options?: any) {
        return GarageApiFp(this.configuration).garageIdHeatTimeRequestSavePost(id, body, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdHeaterPatch(id: number, options?: any) {
        return GarageApiFp(this.configuration).garageIdHeaterPatch(id, options)(this.fetch, this.basePath);
    }

    /**
     * 
     * @param {number} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof GarageApi
     */
    public garageIdTemperaturesGet(id: number, options?: any) {
        return GarageApiFp(this.configuration).garageIdTemperaturesGet(id, options)(this.fetch, this.basePath);
    }

}
