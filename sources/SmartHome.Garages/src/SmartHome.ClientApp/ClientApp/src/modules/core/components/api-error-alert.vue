<template>
    <v-alert v-if="alert != null" type="error" variant="tonal" closable @click:close="alert = null">
        <ul v-if="alert.messages.length > 0">
            <li v-for="message in alert.messages" :key="message">{{ message }}</li>
        </ul>
        <div v-if="alert.showDetails" class="d-flex">
            <div class="show-details-btn" @click="processError(alert.response)">{{ t('Core.ShowDetails') }}</div>
        </div>
    </v-alert>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import type { ApiErrorAlert } from '../services/errors/error.utils';
import { useErrorStore } from '../store/error-store';

const { t } = useI18n();

const alert = computed({
    get() {
        return props.modelValue;
    },
    set(value) {
        emit('update:modelValue', value);
    },
});

const { processError } = useErrorStore();

const emit = defineEmits(['update:modelValue']);
const props = defineProps({
    modelValue: { type: Object as PropType<ApiErrorAlert | null>, required: true },
});
</script>

<style lang="scss" scoped>
.show-details-btn {
    cursor: pointer;
    text-decoration: none;

    &:hover {
        text-decoration: underline;
    }
}
</style>
