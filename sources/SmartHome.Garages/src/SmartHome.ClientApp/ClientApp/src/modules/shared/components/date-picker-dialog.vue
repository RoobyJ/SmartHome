<template>
<v-dialog v-model="showDialog">
    <template #activator="{ props }">
        <v-btn-secondary v-bind="props">
            <v-icon size="20" start>mdi-calendar</v-icon>
        </v-btn-secondary>
    </template>

    <v-card>
        <v-card-title>Choose date</v-card-title>
        
        <v-card-actions>
            <v-btn-primary @click="submitDate">Submit</v-btn-primary>
            <v-btn-secondary @click="showDialog = false">Cancel</v-btn-secondary>
        </v-card-actions>
    </v-card>
</v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const emit = defineEmits<{
    chosen: [result: Date];
}>();

const showDialog = ref(false);
const date = ref<Date | null>();


const submitDate = () => {
    if (date.value != null) emit('chosen', date.value);
};
</script>