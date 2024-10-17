<script setup lang="ts">
import FormCreate from "@/components/reusable/kelola-kategori/form/FormCreate.vue";

const isModalOpen = ref(false);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Buat Kategori"
    description="Biar kategori nya lebih eye-catching bisa pake Emote di depan nama nya bre"
    v-model:is-modal-open="isModalOpen"
  >
    <template #trigger>
      <Button class="flex items-center gap-2 w-full md:w-max justify-start">
        <Icon name="lucide:book-minus" />
        <span>Buat Kategori</span>
      </Button>
    </template>
    <template #form>
      <FormCreate @create-mutate-executed="() => (isModalOpen = false)" />
    </template>
  </ReusableGlobalFormDialog>
</template>
