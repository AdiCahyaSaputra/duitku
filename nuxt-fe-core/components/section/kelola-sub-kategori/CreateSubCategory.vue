<script setup lang="ts">
import FormCreate from "@/components/reusable/kelola-sub-kategori/form/FormCreate.vue";

const isModalOpen = ref(false);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Buat Sub Kategori"
    description="Biar Sub kategori nya lebih eye-catching bisa pake Emote di depan nama nya bre"
    v-model:is-modal-open="isModalOpen"
    mobile-button-icon="lucide:book-minus"
    show-mobile-button
  >
    <template #trigger>
      <Button class="hidden md:flex items-center gap-2 w-full md:w-max justify-start">
        <Icon name="lucide:book-minus" />
        <span>Buat Sub Kategori</span>
      </Button>
    </template>
    <template #form>
      <FormCreate @create-mutate-executed="() => (isModalOpen = false)" />
    </template>
  </ReusableGlobalFormDialog>
</template>
