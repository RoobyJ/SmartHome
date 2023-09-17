<template>
    <v-container fluid>
        System organizations page
        <teleport v-if="afterMounted" to="#page-dashboard-actions">
            <add-organization-dialog />
        </teleport>
    </v-container>
</template>
<script setup lang="ts">
import AddOrganizationDialog from '@/modules/organization/components/add-organization-dialog.vue';
import { useAfterMounted } from '@/modules/shared/composables/after-mounted';

const { afterMounted } = useAfterMounted();
</script>
