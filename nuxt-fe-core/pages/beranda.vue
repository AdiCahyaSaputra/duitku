<script setup lang="ts">
import { headerStore } from "@/store/header.store";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Beranda",
});

const headerHeight = ref(0);

headerStore().$subscribe((_, state) => {
  headerHeight.value = state.headerHeight;
});
</script>

<template>
  <div class="h-[2000px] w-full">
    <div class="grid lg:grid-cols-2 grid-cols-1 gap-4 w-full border-b">
      <SectionBerandaTotalAset />
      <SectionBerandaTotalPengeluaran />
    </div>

    <div
      class="flex justify-between gap-2 p-4 sticky bg-white supports-[backdrop-filter]:bg-white/60 border-b supports-[backdrop-filter]:backdrop-blur-md"
      :style="{ top: `${headerHeight}px` }">
      <div class="flex w-full md:max-w-sm items-center gap-1.5 relative">
        <Input type="text" placeholder="Cari" class="w-full" />
        <Button class="absolute right-0 rounded-l-none" size="icon">
          <Icon name="lucide:search" />
        </Button>
      </div>
      <div class="flex items-center gap-2">
        <ReusableBerandaFormDialog />
        <Button variant="outline" size="icon">
          <Icon name="lucide:filter" />
        </Button>
      </div>
    </div>

    <div class="p-4">
      <SectionBerandaListTransaksi />
    </div>
  </div>
</template>
