<template>
    <v-container fluid>
        <v-container fluid class="pb-2">
            <v-row dense>
                <v-col cols="12" class="text-h4 dashboard-title">
                    <v-avatar v-if="icon != null" size="40" rounded color="readonly-light">
                        <v-icon size="20" :icon="icon" />
                    </v-avatar>
                    {{ t(title) }}
                </v-col>
            </v-row>
        </v-container>

        <router-view v-slot="{ Component, route }">
            <component :is="Component" :key="route.path" />
        </router-view>
    </v-container>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute } from 'vue-router';

const route = useRoute();
const { t } = useI18n();

const title = computed(() => route.meta.title);
const icon = computed(() => route.meta.icon);
</script>

<style lang="scss" scoped>
.dashboard-title {
    // important is required to override text-h4
    font-size: 32px !important;
}
</style>
