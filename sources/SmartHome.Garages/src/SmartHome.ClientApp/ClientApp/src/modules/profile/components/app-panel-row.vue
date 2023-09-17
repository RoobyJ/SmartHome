<template>
    <div class="d-flex align-center panel-row">
        <div class="font-weight-medium pt-2 panel-title-width">{{ title }}</div>
        <div v-if="!edit" class="text-truncate panel-value">{{ value }}</div>
        <v-text-field v-else v-model="value" hide-details="auto" density="comfortable" :rules="rules" />
    </div>
</template>

<script setup lang="ts">
import type { ValidationRule } from '@/modules/shared/composables/validators';
import type { PropType } from 'vue';
import { computed } from 'vue';

const emit = defineEmits(['update:modelValue']);
const props = defineProps({
    title: {
        type: String,
        required: true,
    },
    edit: {
        type: Boolean,
        default: false,
    },
    rules: {
        type: Array as PropType<ValidationRule[]>,
        required: true,
    },
    modelValue: {
        type: String as PropType<string | null>,
        required: true,
    },
});

const value = computed({
    get() {
        return props.modelValue;
    },
    set(value) {
        emit('update:modelValue', value);
    },
});
</script>

<style lang="scss">
.panel-row {
    font-size: 13px;
    min-height: 36px;
    margin-bottom: 10px;
}

.panel-title-width {
    min-width: 140px;
    align-self: flex-start;
}

.panel-value {
    font-weight: 600;
    padding-left: 10px;
}

.prepend-title-width {
    min-width: 124px;
}
</style>
