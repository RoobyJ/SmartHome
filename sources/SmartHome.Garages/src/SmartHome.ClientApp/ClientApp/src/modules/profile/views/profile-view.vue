<template>
    <v-container class="mt-2" fluid>
        <v-row>
            <v-col xl="5" lg="6" cols="12">
                <v-row>
                    <v-col>
                        <profile-user-details />
                    </v-col>
                </v-row>
            </v-col>
        </v-row>
    </v-container>
    <v-container fluid>
        <v-row dense>
            <v-col cols="12">User: {{ name }}</v-col>
            <v-col cols="12">Organization: {{ organizationName }}</v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">Locale: {{ i18n.locale }}</v-col>
            <v-col cols="12">
                <v-btn-primary @click="toggleLang">Toggle language</v-btn-primary>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import { useAccountStore } from '@/modules/core/store/account-store';
import { LocaleCode, getCurrentLocaleInVuetifyFormat, setI18nLanguage } from '@/plugins/i18n';
import { storeToRefs } from 'pinia';
import { useI18n } from 'vue-i18n';
import { useLocale } from 'vuetify';
import ProfileUserDetails from '@/modules/profile/components/profile-user-details.vue';

const { name, organizationName } = storeToRefs(useAccountStore());

const i18n = useI18n();

const { current: currentVuetifyLocale } = useLocale();

const toggleLang = async () => {
    let newLocale = LocaleCode.plPL;
    if (i18n.locale.value === LocaleCode.plPL) newLocale = LocaleCode.enUs;

    setI18nLanguage(newLocale);
    currentVuetifyLocale.value = getCurrentLocaleInVuetifyFormat();
};
</script>
