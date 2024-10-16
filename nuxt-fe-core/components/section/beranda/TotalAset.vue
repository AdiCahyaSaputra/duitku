<script setup lang="ts">
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
} from "@/components/ui/card";
import { toIDR } from "@/lib/helper";
import { getTotalAssets } from "@/services/account.service";

const { data, isLoading } = useQuery({
  queryKey: ["get_total_assets"],
  queryFn: async () => await getTotalAssets({ paginate: false }),
  staleTime: 0,
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
          <h1 class="text-lg font-bold">
            {{ toIDR(data?.totalIncome.totalAsset || 0) }}
          </h1>
        </template>
        <template #loadingFallback>
          <h1 class="text-lg font-bold gap-2 flex items-center mt-2">
            <span>Rp</span>
            <span class="px-2 md:w-1/4 w-full inline-block bg-primary/20 rounded-md">&nbsp;</span>
          </h1>
        </template>
      </ReusableStateLoading>
    </div>

    <div class="flex flex-col gap-4">
      <Button class="w-full md:w-max flex items-center gap-2 justify-start">
        <Icon name="lucide:wallet"/>
        <span>Isi Saldo</span>
      </Button>

      <div class="flex gap-2 overflow-x-auto no-scrollbar">
        <Card v-for="(account, idx) in data?.totalIncome.accounts" :key="idx" 
          class="select-none flex flex-col justify-between shadow-sm">
          <CardHeader class="px-4 pb-0 pt-0">
            <CardDescription class="gap-1 flex items-center text-black pt-2">
              <Icon name="lucide:wallet-minimal"/>
              <span>{{ account.name }}</span>
            </CardDescription>
            <CardContent class="px-0 font-bold text-emerald-600 pb-2">{{ toIDR(account.balance) }}</CardContent>
          </CardHeader>
        </Card>
      </div>
    </div>
  </div>
</template>
