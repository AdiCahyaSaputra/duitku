<script setup lang="ts">
import { toIDR } from "@/lib/helper";
import { getTotalAssets } from "@/services/account.service";

const { data, isLoading } = useQuery({
  queryKey: ["get_total_assets"],
  queryFn: async () => await getTotalAssets({}),
  staleTime: 0
});

</script>

<template>
  <div class="text-left flex flex-col p-4 justify-between gap-4">
    <div class="space-y-1">
      <div>
        <h2 class="text-base font-bold text-black flex items-center gap-2">
          Total Aset Kamu
        </h2>
        <p class="text-sm text-gray-600">
          Jumlah total dari awal sampai hari ini
        </p>
      </div>
      <ReusableStateLoading :is-loading="isLoading">
        <template #content>
          <h1 class="text-lg font-bold">{{ toIDR(data?.totalAsset || 0) }}</h1>
        </template>
        <template #loadingFallback>
          <h1 class="text-lg font-bold gap-2 flex items-center mt-2">
            <span>Rp</span>
            <span class="px-2 md:w-1/4 w-full inline-block bg-primary/20 rounded-md">&nbsp;</span>
          </h1>
        </template>
      </ReusableStateLoading>
    </div>

    <div>
      <p class="text-sm flex items-center gap-2">
        <span class="text-green-600 font-bold">Stonk ni bre</span>
        <Icon name="lucide:trending-up" class="w-4 h-4" />
      </p>

      <p class="text-sm flex items-center gap-2">
        <span class="text-red-600 font-bold">Wah boros nih</span>
        <Icon name="lucide:trending-down" class="w-4 h-4" />
      </p>
    </div>
  </div>
</template>
