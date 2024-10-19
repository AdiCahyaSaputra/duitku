<script setup lang="ts">
import { filterTransactionStore } from "@/store/filter-transaction.store";
import { headerStore } from "@/store/header.store";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Beranda",
});

const searchFilter = defineModel({ type: String });

const headerHeight = ref(0);

const { setSearchFilter } = filterTransactionStore();

headerStore().$subscribe((_, state) => {
  headerHeight.value = state.headerHeight;
});

const handleSearch = () => {
  setSearchFilter(searchFilter.value);
};
</script>

<template>
  <div class="min-h-[2000px] w-full">
    <div class="grid lg:grid-cols-2 grid-cols-1 gap-4 w-full border-b">
      <SectionBerandaTotalAset />
      <SectionBerandaTotalPengeluaran />
    </div>

    <div
      class="flex justify-between gap-2 p-4 sticky bg-white supports-[backdrop-filter]:bg-white/60 border-b supports-[backdrop-filter]:backdrop-blur-md z-20"
      :style="{ top: `${headerHeight}px` }"
    >
      <form @submit.prevent="handleSearch" class="flex w-full items-center gap-1.5 relative">
        <Input v-model="searchFilter" type="text" placeholder="Cari" class="w-full bg-white" />
        <Button type="submit" class="absolute right-0 rounded-l-none" size="icon">
          <Icon name="lucide:search" />
        </Button>
      </form>
      <div class="flex items-center gap-2">
        <SectionBerandaCreateTransaction />
        <SectionBerandaFilterTransaction />
      </div>
    </div>

    <div class="p-4 pb-20">
      <SectionBerandaListTransaction />
    </div>
  </div>
</template>
