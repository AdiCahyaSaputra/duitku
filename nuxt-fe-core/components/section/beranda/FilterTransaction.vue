<script setup lang="ts">
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from '@/components/ui/sheet';
import { castStringDateIntoDotNetDate, toComboboxCommandListFriendly } from "@/lib/helper";
import { getAccounts } from "@/services/account.service";
import { getCategories } from "@/services/category.service";
import { getSubCategories } from "@/services/sub-category.service";
import { filterTransactionStore } from '@/store/filter-transaction.store';
import { getLocalTimeZone } from "@internationalized/date";

const accountIdRef = ref<string | undefined>(undefined);
const categoryIdRef = ref<string | undefined>(undefined);
const subCategoryIdRef = ref<string | undefined>(undefined);
const dateStartRef = ref<string | undefined>(undefined);
const dateEndRef = ref<string | undefined>(undefined);

const isSheetOpen = ref(false);

const { setFilter } = filterTransactionStore();

const { data: accountsResponse, isLoading: accountsFetchLoading } = useQuery({
  queryKey: ["get_accounts"],
  queryFn: () => getAccounts({ paginate: false, limit: -1 }),
  refetchOnMount: 'always'
});

const { data: categoriesResponse, isLoading: categoriesFetchLoading } =
  useQuery({
    queryKey: ["get_categories"],
    queryFn: () => getCategories({ paginate: false, limit: -1 }),
    refetchOnMount: 'always'
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
    refetchOnMount: 'always'
  });

const handleApplyFilter = () => {
  setFilter({
    accountId: accountIdRef.value,
    categoryId: categoryIdRef.value,
    subCategoryId: subCategoryIdRef.value,
    dateStart: dateStartRef.value,
    dateEnd: dateEndRef.value
  });

  isSheetOpen.value = false;
}

filterTransactionStore().$subscribe((_, state) => {
  accountIdRef.value = state.accountId;
  categoryIdRef.value = state.categoryId;
  subCategoryIdRef.value = state.subCategoryId;
  dateStartRef.value = state.dateStart;
  dateEndRef.value = state.dateEnd;
});
</script>

<template>
  <Sheet v-model:open="isSheetOpen">
    <SheetTrigger asChild>
      <Button variant="outline" size="icon">
        <Icon name="lucide:filter" />
      </Button>
    </SheetTrigger>
    <SheetContent>
      <SheetHeader>
        <SheetTitle class="text-left">Filter Transaksi</SheetTitle>
        <SheetDescription class="text-left">Data yang tampil masih bisa di filter disini bre, jadi jangan manual cari data nya</SheetDescription>
      </SheetHeader>

      <div class="space-y-2 mt-4">
        <ReusableGlobalSelectCombobox
          :items="
            toComboboxCommandListFriendly(
              accountsResponse?.accounts ?? [],
              'id',
              'name',
            )
          "
          :default-selected-value="accountIdRef"
          :is-loading="accountsFetchLoading"
          name="Sumber Akun"
          @update-value="
            (selectedValue) => {
              accountIdRef = selectedValue
            }
          "
        />

        <ReusableGlobalSelectCombobox
          :items="
            toComboboxCommandListFriendly(
              categoriesResponse?.categories ?? [],
              'id',
              'name',
            )
          "
          :default-selected-value="categoryIdRef"
          :is-loading="categoriesFetchLoading"
          name="Kategori"
          @update-value="
            (selectedValue) => {
              categoryIdRef = selectedValue
            } 
          "
        />

        <ReusableGlobalSelectCombobox
          :items="
            toComboboxCommandListFriendly(
              subCategoriesResponse?.subCategories ?? [],
              'id',
              'name',
            )
          "
          :default-selected-value="subCategoryIdRef"
          :is-loading="subCategoriesFetchLoading"
          name="Sub Kategori"
          @update-value="
            (selectedValue) => {
              subCategoryIdRef = selectedValue
            } 
          "
        />

        <ReusableGlobalDateRangePicker 
          :date-start-default="dateStartRef?.split('T')[0]"
          :date-end-default="dateEndRef?.split('T')[0]"
          @date-value-change="(date) => {
            dateStartRef = date.start ? castStringDateIntoDotNetDate(date.start?.add({ days: 1 }).toDate(getLocalTimeZone()).toString()) : undefined;
            dateEndRef = date.end ? castStringDateIntoDotNetDate(date.end?.add({ days: 1 }).toDate(getLocalTimeZone()).toString()) : undefined;
          }"
        />

        <Button @click="handleApplyFilter" class="w-full">Terapin Filter</Button>
      </div>
    </SheetContent>
  </Sheet>
</template>
