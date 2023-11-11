<template>
  <v-dialog v-model="show" max-width="95vw" width="99vw">
    <template #activator="{ props }">
      <v-btn v-bind="props" variant="text" icon="mdi-plus" />
    </template>
    <v-card class="add-task-container">
      <v-row class="mb-4">
        <v-col></v-col>
        <v-col class="d-flex justify-center mt-2">
          <div class="title">Add task</div>
        </v-col>
        <v-col class="d-flex justify-end"
          ><v-btn icon="mdi-close" variant="text" @click="show = false" />
        </v-col>
      </v-row>
      <div>
        <v-text-field
          v-model="inputTime"
          variant="outlined"
          bg-color="white"
          prepend-inner-icon="mdi-clock"
          placeholder="Time"
          hint="required format HH:MM"
          :rules="timeInputRules"
          class="time-input--margin"
        />
      </div>
      <v-row class="cyclic-day-picker">
        <v-col cols="6" class="d-flex justify-start align-center">
          {{ pickedDate?.toDateString() }}
        </v-col>
        <v-col cols="6" class="d-flex justify-end">
          <date-picker-dialog @chosen="handleDatePick" />
        </v-col>
        <v-col>
          <day-in-week-picker v-model="resetSelectedDays" @pick="handlePickedDays" />
        </v-col>
      </v-row>
      <v-card-actions class="d-flex justify-end mt-5">
        <v-btn
          variant="flat"
          color="#2488cf"
          :disabled="pickedDate == null || pickedDays == null"
          @click="saveHeatRequest()"
          >Add</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import DayInWeekPicker from './day-in-week-picker.vue'
import DatePickerDialog from '@/modules/shared/components/date-picker-dialog.vue'
import { GarageClient } from '@/modules/core/services/api-clients/garages-client'
import { useRoute } from 'vue-router'
import type { HeatRequestDto } from '@/modules/core/services/api/api.models'

const route = useRoute()

const show = ref(false)
const resetSelectedDays = ref(false)
const inputTime = ref('')
const pickedDays = ref<Map<number, boolean>>(new Map())
const pickedDate = ref<Date | null>(null)

const timeInputRules = [
  (val: string) => val.length === 5 || 'too short',
  (val: string) => val[2] === ':' || 'invalid format',
  (val: string) => val.length > 1 || 'must be filled'
]

const handlePickedDays = (data: Map<number, boolean>) => {
  pickedDate.value = null
  pickedDays.value = data
}
const handleDatePick = (date: Date) => {
  pickedDays.value = new Map()
  pickedDate.value = date
  resetSelectedDays.value = true
}

const saveHeatRequest = () => {
  const id = route.params.garageId

  if (id == null || typeof id !== 'string') return

  if (pickedDate.value != null) {
    pickedDate.value.setHours(
      parseInt(inputTime.value.slice(0, 2)),
      parseInt(inputTime.value.slice(3, 5)),
      0
    )
    const payload: HeatRequestDto = { date: pickedDate.value }
    GarageClient.saveCustomHeatRequest(id, payload)
  }
  show.value = false
}

onMounted(() => {
  pickedDate.value = new Date()
})
</script>

<style lang="scss">
.add-task-container {
  .time-input--margin {
    margin-left: 100px;
    margin-right: 100px;
  }

  .cyclic-day-picker {
    margin: 0 4px 0 4px;
    background-color: #e1e4e8;
    border: 1px solid #e1e4e8;
    border-radius: 25px;
    max-height: 200px;
  }
  .title {
    font-weight: bold;
    font-size: 20px;
  }
}
</style>
