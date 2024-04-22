<template>
    <v-container>
        <v-row>
            <v-col cols="6" sm="4" lg="3">
                <v-card :to="{ name: View.temperatureView }" class="dashboard-tile">
                    <v-responsive :aspect-ratio="1" class="flex align-center">
                        <div class="d-flex flex-column align-center">
                            <v-avatar size="63%">
                                <v-responsive :aspect-ratio="1">
                                    <div class="d-flex align-center justify-center w-100">
                                        <v-icon size="70" class="pt-14">mdi-thermometer</v-icon>
                                    </div>
                                </v-responsive>
                            </v-avatar>
                            <div class="mt-3 font-weight-medium">Temperatures</div>
                        </div>
                    </v-responsive>
                </v-card>
            </v-col>
            <v-col cols="6" sm="4" lg="3">
                <v-card :to="{ name: View.taskView }" class="dashboard-tile">
                    <v-responsive :aspect-ratio="1" class="flex align-center">
                        <div class="d-flex flex-column align-center">
                            <v-avatar size="63%">
                                <v-responsive :aspect-ratio="1">
                                    <div class="d-flex align-end justify-center w-100">
                                        <v-icon size="70" class="pt-14">mdi-fire</v-icon>
                                    </div>
                                </v-responsive>
                            </v-avatar>
                            <div class="mt-3 font-weight-medium">Tasks</div>
                        </div>
                    </v-responsive>
                </v-card>
            </v-col>
            <v-col cols="6" sm="4" lg="3">
                <v-card class="dashboard-tile" :color="heaterStatusColor" @click="ChangeStatus()">
                    <v-responsive :aspect-ratio="1" class="flex align-center">
                        <div class="d-flex flex-column align-center">
                            <v-avatar size="63%">
                                <v-responsive :aspect-ratio="1">
                                    <div class="d-flex align-end justify-center w-100">
                                        <v-icon size="70" class="pt-14">mdi-radiator</v-icon>
                                    </div>
                                </v-responsive>
                            </v-avatar>
                            <div class="mt-3 font-weight-medium">Tasks</div>
                        </div>
                    </v-responsive>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import { View } from '@/router/view-definitions';
import { GarageClient } from '@/modules/core/services/api-clients/garages-client';
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import { isArray } from 'chart.js/helpers';

const isHeating = ref<boolean | null>(null);
const garageId = ref<number | null>(null);

const route = useRoute();

const heaterStatusColor = computed(() => {
    return isHeating.value != null && isHeating.value ? 'green' : 'red';
});

async function ChangeStatus() {
    if (garageId.value == null) return;
    await GarageClient.setGarageHeaterStatus(garageId.value, !isHeating.value);
    isHeating.value = !isHeating.value;
}

onMounted(async () => {
    const id = route.params.garageId;
    if (!isArray(id)) garageId.value = parseInt(id);
    if (garageId.value == null) return;

    const result = await GarageClient.getGarageHeaterStatus(garageId.value);

    if (result.isSuccess) isHeating.value = result.data;
});
</script>
<style lang="scss">
.v-sheet.v-card.dashboard-tile {
    padding: 8px;
    box-shadow: 0px 2px 6px #6161b829;
    font-size: 14px;

    .v-avatar {
        background-color: gray;

        .v-icon svg {
            width: 55% !important;
        }
    }
}
</style>
