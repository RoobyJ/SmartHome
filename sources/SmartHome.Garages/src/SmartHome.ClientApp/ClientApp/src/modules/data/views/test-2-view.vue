<template>
    <v-container fluid>
        <v-row dense>
            <v-col cols="12">User: {{ name }}</v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">Locale: {{ i18n.locale }}</v-col>
            <v-col cols="12">
                <v-btn-primary @click="toggleLang">Toggle language</v-btn-primary>
            </v-col>
            Hello World
        </v-row>
    </v-container>
</template>
<script setup lang="ts">
import { useAccountStore } from '@/modules/core/store/account-store';
import { LocaleCode, setI18nLanguage } from '@/plugins/i18n';
import { storeToRefs } from 'pinia';
import { useI18n } from 'vue-i18n';

const { name } = storeToRefs(useAccountStore());

const i18n = useI18n();
const toggleLang = async () => {
    let newLocale = LocaleCode.plPL;
    if (i18n.locale.value === LocaleCode.plPL) newLocale = LocaleCode.enUs;

    setI18nLanguage(newLocale);
};
</script>
