<script setup lang="ts">
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import {
  castStringDateIntoDotNetDate,
  toComboboxCommandListFriendly,
  toISOLocaleDateString,
} from "@/lib/helper";
import { getAccounts } from "@/services/account.service";
import { getCategories } from "@/services/category.service";
import { getSubCategories } from "@/services/sub-category.service";
import { createTransaction } from "@/services/transaction.service";
import { toTypedSchema } from "@vee-validate/zod";
import { useForm } from "vee-validate";
import { z } from "zod";
import FormDatePicker from "@/components/reusable/beranda/form/FormDatePicker.vue";
import { parseDate } from "@internationalized/date";

const props = defineProps<{
  date: Date; // ISO 8601 
  amount: number;  
}>();

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
    subCategoryId: z.string().nullable().default(null),
    date: z.string({ message: "Tanggal harus di isi" }),
    description: z
      .string({ message: "Deskripsi harus di isi" })
      .trim()
      .min(3, "Deskripsi nya kurang jelas"),
    amount: z
      .number({
        required_error: "Jumlah Harga harus di isi",
        invalid_type_error: "Jumlah Harga nya harus angka",
      })
      .gt(0, "Gak mungkin 0 rupiah bre"),
  }),
);

const form = useForm({
  validationSchema: formSchema,
  initialValues: {
    date: parseDate(toISOLocaleDateString(props.date)).toString(),
    amount: props.amount
  }
});

const { toast } = useToast();
const queryClient = useQueryClient();

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
    queryKey: ["get_sub_categories", form.values.categoryId],
    queryFn: () =>
      getSubCategories({
        paginate: false,
        limit: -1,
        categoryId: form.values.categoryId || "",
      }),
    enabled: computed(() => !!form.values.categoryId),
    refetchOnMount: "always",
  });

const { isPending, mutate: createTransactionMutate } = useMutation({
  mutationKey: ["create_transaction"],
  mutationFn: createTransaction,
  onSuccess: (res) => {
    queryClient.invalidateQueries({
      queryKey: ["get_total_expense"],
      exact: false,
    });
    queryClient.invalidateQueries({
      queryKey: ["get_most_expense"],
      exact: false,
    });
    queryClient.invalidateQueries({
      queryKey: ["get_total_assets"],
      exact: false,
    });
    queryClient.invalidateQueries({ queryKey: ["get_accounts"], exact: false });
    queryClient.invalidateQueries({
      queryKey: ["get_transactions"],
      exact: false,
    });

    form.resetForm();

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
  formData.date = castStringDateIntoDotNetDate(formData.date);

  createTransactionMutate(formData);
});

watch(
  () => props.date,
  (newDate) => {
    const formattedDate = parseDate(toISOLocaleDateString(newDate)).toString();

    form.setFieldValue('date', formattedDate);
  }
)

watch(
  () => props.amount,
  (newAmount) => {
    form.setFieldValue('amount', newAmount);
  }
)

</script>
<template>
  <div>
    <form @submit="onSubmit">
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
          :is-loading="accountsFetchLoading"
        />

        <ReusableGlobalSelectCombobox
          :items="
          toComboboxCommandListFriendly(
            categoriesResponse?.categories ?? [],
            'id',
            'name',
          )
          "
          name="Kategori"
          @update-value="
          (selectedValue) => form.setFieldValue('categoryId', selectedValue)
          "
          :is-loading="categoriesFetchLoading"
        />

        <ReusableGlobalSelectCombobox
          :items="
          toComboboxCommandListFriendly(
            subCategoriesResponse?.subCategories ?? [],
            'id',
            'name',
          )
          "
          name="Sub Kategori"
          @update-value="
          (selectedValue) => form.setFieldValue('subCategoryId', selectedValue)
          "
          :is-loading="subCategoriesFetchLoading"
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

        <!-- Form date Picker -->
        <FormDatePicker
          :date="form.values.date"
          :update-model-value="
          (v) => {
            if (v) {
              form.setFieldValue('date', v.toString());
            } else {
              form.setFieldValue('date', undefined);
            }
          }
          "
        />

        <FormField v-slot="{ componentField }" name="amount">
          <FormItem class="mt-2">
            <FormControl>
              <div class="w-full flex items-center relative">
                <div
                  class="absolute w-max inset-y-1 px-2 flex items-center left-1 text-xs bg-primary rounded-md text-white font-bold select-none"
                >
                  <span> Rp. </span>
                </div>
                <Input
                  type="number"
                  placeholder="Jumlah"
                  class="pl-12"
                  v-bind="componentField"
                />
              </div>
            </FormControl>
            <FormMessage class="mt-2" />
          </FormItem>
        </FormField>
      </div>

      <Button class="md:w-max w-full order-1 mt-4" type="submit" :disabled="isPending">
        <span v-if="isPending">Proses...</span>
        <span v-else>Buat</span>
      </Button>
    </form>
  </div>
</template>
