import { createI18n, type I18n } from 'vue-i18n';

// the bundler is generating this messages based on the json files (configured in vite.config.js)
import messages from '@intlify/unplugin-vue-i18n/messages';

export enum LocaleCode {
    enUs = 'en-US',
    plPL = 'pl-PL',
}

const ALLOWED_LANGS = [LocaleCode.enUs, LocaleCode.plPL];

// it should never be null after initializing the app instance
// we are using the full expanded type, so we can specify `false` on the `legacy` generic argument (it simplifies typings)
let i18n: I18n<
    Record<string, unknown>,
    Record<string, unknown>,
    Record<string, unknown>,
    string,
    false
> = null as unknown as I18n;

const I18N_SELECTED_LANG_KEY = 'i18n_locale';
const I18N_DEFAULT_LANG = LocaleCode.plPL;
const I18N_FALLBACK_LANG = LocaleCode.enUs;

function setupI18n(): I18n<Record<string, unknown>, Record<string, unknown>, Record<string, unknown>, string, false> {
    if (i18n != null) return i18n;

    let selectedLang = localStorage.getItem(I18N_SELECTED_LANG_KEY) as LocaleCode;
    if (!ALLOWED_LANGS.includes(selectedLang)) selectedLang = I18N_DEFAULT_LANG;

    i18n = createI18n({
        legacy: false,
        locale: selectedLang,
        fallbackLocale: I18N_FALLBACK_LANG,
        messages,
    });

    setI18nLanguage(selectedLang);
    return i18n;
}

export function setI18nLanguage(locale: string) {
    // We are not using the legacy mode, so i18n.global.locale is a Ref, not string
    i18n.global.locale.value = locale;

    localStorage.setItem(I18N_SELECTED_LANG_KEY, locale);

    // update the lang attribute
    window?.document?.querySelector('html')?.setAttribute('lang', locale);
}

export default setupI18n();
