<template>
  <v-container class="d-flex direction-row justify-center mr-4">
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(0) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(0)"
      >S</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(1) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(1)"
      >M</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(2) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(2)"
      >T</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(3) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(3)"
      >W</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(4) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(4)"
      >T</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(5) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(5)"
      >F</v-btn
    >
    <v-btn
      icon
      variant="text"
      :class="isSelected(6) ? 'selected' : ''"
      :disabled="disabled"
      @click="toogleSelect(6)"
      >S</v-btn
    >
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { ref } from 'vue'

defineProps({
  disabled: { type: Boolean, required: true }
})

const selectedDays = ref<Map<number, boolean>>(new Map())

const isSelected = (n: number): boolean => {
  console.log(selectedDays.value.get(n))
  return selectedDays.value.get(n) ?? false
}

const toogleSelect = (n: number) => {
  const value = selectedDays.value.get(n)
  selectedDays.value.set(n, !value)
}

onMounted(() => {
  selectedDays.value = new Map<number, boolean>([
    [0, false],
    [1, false],
    [2, false],
    [3, false],
    [4, false],
    [5, false],
    [6, false]
  ])
})
</script>

<style lang="scss">
.selected {
  color: orange;
  border-color: orange;
  border-width: 1px;
}
</style>
