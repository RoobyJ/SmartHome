<template>
  <v-app-bar class="top-bar" name="app-bar" color="app-bar" elevation="1" scroll-behavior="elevate">
    <v-app-bar-nav-icon class="ml-0" @click="$emit('toggleMainMenu')" />

    <v-app-bar-title class="d-flex justify-center">
      {{ route.name }}
    </v-app-bar-title>

    <template #append>
      <v-menu>
        <template #activator="{ props }">
          <v-btn class="mr-md-2" icon v-bind="props">
            <v-avatar color="primary" size="small">
              <v-icon>mdi-account</v-icon>
            </v-avatar>
          </v-btn>
        </template>

        <v-card min-width="300">
          <v-card-text>
            <div class="mx-auto text-center"></div>
          </v-card-text>
        </v-card>
      </v-menu>
    </template>
  </v-app-bar>
</template>

<script lang="ts" setup>
import { useRoute } from 'vue-router'

defineEmits(['toggleMainMenu'])

const route = useRoute()
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
