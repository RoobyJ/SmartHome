<template>
    <v-snackbar v-model="show" :timeout="3000" :color="snackBarColor">
        {{ snackBarText }}

        <template v-slot:actions>
            <v-btn color="white" variant="text" @click="show = false"><v-icon>mdi-close</v-icon></v-btn>
        </template>
    </v-snackbar>
</template>

<script setup lang="ts">
import { computed } from 'vue';


const props = defineProps({
  modelValue: { type: Boolean, default: false },
  snackBarText: { type: String, required: true},
  snackBarColor: { type: String, default: 'green'}
})

const emit = defineEmits(['update:modelValue'])
const show = computed({
  get() {
    return props.modelValue
  },
  set(value) {
    emit('update:modelValue', value)
  }
})

</script>
