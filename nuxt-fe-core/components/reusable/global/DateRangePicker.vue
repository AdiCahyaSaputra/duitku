<script setup lang="ts">
import { type Ref, ref } from "vue";
import {
  DateFormatter,
  getLocalTimeZone,
  parseDate,
  today,
} from "@internationalized/date";

import type { DateRange } from "radix-vue";
import { RangeCalendar } from "@/components/ui/range-calendar";
import { Button } from "@/components/ui/button";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import { filterTransactionStore } from "@/store/filter-transaction.store";

const emit = defineEmits<{
  (e: 'dateValueChange', date: DateRange): void
}>();

const props = defineProps<{
  dateStartDefault?: string;
  dateEndDefault?: string;
}>()

const df = new DateFormatter("id-ID", {
  dateStyle: "long",
});

const isPopOverOpen = ref(false);
const value = ref({
  start: props.dateStartDefault ? parseDate(props.dateStartDefault) : undefined,
  end: props.dateEndDefault ? parseDate(props.dateEndDefault) : undefined,
}) as Ref<DateRange>;

watch(() => [value.value.start, value.value.end], ([newStart, newEnd]) => {
  const newDate: DateRange = {
    start: newStart?.add({ days: 1 }), // sometimes it bug so i need to add 1 day manually
    end: newEnd?.add({ days: 1 }), // same...
  }

  emit('dateValueChange', newDate);
});

</script>

<template>
  <Popover v-model:open="isPopOverOpen">
    <PopoverTrigger as-child>
      <Button
        variant="outline"
        :class="
          cn(
            'w-full justify-start text-left font-normal',
            !value && 'text-muted-foreground',
          )
        "
      >
        <Icon name="lucide:calendar" class="mr-2 h-4 w-4" />
        <template v-if="value.start">
          <template v-if="value.end">
            {{ df.format(value.start.toDate(getLocalTimeZone())) }} -
            {{ df.format(value.end.toDate(getLocalTimeZone())) }}
          </template>

          <template v-else>
            {{ df.format(value.start.toDate(getLocalTimeZone())) }}
          </template>
        </template>
        <template v-else>
          <p class="text-muted-foreground">Tanggal</p>
        </template>
      </Button>
    </PopoverTrigger>
    <PopoverContent class="w-auto p-0 flex flex-col items-end">
      <RangeCalendar
        v-model="value"
        initial-focus
        :number-of-months="1"
        @update:start-value="(startDate) => (value.start = startDate)"
      />

      <div class="w-full p-2">
        <Button variant="secondary" @click="() => {
          value = {
            start: undefined,
            end: undefined,
          }

          isPopOverOpen = false;
        }" class="w-full">
          Kosongin
        </Button>
      </div>
    </PopoverContent>
  </Popover>
</template>
