<script setup lang="ts">
const props = defineProps<{
  url: string;
  label: string;
  iconName: string;
}>();
</script>

<template>
  <NuxtLink :to="props.url">
    <Button
      :variant="$route.fullPath.startsWith(props.url) ? 'default' : 'ghost'"
      class="flex propss-center justify-start space-x-2 w-full"
    >
      <Icon :name="props.iconName" class="w-5 h-5" />
      <span>{{ props.label }}</span>
    </Button>
  </NuxtLink>
</template>
