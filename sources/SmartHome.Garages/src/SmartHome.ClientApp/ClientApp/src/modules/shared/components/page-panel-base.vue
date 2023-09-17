<template>
    <div class="background-layout">
        <div class="background-photo">
            <img src="@/assets/backgrounds/photo.svg" alt="bg-photo" />
        </div>
        <div class="left-overlay">
            <img src="@/assets/backgrounds/left-overlay.svg" alt="bg-left-overlay" />
        </div>
        <div class="right-overlay">
            <img src="@/assets/backgrounds/right-overlay.svg" alt="bg-right-overlay" />
        </div>
        <div class="cmpl-logo d-md-flex">
            <img src="@/assets/cmpl-logo.svg" alt="cmpl-logo" />
        </div>
    </div>

    <div class="v-main-card-container d-flex justify-center">
        <v-card id="main-card" class="v-main-card rounded-0" color="white" flat :loading="initializing">
            <template #loader="{ isActive }">
                <v-progress-linear :active="isActive" color="primary" height="4" indeterminate />
            </template>

            <div class="cmpl-logo">
                <img src="@/assets/cmpl-logo.svg" alt="cmpl-logo" />
            </div>

            <slot />
        </v-card>
    </div>
</template>

<script setup lang="ts">
import { useRouterStore } from '@/modules/core/store/router-store';
import { storeToRefs } from 'pinia';

const { initializing } = storeToRefs(useRouterStore());
</script>

<style lang="scss" scoped>
.background-layout {
    position: fixed;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;

    img {
        height: 100%;
    }

    .background-photo {
        position: fixed;
        height: 100%;
        top: 0;
        left: 0;
        width: 20%;
    }

    .left-overlay {
        position: fixed;
        height: 100%;
        top: 0;
        left: 0;
        width: 23%;
    }

    .right-overlay {
        position: fixed;
        height: 100%;
        top: 0;
        right: 0;
        width: 43%;

        img {
            width: 100%;
        }
    }

    .cmpl-logo {
        display: none;
        position: fixed;
        top: 150px;
        right: 0;
        width: 18%;
        justify-content: center;

        img {
            height: 80px;
        }
    }
}
</style>
