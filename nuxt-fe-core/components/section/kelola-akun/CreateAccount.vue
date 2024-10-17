<script setup lang="ts">
import FormCreate from "@/components/reusable/kelola-akun/form/FormCreate.vue";

const isModalOpen = ref(false);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Buat Akun"
    description="Buat yang sumber income nggak cuma 1, semoga nggak cepet tipes aja sih 😂"
    v-model:is-modal-open="isModalOpen"
    mobile-button-icon="lucide:calculator"
    show-mobile-button
  >
    <template #trigger>
      <Button
        class="hidden md:flex items-center gap-2 w-full md:w-max justify-start"
      >
        <Icon name="lucide:calculator" />
        <span>Buat Akun</span>
      </Button>
    </template>
    <template #form>
      <FormCreate @create-mutate-executed="() => (isModalOpen = false)" />
    </template>
  </ReusableGlobalFormDialog>
</template>
