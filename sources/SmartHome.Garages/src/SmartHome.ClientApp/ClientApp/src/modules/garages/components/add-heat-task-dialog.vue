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
          class="time-input--margin"
          @picked="handlePickedDays"
        />
      </div>
      <v-row class="cyclic-day-picker">
        <v-col cols="12" class="d-flex justify-end"
          ><date-picker-dialog @chosen="handleDatePick" />
        </v-col>
        <v-col>
          <day-in-week-picker :disabled="pickedDate != null" />
        </v-col>
      </v-row>
      <v-card-actions class="d-flex justify-end mt-5">
        <v-btn variant="flat" color="#2488cf">Add</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import DayInWeekPicker from './day-in-week-picker.vue'
import DatePickerDialog from '@/modules/shared/components/date-picker-dialog.vue'

const show = ref(false)
const inputTime = ref('')
const pickedDays = ref<number[]>([])
const pickedDate = ref<Date | null>(null)

const handlePickedDays = (data: number[]) => {}
const handleDatePick = (date: Date) => {
  console.log(date);
  pickedDate.value = date
}
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
