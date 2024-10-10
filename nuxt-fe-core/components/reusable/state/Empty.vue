<script setup lang="ts">
const props = defineProps<{
  isEmpty: boolean;
}>();
</script>

<template>
  <ClientOnly>
    <div>
      <slot v-if="props.isEmpty" name="emptyFallback" />
      <slot v-else name="content" />
    </div>
  </ClientOnly>
</template>
