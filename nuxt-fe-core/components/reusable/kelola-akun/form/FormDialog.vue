<script setup lang="ts">
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

const emit = defineEmits(['update:isModalOpen']);
const props = defineProps<{
  title: string;
  description: string;
  isModalOpen: boolean;
}>();

const isModalOpenRef = ref(props.isModalOpen);

watch(
  () => props.isModalOpen,
  (newVal) => {
    isModalOpenRef.value = newVal;
  }
);

watch(isModalOpenRef, (newVal) => {
  emit('update:isModalOpen', newVal); // Buat update v-model nya
});

</script>

<template>
  <Dialog v-model:open="isModalOpenRef">
    <DialogTrigger asChild>
      <slot name="trigger" />
    </DialogTrigger>
    <DialogTrigger asChild>
      <Button size="icon" class="md:hidden">
        <Icon name="lucide:circle-fading-plus" />
      </Button>
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle class="text-left">
          {{ props.title }}
        </DialogTitle>
        <DialogDescription class="text-left">{{
          props.description
        }}</DialogDescription>
      </DialogHeader>

      <slot name="form" />
    </DialogContent>
  </Dialog>
</template>
