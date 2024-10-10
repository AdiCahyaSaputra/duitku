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

    <div class="flex justify-end items-center gap-2 p-4">
      <Button class="w-full md:w-max">
        Buat Transaksi
      </Button>
      <Button variant="outline" size="icon">
        <Icon name="lucide:filter"/>
      </Button>
    </div>

    <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4 p-4 pt-0">
      <SectionBerandaListAkun v-for="(list, idx) in [...'aaaaa']" :key="idx" />
    </div>
  </div>
</template>
