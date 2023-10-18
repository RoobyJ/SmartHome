<template>
  <div>
    <div>{{ garageName }}</div>
    <div>Heater status: <v-icon :color="heaterStatus ? 'green' : 'red'">mdi-circle</v-icon></div>
    <div>Current temperature: {{ temperature }}</div>
  </div>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';

defineProps({
  garageName: {
    type: String,
    required: true
  },
  heaterStatus: {
    type: Boolean as PropType<Boolean | null>,
    required: true
  },
  temperature: {
    type: Number as PropType<Number | null>,
    required: true
  }
})
</script>