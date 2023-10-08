<template>
  <v-container class="heat-task-container">
    <v-row dense>
      <v-col align-self="center">
        <div class="d-flex align-center time-font-size pl-4">{{ heatTask.time.slice(0, 5) }}</div>
      </v-col>
      <v-col cols="2" align-self="center">
        <div class="d-flex justify-end">
          <div v-for="(day, i) in daysInWeek" :key="i" :class="dayClass(i)">
            {{ day.slice(0, 1).toUpperCase() }}
          </div>
        </div>
      </v-col>
      <v-col cols="2"><v-switch :disabled="isDisabled" class="d-flex align-start justify-end" /></v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import type { CyclicHeatTaskDto } from '@/modules/core/services/api/api.models'
import type { PropType } from 'vue'

const daysInWeek = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday']

const props = defineProps({
  heatTask: { type: Object as PropType<CyclicHeatTaskDto>, required: true },
    isDisabled: {type: Boolean, required: true}
})

const dayClass = (i: number) => {
  console.log(props.heatTask.daysInWeekSelected)
  if (props.isDisabled) return 'disabled-color pr-1';
  return props.heatTask.daysInWeekSelected.includes(i) ? 'day-character--blue pr-1' : 'pr-1'
}
</script>
<style lang="scss">
.heat-task-container {
  border: 1px solid #0288d1;
  border-radius: 20px;
  background-color: #f4f4f5;
  opacity: 1;
}

.day-character--blue {
  color: #0288d1;
}

.time-font-size {
  font-size: 24px;
}

.disabled-color {
  color: #5a5a5a;
}
</style>
