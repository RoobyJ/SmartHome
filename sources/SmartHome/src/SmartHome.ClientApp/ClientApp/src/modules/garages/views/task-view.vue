<template>
    <div>
        <div class="d-flex justify-space-between">
            <div class="pl-4 d-flex align-center font-weight-bold">Heat Tasks</div>
            <div>
                <v-btn variant="text" icon :disabled="selectedTasksIds.length < 1" @click="deleteTasks()">
                    <v-icon>mdi-delete</v-icon>
                </v-btn>
                <heat-task-form-dialog :selected-time="selectedTime" icon="mdi-plus" @created="loadItems" />
            </div>
        </div>
        <v-divider />
        <div class="pt-2 mx-2">
            <v-row>
                <v-col cols="12" v-for="(item, index) in cyclicTasks" :key="index">
                    <heat-task-item
                        :heat-task="item"
                        @clicked-checkbox="
                            (checkboxVal: boolean, id: number) => {
                                processCheckboxClick(checkboxVal, id, taskType.cyclic);
                            }
                        "
                        @updated="loadItems"
                    />
                </v-col>
                <v-col cols="12" v-for="(item, index) in customTasks" :key="index">
                    <heat-task-item
                        :heat-task="item"
                        @clicked-checkbox="
                            (checkboxVal: boolean, id: number) => {
                                processCheckboxClick(checkboxVal, id, taskType.custom);
                            }
                        "
                        @updated="loadItems"
                    />
                </v-col>
            </v-row>
        </div>
    </div>
    <notification v-model="showNotification" snack-bar-text="Successfully deleted" snack-bar-color="red" />
</template>

<script setup lang="ts">
import { GarageClient } from '@/modules/core/services/api-clients/garages-client';
import HeatTaskItem from '../components/heat-task-item.vue';
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { type CustomHeatTaskDto, type CyclicHeatTaskDto } from '@/modules/core/services/api/api.models';
import HeatTaskFormDialog from '../components/heat-task-form-dialog.vue';
import { TaskType } from '../garages.models';
import Notification from '../../core/components/notification.vue';

interface GenericTask {
    id: number;
    taskType: TaskType;
}

const route = useRoute();

const cyclicItems = ref<CyclicHeatTaskDto[]>([]);
const customItems = ref<CustomHeatTaskDto[]>([]);
const selectedTasksIds = ref<GenericTask[]>([]);
const showNotification = ref(false);
const selectedTime = ref('');
const taskType = TaskType;

const cyclicTasks = computed(() => cyclicItems.value);
const customTasks = computed(() => customItems.value);

const loadItems = async () => {
    const id = route.params.garageId;

    if (id == null || typeof id !== 'string') return;

    const cyclicTasksResponse = await GarageClient.getCyclicHeatRequests(id);
    if (cyclicTasksResponse.isSuccess) cyclicItems.value = cyclicTasksResponse.data;

    const customTasksResponse = await GarageClient.getCustomHeatRequests(id);
    if (customTasksResponse.isSuccess) customItems.value = customTasksResponse.data;
};

const processCheckboxClick = (val: boolean, id: number, taskType: TaskType) => {
    if (val) selectedTasksIds.value.push({ id, taskType });
    if (!val) selectedTasksIds.value = selectedTasksIds.value.filter(i => i.id !== id);
};

const deleteTasks = async () => {
    if (selectedTasksIds.value.length < 1) return;

    const garageId = route.params.garageId;

    if (garageId == null || typeof garageId !== 'string') return;

    selectedTasksIds.value.forEach(i => {
        if (i.taskType === TaskType.cyclic) GarageClient.deleteCyclicHeatRequest(garageId, i.id);
        if (i.taskType === TaskType.custom) GarageClient.deleteCustomHeatRequest(garageId, i.id);
    });

    showNotification.value = true;
    selectedTasksIds.value = [];
    await loadItems();
};

onMounted(async () => {
    await loadItems();
});
</script>
