<template>
  <v-container>
    <v-row>
      <v-col>
        <div>Time: {{ time }}</div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
defineProps({
  time: { type: String, required: true }
})
</script>
