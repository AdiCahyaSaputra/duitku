<template>
  <div class="text-left flex flex-col lg:border-l lg:border-t-none border-t">
    <div class="p-4">
      <div class="flex justify-between items-start gap-2 w-full">
        <div>
          <h2 class="text-base font-bold text-black">Total Pengeluaran Oktober</h2>
          <p class="text-sm text-gray-600">01 Oktober - 31 Oktober</p>
        </div>
        <Button variant="outline" size="icon">
          <Icon name="lucide:calendar-range" class="w-4 h-4" />
        </Button>
      </div>
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
