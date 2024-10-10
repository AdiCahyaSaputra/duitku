<script setup lang="ts">
definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Beranda",
});
</script>

<template>
  <div class="h-[2000px] w-full">
    <div class="grid lg:grid-cols-2 grid-cols-1 gap-4 w-full border-b">
      <SectionBerandaTotalAset />
      <SectionBerandaTotalPengeluaran />
    </div>

    <div class="flex justify-between gap-2 p-4">
      <div class="flex w-full md:max-w-sm items-center gap-1.5 relative">
        <Input type="text" placeholder="Cari" class="w-full" />
        <Button class="absolute right-0 rounded-l-none" size="icon">
          <Icon name="lucide:search"/>
        </Button>
      </div>
      <div class="flex items-center gap-2">
        <Button class="w-full md:flex hidden items-center justify-between">
          <span>Buat Transaksi</span>
          <Icon name="lucide:circle-fading-plus" class="w-4 h-4" />
        </Button>
        <Button size="icon" class="md:hidden">
          <Icon name="lucide:circle-fading-plus" />
        </Button>
        <Button variant="outline" size="icon">
          <Icon name="lucide:filter" />
        </Button>
      </div>
    </div>

    <div class="p-4 pt-0">
      <SectionBerandaListTransaksi />
    </div>
  </div>
</template>
