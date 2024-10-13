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
import { getTransactions } from "@/services/transaction.service";

const pageNumber = ref(1);

const { data, isLoading } = useQuery({
  queryKey: ["get_transactions", pageNumber],
  queryFn: async () =>
    await getTransactions({
      paginate: true,
      pageNumber: pageNumber.value,
      limit: 10,
    }),
});
</script>

<template>
  <ReusableStateLoading :is-loading="isLoading">
    <template #content>
      <ReusableStateEmpty :is-empty="!data">
        <template #content>
          <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
            <Card v-for="(transaction, idx) in data?.transactions" :key="idx" class="select-none">
              <CardHeader class="space-y-3">
                <CardDescription class="gap-1 flex flex-col">
                  <Badge class="px-2 w-max bg-emerald-500">{{
                    transaction.category.name
                  }}</Badge>
                  <Badge v-show="transaction.subCategory" variant="secondary" class="px-2 w-max bg-emerald-500/20">{{
                    transaction.subCategory?.name }}</Badge>
                </CardDescription>
                <CardContent class="leading-5 px-0 font-medium">{{
                  transaction.description
                }}</CardContent>
              </CardHeader>
              <CardFooter class="flex flex-col items-end gap-4 mt-8">
                <div class="flex items-end justify-between gap-2 w-full">
                  <div>
                    <p class="text-xs mb-1">
                      {{
                        transformDotNetTimestamp(transaction.date).join(", ")
                      }}
                    </p>
                    <div class="flex items-center gap-2">
                      <Icon name="lucide:wallet-minimal" class="w-4 h-4" />
                      <p class="text-sm font-semibold">
                        {{ transaction.account.name }}
                      </p>
                    </div>

                    <p class="mt-3 font-bold text-emerald-600 text-lg">
                      Rp. {{ transaction.amount }}
                    </p>
                  </div>
                  <Button class="w-max" size="sm">
                    <Icon name="lucide:arrow-up-right" class="w-4 h-4" />
                  </Button>
                </div>
              </CardFooter>
            </Card>
          </div>

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
          <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
            <div>
              <h3 class="scroll-m-20 text-lg font-semibold tracking-tight">
                Belum ada transaksi nih
              </h3>
              <p class="text-sm text-gray-500">
                Yuk belanjain duit nya sekarang
              </p>
            </div>
          </div>
        </template>
      </ReusableStateEmpty>
    </template>
    <template #loadingFallback>
      <SectionBerandaListTransaksiLoader />
    </template>
  </ReusableStateLoading>
</template>
