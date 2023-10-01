<template>
  <v-container>
    <div>{{ garageName }}</div>
    <div>{{ heaterStatus }}<v-icon :color="heaterStatus ? 'green' : 'red'">mdi:circle</v-icon></div>
  </v-container>
</template>

<script setup lang="ts">

defineProps({
    garageName: {
        type: String,
        required: true,
    },
    heaterStatus: {
        type: Boolean,
        required: true,
    },
});
</script>
