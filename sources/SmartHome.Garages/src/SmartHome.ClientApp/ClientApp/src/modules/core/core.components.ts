/* eslint-disable vue/component-definition-name-casing */
import type { App } from 'vue';
import ApiErrorAlert from './components/api-error-alert.vue';
import LookupInput from './components/lookup-input.vue';

export function initializeComponents(app: App<Element>) {
    app.component('c-api-error-alert', ApiErrorAlert);
    app.component('c-lookup-input', LookupInput);
}
