<script setup lang="ts">
const props = defineProps<{
  isLoading: boolean;
}>();
</script>

<template>
  <ClientOnly>
    <div>
      <slot v-if="props.isLoading" name="loadingFallback" />
      <slot v-else name="content" />
    </div>
  </ClientOnly>
</template>
