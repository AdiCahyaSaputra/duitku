<script setup lang="ts">
import { Button } from "@/components/ui/button";
import Calendar from "@/components/ui/calendar/Calendar.vue";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import Popover from "@/components/ui/popover/Popover.vue";
import PopoverContent from "@/components/ui/popover/PopoverContent.vue";
import PopoverTrigger from "@/components/ui/popover/PopoverTrigger.vue";
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import { toComboboxCommandListFriendly } from "@/lib/helper";
import { cn } from "@/lib/utils";
import { getAccounts } from "@/services/akun.service";
import { createTransaction } from "@/services/transaction.service";
import { CalendarDate, DateFormatter, getLocalTimeZone, parseDate, today } from "@internationalized/date";
import { toTypedSchema } from "@vee-validate/zod";
import { useForm } from "vee-validate";
import { z } from "zod";

const frameworks = ref([
  { value: "next.js", label: "Next.js" },
  { value: "sveltekit", label: "SvelteKit" },
  { value: "nuxt", label: "Nuxt" },
  { value: "remix", label: "Remix" },
  { value: "astro", label: "Astro" },
]);

const datePlaceholder = ref();
const dateValue = computed({
  get: () => (form.values.date ? parseDate(form.values.date) : undefined),
  set: (value) => value,
});

const df = new DateFormatter('id-ID', {
  dateStyle: 'long'
});

const formSchema = toTypedSchema(
  z.object({
    accountId: z.string({
      message: "Akun harus di isi",
      required_error: "Akun harus di isi",
    }),
    categoryId: z.string({
      message: "Kategori harus di isi",
      required_error: "Kategori harus di isi",
    }),
    subCategoryId: z.string().nullable(),
    date: z.string({ message: "Tanggal harus di isi" }),
    description: z
      .string({ message: "Deskripsi harus di isi" })
      .trim()
      .min(3, "Deskripsi nya kurang jelas"),
    amount: z.number({ required_error: "Jumlah Harga harus di isi", invalid_type_error: "Jumlah Harga nya harus angka" }).gt(0, "Gak mungkin 0 rupiah bre"),
  }),
);

const form = useForm({
  validationSchema: formSchema,
});

const { toast } = useToast();

const { data: accountsResponse } = useQuery({
  queryKey: ["get_accounts"],
  queryFn: () => getAccounts({ paginate: false, limit: -1 }),
});

const { isPending, mutate: createTransactionMutate } = useMutation({
  mutationFn: createTransaction,
  onSuccess: (res: any) => {
    toast({
      title: "Bisa nih 👌",
      description: res.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    toast({
      title: "Waduh ada error",
      description: err.title,
    });
  },
});

const onSubmit = form.handleSubmit((formData) => {
  console.log(formData);
});
</script>

<template>
  <Dialog>
    <DialogTrigger asChild>
      <Button class="w-full md:flex hidden items-center justify-between">
        <Icon name="lucide:circle-fading-plus" class="w-4 h-4" />
        <span>Buat Transaksi</span>
      </Button>
    </DialogTrigger>
    <DialogTrigger asChild>
      <Button size="icon" class="md:hidden">
        <Icon name="lucide:circle-fading-plus" />
      </Button>
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>Bikin Transaksi Baru</DialogTitle>
      </DialogHeader>

      <form @submit="onSubmit" class="mt-4">
        <div class="space-y-2">
          <ReusableGlobalSelectCombobox
            :items="
              toComboboxCommandListFriendly(
                accountsResponse?.accounts ?? [],
                'id',
                'name',
              )
            "
            name="Sumber Akun"
            @update-value="
              (selectedValue) => form.setFieldValue('accountId', selectedValue)
            "
          />

          <ReusableGlobalSelectCombobox
            :items="frameworks"
            name="Kategori"
            @update-value="
              (selectedValue) => form.setFieldValue('categoryId', selectedValue)
            "
          />

          <ReusableGlobalSelectCombobox
            :items="frameworks"
            name="Sub Kategori"
            @update-value="
              (selectedValue) =>
                form.setFieldValue('subCategoryId', selectedValue)
            "
          />
        </div>

        <div>
          <FormField v-slot="{ componentField }" name="description">
            <FormItem class="mt-2">
              <FormControl>
                <Input
                  type="text"
                  placeholder="Deskripsi singkat.."
                  v-bind="componentField"
                />
              </FormControl>
              <FormMessage class="mt-2" />
            </FormItem>
          </FormField>

          <FormField v-slot="{ componentField }" name="date">
            <FormItem class="mt-2">
              <Popover>
                <PopoverTrigger as-child>
                  <FormControl>
                    <Button
                      variant="outline"
                      :class="
                        cn(
                          'w-full ps-3 text-start font-normal',
                          !dateValue && 'text-muted-foreground',
                        )
                      "
                    >
                      <span>{{
                        dateValue ? df.format(dateValue.toDate(getLocalTimeZone())) : "Tanggal Transaksi"
                      }}</span>
                      <Icon name="lucide:calendar" class="ms-auto h-4 w-4" />
                    </Button>
                    <input hidden />
                  </FormControl>
                </PopoverTrigger>
                <PopoverContent class="w-auto p-0">
                  <Calendar
                    v-model:placeholder="datePlaceholder"
                    v-model="dateValue"
                    calendar-label="Tanggal Transaksi"
                    initial-focus
                    :min-value="new CalendarDate(1900, 1, 1)"
                    :max-value="today(getLocalTimeZone())"
                    @update:model-value="
                      (v) => {
                        if (v) {
                          form.setFieldValue('date', v.toString());
                        } else {
                          form.setFieldValue('date', undefined);
                        }
                      }
                    "
                  />
                </PopoverContent>
              </Popover>
              <FormMessage class="mt-2" />
            </FormItem>
          </FormField>

          <FormField v-slot="{ componentField }" name="amount">
            <FormItem class="mt-2">
              <FormControl>
                <div class="w-full flex items-center relative">
                  <div class="absolute w-max inset-y-1 px-2 flex items-center left-1 text-xs bg-primary rounded-md text-white font-bold select-none">
                    <span>
                      Rp.
                    </span>
                  </div>
                  <Input type="number" placeholder="Jumlah" class="pl-12" v-bind="componentField" />
                </div>
              </FormControl>
              <FormMessage class="mt-2" />
            </FormItem>
          </FormField>
        </div>

        <DialogFooter>
          <Button class="mt-4 md:w-max w-full" type="submit" :disabled="isPending">
            <span v-if="isPending">Proses...</span>
            <span v-else>Buat</span>
          </Button>
        </DialogFooter>
      </form>
    </DialogContent>
  </Dialog>
</template>
