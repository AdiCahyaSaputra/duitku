<template>
  <div class="text-left flex flex-col lg:border-l lg:border-t-none border-t">
    <div class="p-4">
      <h2 class="text-sm font-bold text-black/60 flex items-center gap-2">
        <span>Total Pengeluaran Oktober</span>
        <Icon name="lucide:chevron-right" class="w-4 h-4" />
      </h2>
      <h1 class="text-lg font-bold mt-2">Rp. 100.000</h1>
    </div>

    <p class="text-sm flex items-center gap-2 p-4 pb-0 border-t">
      <span class="text-black font-bold">Top 3 yang ngabisin duit mu 😁</span>
    </p>

    <ul class="p-4 pt-0 text-sm mt-2">
      <li v-for="(item, idx) in [...'aaa']" :key="idx">
        {{ idx + 1 }}. Jajan Kopi
      </li>
    </ul>
  </div>
</template>
