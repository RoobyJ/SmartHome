<template>
  <div>
    <div>{{ garageName }}</div>
    <div>Heater status: <v-icon :color="getHeaterStatusColor">mdi-circle</v-icon></div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
  garageName: {
    type: String,
    required: true
  },
  heaterStatus: {
    type: Boolean,
    required: true
  }
})

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