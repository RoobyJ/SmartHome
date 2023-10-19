<template>
    <div>
    <div class="d-flex justify-space-between">
      <div class="pl-4 d-flex align-center font-weight-bold">Heat Tasks</div>
      <add-heat-task-dialog />
    </div>
    <v-divider  />
    <div class="pt-2 mx-2">
      <v-row>
        <v-col cols="12" v-for="(item, index) in items" :key="index">
          <heat-task-item :heat-task="item" :is-disabled="true" />
        </v-col>
      </v-row>
    </div>
  </div>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client'
import HeatTaskItem from '../components/heat-task-item.vue'
import { onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import type { CyclicHeatTaskDto } from '@/modules/core/services/api/api.models';
import AddHeatTaskDialog from '../components/add-heat-task-dialog.vue';

const route = useRoute()

const items = ref<CyclicHeatTaskDto[] | null>(null)
const loadItems = async () => {
  const id = route.params.garageId

  if (id == null || typeof id !== 'string') return

  const response = await GarageClient.getCyclicHeatRequests(id)
  if (response.isSuccess) items.value = response.data
}

onMounted(async () => {
  await loadItems()
})
</script>