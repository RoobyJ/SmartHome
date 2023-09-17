<template>
    <v-container fluid class="pa-0">
        <v-form>
            <v-row dense>
                <v-col cols="12" class="text-h5">EXAMPLE FORM - {{ title }}</v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-form-label>Email</v-form-label>
                </v-col>
                <v-col cols="12">
                    <v-text-field :error="error" :error-messages="errorMessages" />
                </v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-form-label>Imię</v-form-label>
                </v-col>
                <v-col cols="12">
                    <v-text-field />
                </v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-form-label>Nazwisko</v-form-label>
                </v-col>
                <v-col cols="12">
                    <v-text-field />
                </v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-form-label>Hasło</v-form-label>
                </v-col>
                <v-col cols="12">
                    <v-text-field type="password" :error="error" :error-messages="errorMessages" />
                </v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-checkbox label="Akceptuję regulamin" :error="error" :error-messages="errorMessages" />
                </v-col>
            </v-row>
            <v-row dense>
                <v-col cols="12">
                    <v-form-label>Zakresy (użyto klasę "checkbox-list" aby checkboxy były bliżej siebie)</v-form-label>
                </v-col>
                <v-col cols="12" class="checkbox-list">
                    <v-checkbox label="profile" hide-details />
                    <v-checkbox label="portal_api" hide-details />
                    <v-checkbox label="address_email" hide-details />
                    <v-checkbox label="user_name" :error-messages="errorMessages" />
                </v-col>
            </v-row>
        </v-form>
    </v-container>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
    error: { type: Boolean, default: false },
    title: { type: String, required: true },
});

const errorMessages = computed<string | undefined>(() => (props.error ? 'Validation error' : undefined));
</script>
