<script setup lang="ts">
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import { CalendarDate, DateFormatter, getLocalTimeZone, parseDate, today, type DateValue } from "@internationalized/date";

const props = defineProps<{
  date?: string;
  updateModelValue(v?: DateValue): void;
}>();

const datePlaceholder = ref();
const dateValue = computed({
  get: () => (props.date ? parseDate(props.date) : undefined),
  set: (value) => value,
});

const df = new DateFormatter("id-ID", {
  dateStyle: "long",
});

</script>

<template>
  <FormField v-slot="{ componentField }" name="date">
    <FormItem class="mt-2">
      <Popover>
        <PopoverTrigger as-child>
          <FormControl>
            <Button variant="outline" :class="cn(
              'w-full ps-3 text-start font-normal',
              !dateValue && 'text-muted-foreground',
            )
              ">
              <span>{{
                dateValue
                  ? df.format(dateValue.toDate(getLocalTimeZone()))
                  : "Tanggal Transaksi"
              }}</span>
              <Icon name="lucide:calendar" class="ms-auto h-4 w-4" />
            </Button>
            <input v-bind="componentField" hidden />
          </FormControl>
        </PopoverTrigger>
        <PopoverContent class="w-auto p-0">
          <Calendar v-model:placeholder="datePlaceholder" v-model="dateValue" calendar-label="Tanggal Transaksi"
            initial-focus :min-value="new CalendarDate(1900, 1, 1)" :max-value="today(getLocalTimeZone())"
            @update:model-value="props.updateModelValue" />
        </PopoverContent>
      </Popover>
      <FormMessage class="mt-2" />
    </FormItem>
  </FormField>
</template>
