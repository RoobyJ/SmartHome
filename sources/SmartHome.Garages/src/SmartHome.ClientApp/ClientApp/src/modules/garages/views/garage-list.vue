<template>
  <v-container>
    <v-list v-for="garage in garages" :key="garage.id">
      <v-list-item>
        <garage-details :garage-name="garage.name" :heater-status="garage.heaterStatus" />
      </v-list-item>
    </v-list>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import GarageDetails from '../components/garage-details.vue'
import { GaragesClient } from '@/modules/core/services/api-clients/garages-client'
import type { GarageDetailsDto } from '@/modules/core/services/api/api.models'

const garages = ref<GarageDetailsDto[] | null>(null)

onMounted(async () => {
  const response = await GaragesClient.getApplications()
  if (response.isSuccess) garages.value = response.data
})
</script>
@/modules/core/services/xdcopy
