<script setup lang="ts">
const props = defineProps<{
  title: string;
  description: string;
  isPending: boolean;
  isOpen: boolean;
  cancelText?: string;
  confirmText?: {
    isPending?: string;
    isIdle?: string;
  };
}>();

const emit = defineEmits<{
  (e: "handleDelete"): void;
  (e: "update:isOpen", isOpen: boolean): void;
}>();

const isOpenRef = ref(props.isOpen);

watch(
  () => props.isOpen,
  (newVal) => {
    isOpenRef.value = newVal;
  }
)

watch(isOpenRef, (newVal) => {
  emit('update:isOpen', newVal);
});
</script>

<template>
  <AlertDialog :open="isOpenRef">
    <AlertDialogTrigger asChild>
      <slot name="trigger"/>
    </AlertDialogTrigger>
    <AlertDialogContent>
      <AlertDialogHeader>
        <AlertDialogTitle>{{ props.title }}</AlertDialogTitle>
        <AlertDialogDescription>{{ props.description }}</AlertDialogDescription>
      </AlertDialogHeader>
      <AlertDialogFooter>
        <AlertDialogCancel @click="isOpenRef = false">{{ props.cancelText || 'Ga Jadi 🙏' }}</AlertDialogCancel>
        <AlertDialogAction
          @click="emit('handleDelete')"
          class="bg-destructive hover:bg-destructive/90"
          :disabled="props.isPending"
        >
          {{ props.isPending ? (props.confirmText?.isPending || 'Lagi proses..') : (props.confirmText?.isIdle || 'Iya Hapus 👌') }}
        </AlertDialogAction>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>
