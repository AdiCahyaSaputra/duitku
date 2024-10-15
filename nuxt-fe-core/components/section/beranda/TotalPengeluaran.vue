<script setup lang="ts">
import {
  castStringDateIntoDotNetDate,
  getCurrentDate,
  toIDR,
} from "@/lib/helper";
import { getMostExpensiveTransactions, getTotalExpense } from "@/services/transaction.service";
import { getLocalTimeZone } from "@internationalized/date";
import { useDateFormat } from "@vueuse/core";

const { month, year } = getCurrentDate();

const dateStart = ref(new Date(`${year}-${month}-01`));
const dateEnd = ref(new Date(year, month, 0));

const dateStartFormatted = computed(() =>
  useDateFormat(dateStart.value, "DD MMMM", {
    locales: "id-ID",
  })
);

const dateEndFormatted = computed(() =>
  useDateFormat(dateEnd.value, "DD MMMM", {
    locales: "id-ID",
  })
);

const { data: totalExpenseResponse, isLoading: totalExpenseFetchLoading } =
  useQuery({
    queryKey: ["get_total_expense", dateStart, dateEnd],
    queryFn: async () =>
      await getTotalExpense({
        dateStart: castStringDateIntoDotNetDate(dateStart.value.toString()),
        dateEnd: castStringDateIntoDotNetDate(dateEnd.value.toString()),
      }),
  });

const { data: mostExpensiveResponse, isLoading: mostExpensiveFetchLoading } = useQuery({
  queryKey: ["get_most_expense", dateStart, dateEnd],
  queryFn: async () =>
    await getMostExpensiveTransactions({
      dateStart: castStringDateIntoDotNetDate(dateStart.value.toString()),
      dateEnd: castStringDateIntoDotNetDate(dateEnd.value.toString()),
    }),
});
</script>

<template>
  <div class="text-left flex flex-col lg:border-l lg:border-t-none border-t">
    <div class="p-4">
      <div class="flex justify-between items-start gap-2 w-full">
        <div>
          <h2 class="text-base font-bold text-black">Total Pengeluaran Kamu</h2>
          <p class="text-sm text-gray-600">
            {{ dateStartFormatted }} - {{ dateEndFormatted }}
          </p>
        </div>
        <ReusableGlobalDateRangePicker
          :dateStartDefault="dateStart.toISOString().split('T')[0]"
          :dateEndDefault="dateEnd.toISOString().split('T')[0]"
          @dateValueChange="
            (date) => {
              dateStart = date.start
                ? date.start?.toDate(getLocalTimeZone())
                : new Date(`${year}-${month}-01`);
              dateEnd = date.end
                ? date.end?.toDate(getLocalTimeZone())
                : new Date(year, month, 0);
            }
          "
        >
          <Button variant="outline" size="icon">
            <Icon name="lucide:calendar-range" class="w-4 h-4" />
          </Button>
        </ReusableGlobalDateRangePicker>
      </div>
      <ReusableStateLoading :is-loading="totalExpenseFetchLoading">
        <template #content>
          <h1 class="text-lg font-bold mt-2">
            {{ toIDR(totalExpenseResponse?.totalExpense || 0) }}
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

    <p class="text-sm flex items-center gap-2 p-4 pb-0 border-t">
      <span class="text-black font-bold">Top 3 yang ngabisin duit mu 😁</span>
    </p>

    <ul class="p-4 pt-0 text-sm mt-2">
      <li v-for="(transaction, idx) in mostExpensiveResponse?.transactions" :key="idx">
        {{ idx + 1 }}. {{ transaction.description }}
      </li>
    </ul>
  </div>
</template>
