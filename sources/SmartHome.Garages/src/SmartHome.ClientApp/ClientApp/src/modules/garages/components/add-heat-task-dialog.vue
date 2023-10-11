<template>
  <v-dialog v-model="show" fullscreen>
    <template #activator="{ props }">
      <v-btn v-bind="props" variant="text" icon="mdi-plus" />
    </template>
    <v-card>
      <v-card-item class="d-flex justify-end"
        ><v-btn icon="mdi-close" variant="text" @click="show = false"
      /></v-card-item>
      <v-card-item>
        <v-text-field v-model="inputTime" @picked="handlePickedDays" />
      </v-card-item>
      <v-card-actions>
        <day-in-week-picker />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import DayInWeekPicker from './day-in-week-picker.vue'

const show = ref(false)
const inputTime = ref('')
const pickedDays = ref<number[]>([])

const handlePickedDays = (data: any) => {}
</script>
