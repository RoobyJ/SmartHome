import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';

import i18n from '@/plugins/i18n';
import { useI18n } from 'vue-i18n';
import { createVuetify, type VuetifyOptions } from 'vuetify';
import * as components from 'vuetify/components';
import { VBtn } from 'vuetify/components';
import * as directives from 'vuetify/directives';
import * as labsComponents from 'vuetify/labs/components';
import { createVueI18nAdapter } from 'vuetify/locale/adapters/vue-i18n';

const primaryDarkerTextColor = '#001a31';
const primaryLighterTextColor = '#002343';

const config: VuetifyOptions = {
    locale: {
        adapter: createVueI18nAdapter({ i18n, useI18n }),
    },
    aliases: {
        VBtnPrimary: VBtn,
        VBtnSecondary: VBtn,
    },
    display: {
        mobileBreakpoint: 'md',
    },
    theme: {
        defaultTheme: 'cmpl',
        themes: {
            cmpl: {
                dark: false,
                colors: {
                    'primary-darker-text': primaryDarkerTextColor,
                    'primary-lighter-text': primaryLighterTextColor,
                    surface: '#ffffff',
                    primary: '#2488cf',

                    // these colors are accessible using class "bg-{name}", for example: "bg-readonly"
                    ...createColors('background', '#f4f4f5', primaryDarkerTextColor),
                    ...createColors('app-bar', '#fafafa', primaryLighterTextColor),
                    ...createColors('drawer', '#f9f9fa', primaryLighterTextColor),
                    ...createColors('readonly', '#98a6b3', primaryDarkerTextColor),
                    ...createColors('readonly-light', '#e1e4e8', primaryDarkerTextColor),
                },
            },
        },
    },
    defaults: {
        VBtn: {
            style: [{ textTransform: 'none' }],
        },
        VBtnPrimary: {
            style: [{ textTransform: 'none' }],
            elevation: 0,
            color: 'primary',
            class: 'v-btn-primary',
        },
        VBtnSecondary: {
            style: [{ textTransform: 'none' }],
            elevation: 0,
            variant: 'outlined',
            class: 'v-btn-secondary',
        },
        VTextField: {
            variant: 'outlined',
            density: 'compact',
            hideDetails: false,
            bgColor: 'white',
        },
        VCheckbox: {
            hideDetails: true,
            VLabel: {
                class: 'v-checkbox-label',
            },
        },
    },
};

if (import.meta.env.DEV) {
    // Disable tree-shaking for DEV mode.
    config.components = { components, labsComponents };
    config.directives = { directives };
}

export default createVuetify(config);

function createColors(name: string, bgColor: string, textColor: string) {
    const result: any = {};
    result[name] = bgColor;
    result[`on-${name}`] = textColor;
    return result;
}
