<script setup lang="ts">
import {
  castStringDateIntoDotNetDate,
  toComboboxCommandListFriendly,
  toIDR,
  transformDotNetTimestamp,
} from "@/lib/helper";
import { getAccounts } from "@/services/account.service";
import { getCategories } from "@/services/category.service";
import { getSubCategories } from "@/services/sub-category.service";
import { getTransactions } from "@/services/transaction.service";
import { getLocalTimeZone } from "@internationalized/date";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Laporan",
});

const searchFilter = defineModel({ type: String });

const pageNumber = ref(1);
const searchRef = ref<string | undefined>("");
const categoryIdRef = ref<string | undefined>("");
const subCategoryIdRef = ref<string | undefined>("");
const accountIdRef = ref<string | undefined>("");
const dateStartRef = ref<string | undefined>("");
const dateEndRef = ref<string | undefined>("");

const { data: accountsResponse, isLoading: accountsFetchLoading } = useQuery({
  queryKey: ["get_accounts"],
  queryFn: () => getAccounts({ paginate: false, limit: -1 }),
  refetchOnMount: "always",
});

const { data: categoriesResponse, isLoading: categoriesFetchLoading } =
  useQuery({
    queryKey: ["get_categories"],
    queryFn: () => getCategories({ paginate: false, limit: -1 }),
    refetchOnMount: "always",
  });

const { data: subCategoriesResponse, isLoading: subCategoriesFetchLoading } =
  useQuery({
    queryKey: ["get_sub_categories", categoryIdRef.value],
    queryFn: () =>
      getSubCategories({
        paginate: false,
        limit: -1,
        categoryId: categoryIdRef.value || "",
      }),
    enabled: computed(() => !!categoryIdRef.value),
    refetchOnMount: "always",
  });

const {
  data: transactionResponse,
  isLoading: transactionFetchLoading,
} = useQuery({
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

const handleSearch = () => {
  searchRef.value = searchFilter.value || "";
};
</script>

<template>
  <div class="w-full pb-20">
    <div class="w-full flex flex-col p-4 gap-2 border-b">
      <form @submit.prevent="handleSearch" class="flex w-full items-center gap-1.5 relative">
        <Input v-model="searchFilter" type="text" placeholder="Cari" class="w-full bg-white" />
        <Button type="submit" class="absolute right-0 rounded-l-none" size="icon">
          <Icon name="lucide:search" />
        </Button>
      </form>
      <div class="flex md:items-center md:flex-row flex-col gap-2">
        <ReusableGlobalSelectCombobox :items="toComboboxCommandListFriendly(
          accountsResponse?.accounts ?? [],
          'id',
          'name'
        )
          " name="Sumber Akun" :is-loading="accountsFetchLoading"
          @update-value="(selectedValue) => accountIdRef = selectedValue" />

        <ReusableGlobalSelectCombobox :items="toComboboxCommandListFriendly(
          categoriesResponse?.categories ?? [],
          'id',
          'name'
        )
          " name="Kategori" :is-loading="categoriesFetchLoading"
          @update-value="(selectedValue) => categoryIdRef = selectedValue" />

        <ReusableGlobalSelectCombobox :items="toComboboxCommandListFriendly(
          subCategoriesResponse?.subCategories ?? [],
          'id',
          'name'
        )
          " name="Kategori" :is-loading="subCategoriesFetchLoading"
          @update-value="(selectedValue) => subCategoryIdRef = selectedValue" />

        <ReusableGlobalDateRangePicker :date-start-default="dateStartRef?.split('T')[0]"
          :date-end-default="dateEndRef?.split('T')[0]" @date-value-change="(date) => {
            dateStartRef = date.start
              ? castStringDateIntoDotNetDate(
                date.start
                  ?.add({ days: 1 })
                  .toDate(getLocalTimeZone())
                  .toString()
              )
              : undefined;
            dateEndRef = date.end
              ? castStringDateIntoDotNetDate(
                date.end
                  ?.add({ days: 1 })
                  .toDate(getLocalTimeZone())
                  .toString()
              )
              : undefined;
          }
            " />

        <Button class="flex items-center justify-start gap-2 bg-emerald-600 hover:bg-emerald-700">
          <Icon name="lucide:download" />
          <span>Download Laporan</span>
        </Button>
      </div>
    </div>

    <div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead class="pl-4">Deskripsi</TableHead>
            <TableHead>Sumber Akun</TableHead>
            <TableHead>Kategori</TableHead>
            <TableHead>Sub Kategori</TableHead>
            <TableHead>Harga</TableHead>
            <TableHead>Tanggal</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow v-if="transactionFetchLoading">
            <TableCell class="font-medium pl-4 text-nowrap">
              Lagi di cari..
            </TableCell>
          </TableRow>
          <TableRow v-else-if="!transactionResponse?.transactions.length">
            <TableCell class="font-medium pl-4 text-nowrap">
              Data nya kosong / ga ketemu
            </TableCell>
          </TableRow>
          <TableRow v-else v-for="(transaction, idx) in transactionResponse?.transactions" :key="idx">
            <TableCell class="font-medium pl-4 text-nowrap">
              {{ transaction.description }}
            </TableCell>
            <TableCell>
              <Badge variant="secondary" class="w-max text-nowrap bg-emerald-400/20">{{ transaction.account.name }}
              </Badge>
            </TableCell>
            <TableCell class="text-nowrap font-light">{{ transaction.category.name }}</TableCell>
            <TableCell class="text-nowrap font-light">{{ transaction.subCategory?.name || '-' }}</TableCell>
            <TableCell class="text-nowrap font-medium">{{ toIDR(transaction.amount) }}</TableCell>
            <TableCell class="text-nowrap font-medium">
              <Badge variant="secondary" class="w-full">{{ transformDotNetTimestamp(transaction.date).join(', ') }}
              </Badge>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>

    <div class="p-4 space-x-2">
      <Button variant="outline" size="icon" @click="pageNumber = pageNumber - 1"
        :disabled="!transactionResponse?.isPreviousExists">
        <Icon name="lucide:chevron-left" />
      </Button>
      <Button variant="outline" size="icon" @click="pageNumber = pageNumber + 1"
        :disabled="!transactionResponse?.isNextExists">
        <Icon name="lucide:chevron-right" />
      </Button>
    </div>
  </div>
</template>
