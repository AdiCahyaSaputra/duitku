<script setup lang="ts">
import FormTransaction from "@/components/reusable/beranda/form/FormTransaction.vue";

const isModalOpen = ref(false);
</script>

<template>
  <ReusableGlobalFormDialog title="Buat Transaksi" description="Catet transaksi disini bre"
    v-model:is-modal-open="isModalOpen">
    <template #trigger>
      <Button class="w-full md:flex hidden items-center justify-between">
        <Icon name="lucide:circle-fading-plus" class="w-4 h-4" />
        <span>Buat Transaksi</span>
      </Button>
    </template>

    <template #form>
      <FormTransaction @create-mutate-executed="() => (isModalOpen = false)" />
    </template>
  </ReusableGlobalFormDialog>
</template>
