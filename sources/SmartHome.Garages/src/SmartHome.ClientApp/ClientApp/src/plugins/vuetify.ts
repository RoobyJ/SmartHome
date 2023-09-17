import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';

import { aliases, dmIcons } from '@/modules/core/icons/core.icons';
import DateFnsAdapter from '@date-io/date-fns';
import { enUS as dateFnsEn, pl as dateFnsPl } from 'date-fns/locale';
import { createVuetify, type VuetifyOptions } from 'vuetify';
import * as components from 'vuetify/components';
import { VBtn, VLabel } from 'vuetify/components';
import * as directives from 'vuetify/directives';
import { mdi, aliases as vuetifyAliases } from 'vuetify/iconsets/mdi';
import * as labsComponents from 'vuetify/labs/components';
import { en as vuetifyEn, pl as vuetifyPl } from 'vuetify/locale';
import { getCurrentLocaleInVuetifyFormat } from './i18n';

const primaryDarkerTextColor = '#001a31';
const primaryLighterTextColor = '#002343';

const config: VuetifyOptions = {
    locale: {
        // Use adapter only when we want to provide custom translations for vuetify components
        // adapter: createVueI18nAdapter({ i18n, useI18n }),
        locale: getCurrentLocaleInVuetifyFormat(),
        messages: {
            pl: vuetifyPl,
            en: vuetifyEn,
        },
    },
    date: {
        adapter: DateFnsAdapter,
        locale: {
            en: dateFnsEn,
            pl: dateFnsPl,
        },
    },
    aliases: {
        VBtnPrimary: VBtn,
        VBtnSecondary: VBtn,
        VFormLabel: VLabel,
    },
    icons: {
        defaultSet: 'mdi',
        aliases: {
            ...vuetifyAliases,
            ...aliases,
        },
        sets: {
            mdi,
            dm: dmIcons,
        },
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
                    ...createColors('menu-icon', '#f2f3f4', primaryDarkerTextColor),
                    ...createColors('active-menu-icon', '#1b93d5', '#ffffff'),
                    ...createColors('menu-item', '#ffffff', primaryDarkerTextColor),
                    ...createColors('active-menu-item', '#0288d1', '#ffffff'),
                    ...createColors('group-item', '#f9f9fa', primaryDarkerTextColor),
                    ...createColors('dialog', '#ffffff', primaryDarkerTextColor),
                    ...createColors('btn-secondary', '#ffffff', primaryLighterTextColor),
                    ...createColors('error-dialog-details', '#F5F6F7', primaryLighterTextColor),
                },
            },
        },
    },
    defaults: {
        VBtn: {
            style: [{ textTransform: 'none' }],
        },
        VBtnPrimary: {
            style: [{ textTransform: 'none', fontWeight: 400 }],
            elevation: 0,
            color: 'primary',
            class: 'v-btn-primary',
        },
        VBtnSecondary: {
            style: [{ textTransform: 'none' }],
            elevation: 0,
            variant: 'outlined',
            class: 'bg-btn-secondary',
        },
        VFormLabel: {
            class: 'v-form-label',
        },
        VTextField: {
            variant: 'outlined',
            persistentHint: true,
            hideDetails: false,
            singleLine: true,
            bgColor: 'white',
            color: 'primary',
        },
        VList: {
            VCheckboxBtn: {
                density: 'default',
            },
        },
        VSelect: {
            variant: 'outlined',
            persistentHint: true,
            hideDetails: false,
            singleLine: true,
            bgColor: 'white',
            color: 'primary',
        },
        VAutocomplete: {
            variant: 'outlined',
            persistentHint: true,
            hideDetails: false,
            singleLine: true,
            bgColor: 'white',
            color: 'primary',
        },
        VCombobox: {
            variant: 'outlined',
            persistentHint: true,
            hideDetails: false,
            singleLine: true,
            bgColor: 'white',
            color: 'primary',
        },
        VCheckbox: {
            hideDetails: false,
            density: 'compact',
            persistentHint: true,
            color: 'primary',
            VLabel: {
                style: [
                    {
                        marginLeft: '1rem',
                        opacity: 1,
                        fontWeight: 500,
                    },
                ],
            },
        },
        VCheckboxBtn: {
            density: 'compact',
            color: 'primary',
        },
        VDialog: {
            maxWidth: 624,
            persistent: true,
            VCard: {
                color: 'dialog',
                class: 'v-card--in-dialog',
                elevation: 5,
                VCardActions: {
                    VBtnPrimary: {
                        minWidth: 100,
                    },
                    VBtnSecondary: {
                        minWidth: 100,
                    },
                },
            },
        },
    },
};

if (import.meta.env.DEV) {
    // Disable tree-shaking for DEV mode.
    config.components = { ...components, ...labsComponents };
    config.directives = { ...directives };
}

export default createVuetify(config);

function createColors(name: string, bgColor: string, textColor: string) {
    const result: any = {};
    result[name] = bgColor;
    result[`on-${name}`] = textColor;
    return result;
}
