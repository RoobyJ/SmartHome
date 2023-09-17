<template>
    <h3 class="pb-4">TODO: zaakceptowanie zaproszenia/formularz rejestracji użytkownika</h3>
    <p>TOKEN: {{ token }}</p>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const { params } = useRoute();
const token = computed(() => (Array.isArray(params.token) ? params.token[0] : params.token));
</script>
