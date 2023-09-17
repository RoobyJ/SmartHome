<template>
    <v-card rounded="0" class="card-shadow">
        <div class="font-weight-regular text-uppercase py-5 v-card-text-title">{{ cardTitle }}</div>
        <div>
            <v-divider :thickness="2" />
            <slot />
        </div>
    </v-card>
</template>

<script setup lang="ts">
defineProps({
    cardTitle: {
        type: String,
        required: true,
    },
});
</script>

<style lang="scss">
.card-shadow {
    box-shadow: 0 1px 12px #0000000a;
}

.v-card-text-title {
    color: #98a6b3;
    line-height: 12px;
    padding-left: 25px;
}
</style>
