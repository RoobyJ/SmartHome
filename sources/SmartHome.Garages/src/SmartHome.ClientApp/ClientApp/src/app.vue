<script setup lang="ts">
import { useI18n } from 'vue-i18n';

import { useDate } from 'vuetify/labs/date';

import { storeToRefs } from 'pinia';
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';
import MainMenu from './modules/core/components/main-menu.vue';
import ApiErrorDialog from './modules/core/components/api-error-dialog.vue';
import TopBar from './modules/core/components/top-bar.vue';
import { useRouterStore } from './modules/core/store/router-store';
import PagePanelBase from './modules/shared/components/page-panel-base.vue';

const route = useRoute();
const { initializing } = storeToRefs(useRouterStore());

const useDashboardLayout = computed(() => {
    if (initializing.value) return false;
    return route.meta?.useDashboardLayout ?? false;
});

const version = import.meta.env.VITE_APP_VERSION ?? 'unknown version';

const { t } = useI18n();

const date = useDate();
const currentYear = date.getYear(new Date());

const mainMenuVisible = ref(false);
</script>

<template>
    <v-app theme="cmpl">
        <main-menu v-if="useDashboardLayout" v-model="mainMenuVisible" />

        <top-bar v-if="useDashboardLayout" @toggle-main-menu="mainMenuVisible = !mainMenuVisible" />

        <v-main>
            <page-panel-base v-if="initializing">
                <v-container>
                    <v-row dense>
                        <v-col cols="12" class="text-h4">{{ t('Core.Loading') }}...</v-col>
                    </v-row>
                </v-container>
            </page-panel-base>

            <router-view v-else v-slot="{ Component, route }">
                <component :is="Component" :key="route.path" />
            </router-view>

            <api-error-dialog />
        </v-main>

        <v-footer v-if="useDashboardLayout" app class="px-5 px-md-8" color="drawer">
            <span class="text-truncate mr-2">
                &copy; 2023 - {{ currentYear }} Jureczko Enterteiment. {{ t('Core.AllRightsReserved') }}.
            </span>
            <span class="text-no-wrap ml-auto">{{ version }}</span>
        </v-footer>
    </v-app>
</template>

<style lang="scss">
@use '@/styles/main.scss';
</style>
