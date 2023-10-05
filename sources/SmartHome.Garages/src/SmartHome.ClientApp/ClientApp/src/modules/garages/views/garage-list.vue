<template>
  <v-container>
    <v-list v-if="garages">
      <v-list-item class="px-0" v-for="garage in garages" :key="garage.id" @click="openGarage(garage.id)">
        <garage-details :garage-name="garage.name" :heater-status="garage.heaterStatus" />
        <v-divider />
      </v-list-item>
    </v-list>
    <div v-else class="d-flex justify-center pt-6">No data</div>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import GarageDetails from '../components/garage-details.vue'
import { GaragesClient } from '@/modules/core/services/api-clients/garages-client'
import type { GarageDetailsDto } from '@/modules/core/services/api/api.models'
import { useRouter } from 'vue-router';
import { View } from '@/router/view-definitions';

const garages = ref<GarageDetailsDto[] | null>(null)

const router  = useRouter();

async function openGarage(id: number): Promise<any> {
  await router.push({ name: View.garagePageView, params: { garageId: id } });
}

onMounted(async () => {
  const response = await GaragesClient.getApplications()
  if (response.isSuccess) garages.value = response.data
})
</script>
