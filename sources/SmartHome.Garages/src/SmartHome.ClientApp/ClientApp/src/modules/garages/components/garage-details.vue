<template>
  <div>
    <div>{{ garageName }}</div>
    <div>Heater status: <v-icon :color="heaterStatus ? 'green' : 'red'">mdi-circle</v-icon></div>
  </div>
</template>

<script setup lang="ts">
defineProps({
  garageName: {
    type: String,
    required: true
  },
  heaterStatus: {
    type: Boolean,
    required: true
  }
})
</script>