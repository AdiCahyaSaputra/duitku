<script setup lang="ts">
import Badge from "@/components/ui/badge/Badge.vue";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
} from "@/components/ui/card";
import { toIDR, transformDotNetTimestamp } from "@/lib/helper";
import { getTransactions } from "@/services/transaction.service";
import { filterTransactionStore } from "@/store/filter-transaction.store";

const pageNumber = ref(1);

const {
  search,
  categoryId,
  subCategoryId,
  accountId,
  dateStart,
  dateEnd
} = filterTransactionStore();

const searchRef = ref<string | undefined>(search);
const categoryIdRef = ref<string | undefined>(categoryId);
const subCategoryIdRef = ref<string | undefined>(subCategoryId);
const accountIdRef = ref<string | undefined>(accountId);
const dateStartRef = ref<string | undefined>(dateStart);
const dateEndRef = ref<string | undefined>(dateEnd);

const { data, isLoading, refetch } = useQuery({
  queryKey: [
    "get_transactions",
    pageNumber,
    searchRef,
    categoryIdRef,
    subCategoryIdRef,
    accountIdRef,
    dateStartRef,
    dateEndRef,
  ],
  queryFn: async () =>
    await getTransactions({
      paginate: true,
      pageNumber: pageNumber.value,
      limit: 10,
      search: searchRef.value,
      categoryId: categoryIdRef.value,
      subCategoryId: subCategoryIdRef.value,
      accountId: accountIdRef.value,
      dateStart: dateStartRef.value,
      dateEnd: dateEndRef.value,
    }),
  refetchOnMount: "always",
});

filterTransactionStore().$subscribe((_, state) => {
  searchRef.value = state.search;
  categoryIdRef.value = state.categoryId;
  subCategoryIdRef.value = state.subCategoryId;
  accountIdRef.value = state.accountId;
  dateStartRef.value = state.dateStart;
  dateEndRef.value = state.dateEnd;

  refetch();
});
</script>

<template>
  <ReusableStateLoading :is-loading="isLoading">
    <template #content>
      <ReusableStateEmpty :is-empty="!data">
        <template #content>
          <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4">
            <Card
              v-for="(transaction, idx) in data?.transactions"
              :key="idx"
              class="select-none flex flex-col justify-between"
            >
              <CardHeader class="space-y-3">
                <CardDescription class="gap-1 flex flex-col">
                  <Badge class="px-2 w-max bg-emerald-500">{{
                    transaction.category.name
                  }}</Badge>
                  <Badge
                    v-show="transaction.subCategory"
                    variant="secondary"
                    class="px-2 w-max bg-emerald-500/20"
                    >{{ transaction.subCategory?.name }}</Badge
                  >
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
                      {{ toIDR(transaction.amount) }}
                    </p>
                  </div>
                  <ReusableBerandaFormDeleteDialog :id="transaction.id" />
                </div>
              </CardFooter>
            </Card>
          </div>

          <div class="mt-4 space-x-2">
            <Button
              variant="outline"
              size="icon"
              @click="pageNumber = pageNumber - 1"
              :disabled="!data?.isPreviousExists"
            >
              <Icon name="lucide:chevron-left" />
            </Button>
            <Button
              variant="outline"
              size="icon"
              @click="pageNumber = pageNumber + 1"
              :disabled="!data?.isNextExists"
            >
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
      <SectionBerandaListTransactionLoader />
    </template>
  </ReusableStateLoading>
</template>
