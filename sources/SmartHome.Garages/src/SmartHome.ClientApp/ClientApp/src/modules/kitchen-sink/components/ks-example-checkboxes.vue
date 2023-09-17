<template>
    <v-container fluid class="pa-0">
        <v-row dense>
            <v-col cols="12" class="text-h5">EXAMPLE CHECKBOXES - {{ title }}</v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-checkbox label="Default checkbox1" :error="error" :error-messages="errorMessages" />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-checkbox
                    v-model="errorsCheckbox1"
                    label="Checkbox - when checked it will be in error state"
                    :error="errorsCheckbox1"
                    :error-messages="errorsCheckbox1 ? errorMessages : []"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-checkbox
                    v-model="errorsCheckbox2"
                    label="Checkbox - with hint"
                    :error="errorsCheckbox2"
                    :error-messages="errorsCheckbox2 ? errorMessages : []"
                    hint="Check this to display validation error"
                    persistent-hint
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="1">
                <v-checkbox-btn v-model="enableInlineCheckbox" class="my-2" />
            </v-col>
            <v-col>
                <v-text-field
                    :disabled="!enableInlineCheckbox"
                    label="This example is v-checkbox-btn for inlining with other elements"
                />
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

const errorsCheckbox1 = ref(false);
const errorsCheckbox2 = ref(false);
const enableInlineCheckbox = ref(false);

const props = defineProps({
    error: { type: Boolean, default: false },
    title: { type: String, required: true },
});

const errorMessages = computed<string | undefined>(() => (props.error ? 'Validation error' : undefined));
</script>
