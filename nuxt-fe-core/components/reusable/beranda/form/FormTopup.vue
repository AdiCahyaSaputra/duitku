<script setup lang="ts">
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import {
  castStringDateIntoDotNetDate,
  toComboboxCommandListFriendly,
} from "@/lib/helper";
import { getAccounts, topUpBalance } from "@/services/account.service";
import { getCategories } from "@/services/category.service";
import { getSubCategories } from "@/services/sub-category.service";
import { createTransaction } from "@/services/transaction.service";
import { toTypedSchema } from "@vee-validate/zod";
import { useForm } from "vee-validate";
import { z } from "zod";
import FormDatePicker from "./FormDatePicker.vue";
import type AccountDto from "@/dto/AccountDto";

const emit = defineEmits(["createMutateExecuted"]);

const formSchema = toTypedSchema(
  z.object({
    id: z.string({
      message: "Akun harus di isi",
      required_error: "Akun harus di isi",
    }),
    balance: z
      .number({
        required_error: "Jumlah Saldo harus di isi",
        invalid_type_error: "Jumlah Saldo nya harus angka",
      })
      .gt(0, "Gak mungkin 0 rupiah bre"),
  }),
);

const form = useForm({
  validationSchema: formSchema,
});

const { toast } = useToast();
const queryClient = useQueryClient();

const { 
  data: accountsResponse, 
  isLoading: accountsFetchLoading 
} = useQuery({
  queryKey: ["get_accounts"],
  queryFn: () => getAccounts({ paginate: false, limit: -1 }),
  refetchOnMount: 'always'
});

const { isPending, mutate: topUpBalanceMutate } = useMutation({
  mutationKey: ["create_transaction"],
  mutationFn: (args: { formData: Pick<AccountDto, "id" | "balance">, id: string }) => topUpBalance(args.formData, args.id),
  onSuccess: (res) => {
    queryClient.invalidateQueries({ queryKey: ["get_total_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_most_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_total_assets"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_accounts"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_transactions"], exact: false });

    form.resetForm();

    emit("createMutateExecuted");

    toast({
      title: "Bisa nih 👌",
      description: res?.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    emit("createMutateExecuted");

    toast({
      title: "Waduh ada error",
      description: err.title,
    });
  },
});

const onSubmit = form.handleSubmit((formData) => {
  topUpBalanceMutate({
    formData,
    id: formData.id
  });
});

</script>
<template>
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
          (selectedValue) => form.setFieldValue('id', selectedValue)
        "
        :is-loading="accountsFetchLoading"
      />
    </div>

    <div>
      <FormField v-slot="{ componentField }" name="balance">
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
                placeholder="Saldo"
                class="pl-12"
                v-bind="componentField"
              />
            </div>
          </FormControl>
          <FormMessage class="mt-2" />
        </FormItem>
      </FormField>
    </div>

    <DialogFooter>
      <Button class="mt-4 md:w-max w-full" type="submit" :disabled="isPending">
        <span v-if="isPending">Proses...</span>
        <span v-else>Isi Saldo</span>
      </Button>
    </DialogFooter>
  </form>
</template>
