<template>
  <div>
    <v-card>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client';
import type { TemperatureDto } from '@/modules/core/services/api/api.models';
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router';

const route = useRoute()

const loading = ref(false);
const totalItems = ref(0);
const items = ref<TemperatureDto[]>([]);


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
