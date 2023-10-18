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
    <v-row>
            <v-col cols="6" sm="4" lg="3" v-if="hasAppAccessPermission">
                <v-card :to="{ name: page.myRoutes }" class="dashboard-tile">
                    <v-responsive :aspect-ratio="1" class="flex align-center">
                        <div class="d-flex flex-column align-center">
                            <v-avatar size="63%">
                                <v-responsive :aspect-ratio="1">
                                    <div class="d-flex align-center justify-center w-100">
                                        <v-icon size="100%">$dm-my-routes</v-icon>
                                    </div>
                                </v-responsive>
                            </v-avatar>
                            <div class="mt-3 font-weight-medium">{{ $t('MyRoutes.MyRoutes') }}</div>
                        </div>
                    </v-responsive>
                </v-card>
            </v-col>
        </v-row>
  </div>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client'
import HeatTaskItem from '../components/heat-task-item.vue'
import { onMounted, ref } from 'vue'
import type { CyclicHeatTaskDto } from '@/modules/core/services/api/api.models'
import AddHeatTaskDialog from '../components/add-heat-task-dialog.vue';
import { useRoute } from 'vue-router'

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