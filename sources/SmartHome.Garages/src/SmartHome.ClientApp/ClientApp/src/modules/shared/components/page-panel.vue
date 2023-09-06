<template>
    <page-panel-base>
        <router-view v-slot="{ Component, route }">
            <component :is="Component" :key="route.path" />
        </router-view>
    </page-panel-base>
</template>

<script setup lang="ts">
import PagePanelBase from './page-panel-base.vue';
</script>
