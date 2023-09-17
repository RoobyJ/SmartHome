<template>
    <v-app-bar class="top-bar" name="app-bar" color="app-bar" scroll-behavior="elevate">
        <v-app-bar-nav-icon v-if="mobile" class="ml-0" @click="$emit('toggleMainMenu')" />

        <v-app-bar-title>
            <!-- Using key, so the component is rerendered every time the page changes -->
            <v-breadcrumbs
                v-if="breadcrumbs != null"
                :key="'breadcrumb-' + String(route.name)"
                :items="breadcrumbs"
                class="pl-0 pl-md-3"
                divider=">"
            />
        </v-app-bar-title>

        <template #append>
            <v-menu v-if="isLoggedIn">
                <template #activator="{ props }">
                    <v-btn class="mr-md-2" icon v-bind="props">
                        <v-avatar color="primary" size="small">{{ initials }}</v-avatar>
                    </v-btn>
                </template>

                <v-card min-width="300">
                    <v-card-text>
                        <div class="mx-auto text-center">
                            <v-avatar color="primary" size="large">
                                <span class="text-h5">{{ initials }}</span>
                            </v-avatar>

                            <h3 class="pt-2">{{ name }}</h3>

                            <p class="text-caption mt-1">{{ email }}</p>

                            <v-divider class="my-3" />

                            <template v-if="isDev">
                                <v-btn variant="text" block :to="{ path: '/kitchen-sink' }">KITCHEN SINK</v-btn>
                            </template>

                            <v-btn variant="text" block @click="logout">{{ t('Core.LogOut') }}</v-btn>
                        </div>
                    </v-card-text>
                </v-card>
            </v-menu>
        </template>
    </v-app-bar>
</template>

<script lang="ts" setup>
import { useDev } from '@/modules/shared/composables/is-dev';
import { viewBreadcrumbDefinitions } from '@/plugins/router/view-breadcrumbs';
import type { View } from '@/plugins/router/view-definitions';
import { storeToRefs } from 'pinia';
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute } from 'vue-router';
import { useDisplay } from 'vuetify';
import { AccountClient } from '../services/api-clients/account-client';
import { useAccountStore } from '../store/account-store';

defineEmits(['toggleMainMenu']);

const { isDev } = useDev();

const { t } = useI18n();

const route = useRoute();

const { isLoggedIn, name, initials, email, organizationName } = storeToRefs(useAccountStore());

// it will get breadcrumbs from definitions and translate the title
const breadcrumbs = computed(() => {
    const view = route.name as View;

    const foundBreadcrumbs = viewBreadcrumbDefinitions[view];
    if (foundBreadcrumbs == null) return null;

    const resolvedBreadcrumbs =
        typeof foundBreadcrumbs === 'object' ? foundBreadcrumbs : foundBreadcrumbs(organizationName.value);

    // translate titles
    return resolvedBreadcrumbs.map(item => {
        if (typeof item === 'string') return t(item);

        const title =
            item.titleArguments != null && item.titleArguments.length > 0
                ? t(item.title, item.titleArguments)
                : t(item.title);

        return {
            ...item,
            title,
        };
    });
});

const { mobile } = useDisplay();

const logout = () => AccountClient.logout();
</script>

<style lang="scss">
@use 'vuetify/_settings';

.v-app-bar.top-bar {
    &.v-toolbar:not(.v-toolbar--flat) {
        box-shadow: 0px 3px 6px #6161b829;
    }

    .v-app-bar-title {
        font-size: 1rem;
        font-weight: 500;
    }

    .v-toolbar__append {
        margin-right: 14px;

        @media #{map-get(settings.$display-breakpoints, 'sm-and-down')} {
            margin-right: 6px;
        }
    }
}
</style>
