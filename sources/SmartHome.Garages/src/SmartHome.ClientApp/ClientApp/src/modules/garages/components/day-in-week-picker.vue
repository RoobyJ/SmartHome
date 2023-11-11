<template>
  <v-container class="d-flex direction-row justify-center mr-4">
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(0) ? 'selected' : ''"
      @click="toggleSelect(0)"
      >S</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(1) ? 'selected' : ''"
      @click="toggleSelect(1)"
      >M</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(2) ? 'selected' : ''"
      @click="toggleSelect(2)"
      >T</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(3) ? 'selected' : ''"
      @click="toggleSelect(3)"
      >W</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(4) ? 'selected' : ''"
      @click="toggleSelect(4)"
      >T</v-btn
    >
    <v-btn
      icon
      variant="text"
      class="mr-1"
      :class="isSelected(5) ? 'selected' : ''"
      @click="toggleSelect(5)"
      >F</v-btn
    >
    <v-btn icon variant="text" :class="isSelected(6) ? 'selected' : ''" @click="toggleSelect(6)"
      >S</v-btn
    >
  </v-container>
</template>

<script setup lang="ts">
import { computed, onMounted, watch } from 'vue'
import { ref } from 'vue'

const emit = defineEmits(['update:modelValue', 'pick']);

const props = defineProps({
  modelValue: { type: Boolean, default: false }
})

const resetData = computed({
  get() {
    return props.modelValue
  },
  set(value) {
    emit('update:modelValue', value)
  }
})

const selectedDays = ref<Map<number, boolean>>(new Map())

const isSelected = (n: number): boolean => {
  return selectedDays.value.get(n) ?? false
}

const toggleSelect = (n: number) => {
  const value = selectedDays.value.get(n)
  selectedDays.value.set(n, !value)
  emit('pick', selectedDays.value)
}

const resetSelectedDays = () => {
  selectedDays.value = new Map<number, boolean>([
    [0, false],
    [1, false],
    [2, false],
    [3, false],
    [4, false],
    [5, false],
    [6, false]
  ])
}

watch(resetData, (newValue: boolean) => {
  if(newValue) resetSelectedDays();
  resetData.value = false;
});

onMounted(() => {
  resetSelectedDays();
})
</script>

<style lang="scss">
.selected {
  color: orange;
  border-color: orange;
  border-width: 1px;
}
</style>
