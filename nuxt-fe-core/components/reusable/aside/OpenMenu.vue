<script setup lang="ts">
const props = defineProps<{
  open: boolean;
}>();

const emit = defineEmits(["toggleOpen"]);
</script>

<template>
  <div
    class="fixed lg:hidden bottom-0 p-4 inset-x-0 z-30 bg-white supports-[backdrop-filter]:bg-white/60 border-t supports-[backdrop-filter]:backdrop-blur-md"
  >
    <Button
      @click="emit('toggleOpen', props.open)"
      class="w-full flex items-center justify-start space-x-2"
    >
      <Icon name="lucide:menu" class="w-4 h-4" />
      <span>Buka Menu</span>
    </Button>
  </div>
</template>
