<template>
  <div>
    <div>
      <!-- <v-date-picker /> -->
    </div>
    <v-card>
      <v-data-table
                :headers="headers"
                :items="items"
                :items-length="totalItems"
                item-value="name"
                :loading="loading"
                class="font-weight-medium"
                @update:options="loadItems"
            />
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client';
import type { TemperatureDto } from '@/modules/core/services/api/api.models';
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router';
import { type DataTableHeader } from '@/modules/core/core.models';

const route = useRoute()

const headers = ref<DataTableHeader[] | DataTableHeader[][]>([
    { title: 'Date', key: 'date', sortable: false },
    { title: 'Temperature', key: 'temperature', sortable: false },
]);

const loading = ref(false);
const items = ref<TemperatureDto[]>([]);
const totalItems = ref(0);

async function loadItems(): Promise<void> {
    const id = route.params.garageId

  if (id == null || typeof id !== 'string') return

    loading.value = true;

    const response = await GarageClient.getGarageTemperatures(id);

    if(response.isSuccess) {
        items.value = response.data;
        totalItems.value = response.data.length;
    }
    loading.value = false;
}

onMounted(async () => {
    await loadItems();
})
</script>
