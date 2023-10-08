<template>
  <v-container>
    <div>
      <div>Heat Tasks</div>
      <v-btn variant="text" icon="mdi-plus" @click="openAddTaskDialog()" />
    </div>
    <div>
      <v-row>
        <v-col v-for="(item, index) in items" :key="index">
          <heat-task-item />
        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client'
import HeatTaskItem from '../components/heat-task-item.vue'
import { onMounted, ref } from 'vue'
import type { CyclicHeatRequestDto } from '@/modules/core/services/api/api.models'
import { useRoute } from 'vue-router'

const route = useRoute()

const items = ref<CyclicHeatRequestDto[] | null>(null)
const loadItems = async () => {
  const id = route.params.garageId

  if (id == null) return
  const response = await GarageClient.getCyclicHeatRequests(id)
  if (response.isSuccess) items.value = response.data
  console.log(items.value)
}

const openAddTaskDialog = () => {
  console.log('test')
}

onMounted(async () => {
  await loadItems()
})
</script>
