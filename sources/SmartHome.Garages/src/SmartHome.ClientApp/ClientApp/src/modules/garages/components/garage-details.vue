<template>
  <div>
    <div>{{ garageName }}</div>
    <div>Heater status: <v-icon :color="getHeaterStatusColor">mdi-circle</v-icon></div>
    <div>Current temperature: {{ currentTemperature }}</div>
  </div>
</template>

<script setup lang="ts">
import { computed, type PropType } from 'vue';

const props = defineProps({
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

const currentTemperature = computed(() => props.temperature?.toString() + ' °C');

const getHeaterStatusColor = computed(() => {
  switch (props.heaterStatus){
    case true:
      return 'green';
    case false:
      return 'red';
    default:
      return 'grey';
  }
});
</script>