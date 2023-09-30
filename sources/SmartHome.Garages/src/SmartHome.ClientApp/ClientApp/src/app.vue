<script setup lang="ts">
import { ref } from 'vue'
import { RouterView } from 'vue-router'
import TopBar from './modules/core/components/top-bar.vue'
import MainMenu from './modules/core/components/main-menu.vue'
import { useDate } from 'vuetify/labs/date'

const mainMenuVisible = ref(false)

const version = import.meta.env.VITE_APP_VERSION ?? 'unknown version'

</script>

<template>
  <v-app>
    <main-menu v-model="mainMenuVisible" />
    <top-bar @toggle-main-menu="mainMenuVisible = !mainMenuVisible" />

    <v-main>
        <RouterView />
    </v-main>

    <v-footer app elevation="5" color="drawer">
      <span class="text-truncate mr-2">
        SmartHome
      </span>
      <span class="text-no-wrap ml-auto">{{ version }}</span>
    </v-footer>
  </v-app>
</template>
