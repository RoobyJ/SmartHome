<template>
    <v-container fluid class="pa-0">
        <v-row dense>
            <v-col cols="12" class="text-h5">EXAMPLE TEXT FIELDS - {{ title }}</v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field label="DEFAULT TEXT FIELD" :density="density" />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field label="TEXT FIELD WITH HINT" hint="Wypełnij to pole tekstem" :density="density" />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    clearable
                    label="TEXT FIELD WITH ICONS + CLEARABLE"
                    :density="density"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    clearable
                    error
                    error-messages="Validation message"
                    label="TEXT FIELD WITH ICONS + CLEARABLE + VALIDATION ERRORS"
                    :density="density"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    readonly
                    model-value="TEXT FIELD WITH ICONS + READONLY"
                    :density="density"
                />
            </v-col>
        </v-row>
        <v-row dense>
            <v-col cols="12">
                <v-text-field
                    prepend-icon="mdi-home"
                    append-icon="mdi-home"
                    prepend-inner-icon="mdi-home"
                    append-inner-icon="mdi-home"
                    disabled
                    model-value="TEXT FIELD WITH ICONS + DISABLED"
                    :density="density"
                />
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import type { PropType } from 'vue';

defineProps({
    density: { type: String as PropType<'default' | 'comfortable'>, default: 'default' },
    title: { type: String, required: true },
});
</script>
