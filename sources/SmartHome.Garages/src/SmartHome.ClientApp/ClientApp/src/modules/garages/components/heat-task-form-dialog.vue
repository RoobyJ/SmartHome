<template>
  <v-dialog v-model="show" max-width="95vw" width="99vw">
    <template #activator="{ props }">
      <v-btn v-bind="props" variant="text" :icon="icon" />
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
          <day-in-week-picker
            v-model="resetSelectedDays"
            :picked-days="selectedDays"
            :is-edit="isEdit"
            @pick="handlePickedDays"
          />
        </v-col>
      </v-row>
      <v-card-actions class="d-flex justify-end mt-5">
        <v-btn
          variant="flat"
          color="#2488cf"
          :disabled="inputTime.length < 1 || isPickedAnyDayOrDate"
          @click="saveHeatRequest()"
          >{{ isEdit ? 'Edit' : 'Add' }}</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { onMounted, ref, type PropType, computed } from 'vue'
import DayInWeekPicker from './day-in-week-picker.vue'
import DatePickerDialog from '@/modules/shared/components/date-picker-dialog.vue'
import { GarageClient } from '@/modules/core/services/api-clients/garages-client'
import { useRoute } from 'vue-router'
import type {
  CreateCyclicHeatTaskDto,
  CustomHeatTaskDto,
  CyclicHeatTaskDto,
  HeatRequestDto
} from '@/modules/core/services/api/api.models'

const route = useRoute()

const props = defineProps({
  selectedTime: { type: String, default: '' },
  selectedDay: { type: Object as PropType<Date | null>, default: null },
  selectedDays: { type: Map<number, boolean>, default: new Map() },
  selectedItemId: { type: Number, default: -1 },
  icon: { type: String, default: 'mdi-square' },
  isEdit: { type: Boolean, default: false }
})

const show = ref(false)
const resetSelectedDays = ref(false)
const inputTime = ref(props.selectedTime)
const pickedDays = ref<Map<number, boolean>>(props.selectedDays)
const pickedDate = ref<Date | null>(props.selectedDay)

const emit = defineEmits(['created'])

const timeInputRules = [
  (val: string) => val.length === 5 || 'too short',
  (val: string) => val[2] === ':' || 'invalid format',
  (val: string) => val.length > 1 || 'must be filled'
]

const isPickedAnyDayOrDate = computed(() => {
  let block = true

  pickedDays.value.forEach((day) => {
    if (day === true) block = false
  })
  if (pickedDate.value != null) return false;

  return block
})

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
      parseInt(inputTime.value.slice(0, 2)) + (-(new Date().getTimezoneOffset() / 60)) ,
      parseInt(inputTime.value.slice(3, 5)),
      0
    )
    if (props.isEdit) {
      const payload: CustomHeatTaskDto = { id: props.selectedItemId, date: pickedDate.value }
      GarageClient.editCustomHeatRequest(id, payload)
    } else {
      const payload: HeatRequestDto = { date: pickedDate.value }
      GarageClient.saveCustomHeatRequest(id, payload)
    }
  }

  if (pickedDays.value.size > 0) {
    const selectedDays = selectedDaysToList()

    if (props.isEdit) {
      const payload: CyclicHeatTaskDto = {
        id: props.selectedItemId,
        time: inputTime.value + ':00',
        garageId: Number(id),
        daysInWeekSelected: selectedDays
      }
      GarageClient.editCyclicHeatRequest(id, payload)
    } else {
      const payload: CreateCyclicHeatTaskDto = {
        time: inputTime.value + ':00',
        daysInWeekSelected: selectedDays
      }
      GarageClient.saveCyclicHeatRequest(id, payload)
    }
  }
  show.value = false
  emit('created')
}

onMounted(() => {
  if (!props.isEdit) pickedDate.value = new Date()
})

function selectedDaysToList(): number[] {
  const selectedDays: number[] = []
  for (let i = 0; i < pickedDays.value.size; i++) {
    if (pickedDays.value.get(i) === true) selectedDays.push(i)
  }

  return selectedDays
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
