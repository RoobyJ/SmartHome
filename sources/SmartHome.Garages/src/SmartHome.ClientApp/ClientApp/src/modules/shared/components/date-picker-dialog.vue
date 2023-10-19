<template>
  <v-dialog v-model="showDialog">
    <template #activator="{ props }">
      <v-btn v-bind="props" variant="text" class="pr-0">
        <v-icon size="20" start>mdi-calendar</v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title class="font-weight-bold">Choose date</v-card-title>
      <v-date-picker v-model="date" @click:save="submitDate()" @click:cancel="showDialog = false" />
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'

const emit = defineEmits<{
  chosen: [result: Date]
}>()

const showDialog = ref(false)
const date = ref<any>()

const submitDate = () => {
  nextTick(() => {
    const newDate = new Date(date.value)
    if (date.value != null) emit('chosen', newDate)
    showDialog.value = false
  })
}
</script>
