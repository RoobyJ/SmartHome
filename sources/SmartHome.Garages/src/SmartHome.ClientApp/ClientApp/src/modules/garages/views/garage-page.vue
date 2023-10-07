<template>
  <v-container>
    <div>
      <div>Heat Tasks</div>
      <v-btn variant="text" icon="mdi-plus" @click="openAddTaskDialog()" />
    </div>
    <div>
      <v-row>
        <v-col><heat-task-item /></v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client';
import HeatTaskItem from '../components/heat-task-item.vue';
import { onMounted, ref } from 'vue';

const items = ref<number>(1);
const loadItems = async() => {
    const response = GarageClient.getGarages();
}

const openAddTaskDialog = () => {
    console.log('test');
}

onMounted( async() => {
    await loadItems();
})
</script>
