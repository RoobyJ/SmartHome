<template>
    <v-container fluid>
        <v-row>
            <v-col cols="12">
                <ks-example-btns />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12"><v-divider /></v-col>
        </v-row>
        <v-row>
            <v-col cols="5">
                <ks-example-text-fields title="DEFAULT (40px)" />
            </v-col>
            <v-col cols="5">
                <ks-example-text-fields title="COMFORTABLE (36px)" density="comfortable" />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="5">
                <ks-example-fields-with-dropdowns title="DEFAULT (40px)" />
            </v-col>
            <v-col cols="5">
                <ks-example-fields-with-dropdowns title="COMFORTABLE (36px)" density="comfortable" />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12"><v-divider /></v-col>
        </v-row>
        <v-row>
            <v-col cols="6">
                <ks-example-checkboxes title="NO ERRORS" />
            </v-col>
            <v-col cols="6">
                <ks-example-checkboxes error title="WITH ERRORS" />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12"><v-divider /></v-col>
        </v-row>
        <v-row>
            <v-col cols="4">
                <ks-example-form title="NO ERRORS" />
            </v-col>
            <v-col cols="4">
                <ks-example-form error title="WITH ERRORS" />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12"><v-divider /></v-col>
        </v-row>
        <v-row>
            <v-col cols="12">
                <ksExampleLookupInput
                    example-title="SINGLE"
                    title="Organizations.Organization"
                    sub-title="Choose organization from list"
                />
            </v-col>
            <v-col cols="12">
                <ksExampleLookupInput
                    example-title="MULTIPLE"
                    title="Organizations.Organization"
                    sub-title="Choose organizations from list"
                    multiple
                />
            </v-col>
        </v-row>
        <v-row>
            <v-col cols="12"><v-divider /></v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import KsExampleBtns from '../components/ks-example-btns.vue';
import KsExampleCheckboxes from '../components/ks-example-checkboxes.vue';
import KsExampleFieldsWithDropdowns from '../components/ks-example-fields-with-dropdowns.vue';
import KsExampleForm from '../components/ks-example-form.vue';
import KsExampleTextFields from '../components/ks-example-text-fields.vue';
import ksExampleLookupInput from '../components/ks-example-lookup-input.vue';
</script>
