<script setup lang="ts">
import Badge from "@/components/ui/badge/Badge.vue";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { transformDotNetTimestamp } from "@/lib/helper";
import { getTransactions } from "@/services/transaction-service";

const pageNumber = ref(1);

const { data, isLoading } = useQuery({
  queryKey: ["get_transactions", pageNumber],
  queryFn: async () => await getTransactions(pageNumber.value),
});
</script>

<template>
  <ReusableStateLoading :is-loading="isLoading">
    <template #content>
      <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
        <ReusableStateEmpty :is-empty="!data">
          <template #content>
            <Card v-for="(transaction, idx) in data?.transactions" :key="idx" class="select-none">
              <CardHeader class="space-y-3">
                <CardDescription class="gap-1 flex flex-col">
                  <Badge class="px-2 w-max bg-emerald-500">{{
                    transaction.category.name
                  }}</Badge>
                  <Badge v-show="transaction.subCategory" variant="secondary" class="px-2 w-max bg-emerald-500/20">{{
                    transaction.subCategory?.name }}</Badge>
                </CardDescription>
                <CardTitle class="leading-5">{{
                  transaction.description
                }}</CardTitle>
              </CardHeader>
              <CardContent class="font-bold text-right py-0">Rp. {{ transaction.amount }}</CardContent>
              <CardFooter class="flex flex-col items-end gap-2 mt-4">
                <div class="text-sm text-gray-700 flex justify-between items-center w-full">
                  <span>{{
                    transformDotNetTimestamp(transaction.date)[0]
                  }}</span>
                  <span>{{
                    transformDotNetTimestamp(transaction.date)[1]
                  }}</span>
                </div>
                <Button class="flex items-center justify-between gap-2 w-full" size="sm">
                  <span>{{ transaction.account.name }}</span>
                  <Icon name="lucide:arrow-up-right" class="w-4 h-4" />
                </Button>
              </CardFooter>
            </Card>

            <div class="mt-4 space-x-2">
              <Button variant="outline" size="icon" :disabled="!data?.isPreviousExists">
                <Icon name="lucide:chevron-left" />
              </Button>
              <Button variant="outline" size="icon" :disabled="!data?.isNextExists">
                <Icon name="lucide:chevron-right" />
              </Button>
            </div>
          </template>

          <template #emptyFallback>
            <h3 class="scroll-m-20 text-lg font-semibold tracking-tight">
              Belum ada transaksi nih
            </h3>
            <p class="text-sm text-gray-500">Yuk belanjain duit nya sekarang</p>
          </template>
        </ReusableStateEmpty>
      </div>
    </template>
    <template #loadingFallback>
      <SectionBerandaListTransaksiLoader />
    </template>
  </ReusableStateLoading>
</template>
