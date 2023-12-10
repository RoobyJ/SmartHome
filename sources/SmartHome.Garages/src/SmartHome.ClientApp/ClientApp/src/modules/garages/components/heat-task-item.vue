<template>
  <v-container class="heat-task-container">
    <v-row dense>
      <v-col cols="1" align-self="center"
        ><v-checkbox
          density="compact"
          class="pt-5"
          color="#2488cf"
          @update:modelValue="(val) => clickCheckbox(val)"
        />
      </v-col>
      <v-col align-self="center">
        <div class="d-flex align-center time-font-size pl-4">{{ getTime }}</div>
      </v-col>
      <v-col cols="3" align-self="center">
        <div v-if="isCyclic" class="d-flex justify-end">
          <div v-for="(day, i) in daysInWeek" :key="i" :class="dayClass(i)">
            {{ day.slice(0, 1).toUpperCase() }}
          </div>
        </div>
        <div v-else class="date">
          {{ getDateString }}
        </div>
      </v-col>
      <v-col cols="2">
        <v-switch hide-details />
        <heat-task-form-dialog
          :selected-time="getTime"
          :selected-day="getDate"
          :selected-days="getSelectedDays"
          :selected-item-id="heatTask.id"
          is-edit
          icon="mdi-lead-pencil"
          @created="emit('updated')"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import type { CustomHeatTaskDto, CyclicHeatTaskDto } from '@/modules/core/services/api/api.models'
import HeatTaskFormDialog from './heat-task-form-dialog.vue';
import { computed, type PropType } from 'vue'

const daysInWeek = ['sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday']

const emit = defineEmits(['clicked-checkbox', 'updated'])

const props = defineProps({
  heatTask: { type: Object as PropType<CustomHeatTaskDto | CyclicHeatTaskDto>, required: true }
})

const isCyclic = computed(() => 'time' in props.heatTask)

const getDate = computed(() => {
  if ('date' in props.heatTask) return new Date(props.heatTask.date.toString().slice(0, 10))
})

const getDateString = computed(() => {
  if ('date' in props.heatTask) return props.heatTask.date.toString().slice(0, 10)
})

const getSelectedDays = computed(() => {
  if ('daysInWeekSelected' in props.heatTask) {
    let cyclicHeatDays = new Map<number, boolean>();
    for (let i =0; i < 7; i++) {
      cyclicHeatDays.set(i, props.heatTask.daysInWeekSelected.includes(i));
    }

    return cyclicHeatDays;
  }
})

const getTime = computed(() => {
  if ('time' in props.heatTask) return props.heatTask.time.slice(0, 5)
  if ('date' in props.heatTask) return props.heatTask.date.toString().slice(11, 16)
})

const dayClass = (i: number) => {
  if ('date' in props.heatTask) return 'pr-1'
  return props.heatTask.daysInWeekSelected.includes(i) ? 'day-character--blue pr-1' : 'pr-1'
}

const clickCheckbox = (val: boolean) => {
  emit('clicked-checkbox', val, props.heatTask.id)
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

.date {
  color: #2488cf;
}
</style>
