<script setup lang="ts">
const props = defineProps<{
  url: string;
  label: string;
  iconName: string;
}>();

const emit = defineEmits<{
  (e: 'navItemClicked'): void
}>();

const baseClass = "flex items-center justify-start space-x-2 w-full";
</script>

<template>
  <NuxtLink :to="props.url">
    <Button @click="emit('navItemClicked')" :variant="$route.fullPath.startsWith(props.url) ? 'default' : 'ghost'"
      :class="`${baseClass} ${!$route.fullPath.startsWith(props.url) ? 'bg-white' : ''}`">
      <Icon :name="props.iconName" class="w-5 h-5" />
      <span>{{ props.label }}</span>
    </Button>
  </NuxtLink>
</template>
