<template>
  <div>
    <div class="d-flex justify-space-between">
      <div class="pl-4 d-flex align-center font-weight-bold">Heat Tasks</div>
      <div>
        <v-btn variant="text" icon :disabled="selectedTasksIds.length < 1" @click="deleteTasks()"><v-icon>mdi-delete</v-icon></v-btn>
        <heat-task-form-dialog :selected-time="selectedTime" icon="mdi-plus" @created="loadItems" />
      </div>
    </div>
    <v-divider />
    <div class="pt-2 mx-2">
      <v-row>
        <v-col cols="12" v-for="(item, index) in cyclicItems" :key="index">
          <heat-task-item :heat-task="item" @clicked-checkbox="processCheckboxClick" @updated="loadItems" />
        </v-col>
        <v-col cols="12" v-for="(item, index) in customItems" :key="index">
          <heat-task-item :heat-task="item" @clicked-checkbox="processCheckboxClick" @updated="loadItems" />
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
import { type CustomHeatTaskDto, type CyclicHeatTaskDto } from '@/modules/core/services/api/api.models'
import HeatTaskFormDialog from '../components/heat-task-form-dialog.vue'

const route = useRoute()

const cyclicItems = ref<CyclicHeatTaskDto[] | null>(null)
const customItems = ref<CustomHeatTaskDto[] | null>(null)
const selectedTasksIds = ref<number[]>([]);
const selectedTime = ref('');

const loadItems = async () => {
  const id = route.params.garageId

  if (id == null || typeof id !== 'string') return

  const cyclicTasksResponse = await GarageClient.getCyclicHeatRequests(id)
  if (cyclicTasksResponse.isSuccess) cyclicItems.value = cyclicTasksResponse.data

  const customTasksResponse = await GarageClient.getCustomHeatRequests(id);
  if (customTasksResponse.isSuccess) customItems.value = customTasksResponse.data
}

const processCheckboxClick = (val: boolean, id: number  ) => {
  if (val) selectedTasksIds.value.push(id);
  if (!val) selectedTasksIds.value = selectedTasksIds.value.filter(i => i !== id);
}

const deleteTasks = () => {
  if (selectedTasksIds.value.length < 1) return;

  const id = route.params.garageId

  if (id == null || typeof id !== 'string') return

  selectedTasksIds.value.forEach(i => GarageClient.deleteCyclicHeatRequest(id, i));
  selectedTasksIds.value = [];
  cyclicItems.value  = cyclicItems.value?.filter(i => selectedTasksIds.value.find(x => x == i.id)) ?? [];
  loadItems();
}

onMounted(async () => {
  await loadItems()
})
</script>
