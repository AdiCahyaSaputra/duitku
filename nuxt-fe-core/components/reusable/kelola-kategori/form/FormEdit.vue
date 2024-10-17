<script setup lang="ts">
import DialogFooter from "@/components/ui/dialog/DialogFooter.vue";
import { useToast } from "@/components/ui/toast";
import type AccountDto from "@/dto/AccountDto";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import type CategoryDto from "@/dto/CategoryDto";
import { editAccount } from "@/services/account.service";
import { editCategory } from "@/services/category.service";
import { toTypedSchema } from "@vee-validate/zod";
import { useForm } from "vee-validate";
import { z } from "zod";

const emit = defineEmits<{
  (e: "editMutateExecuted"): void;
}>();

const props = defineProps<{
  id: string;
  name: string;
}>();

const inputItems: { name: string; placeholder: string; type: string }[] = [
  {
    name: "name",
    placeholder: "Nama Akun",
    type: "text",
  },
];

const formSchema = toTypedSchema(
  z.object({
    name: z.string({
      message: "Nama Akun harus di isi",
      required_error: "Nama Akun harus di isi",
    }),
  }),
);

const form = useForm({
  validationSchema: formSchema,
  initialValues: {
    name: props.name,
  },
});

const { toast } = useToast();

const queryClient = useQueryClient();

const { isPending, mutate: editCategoryMutate } = useMutation({
  mutationKey: ["edit_account", props.id],
  mutationFn: ({
    formData,
    id,
  }: {
    formData: Pick<CategoryDto, "name">;
    id: string;
  }) => editCategory(formData, id),
  onSuccess: (res) => {
    queryClient.invalidateQueries({ queryKey: ["get_total_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_most_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_total_assets"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_categories"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_accounts"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_transactions"], exact: false });

    form.resetForm();

    emit("editMutateExecuted");

    toast({
      title: "Bisa nih 👌",
      description: res?.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    emit("editMutateExecuted");

    toast({
      title: "Waduh ada error",
      description: err.title,
    });
  },
});

const onSubmit = form.handleSubmit((formData) => {
  editCategoryMutate({
    formData,
    id: props.id,
  });
});
</script>

<template>
  <form @submit="onSubmit">
    <div v-for="(inputItem, idx) in inputItems" :key="idx">
      <FormField v-slot="{ componentField }" :name="inputItem.name">
        <FormItem class="mt-2">
          <FormControl>
            <Input :type="inputItem.type" :placeholder="inputItem.placeholder" v-bind="componentField" />
          </FormControl>
          <FormMessage class="mt-2" />
        </FormItem>
      </FormField>
    </div>

    <DialogFooter>
      <Button class="mt-4 md:w-max w-full" type="submit" :disabled="isPending">
        <span v-if="isPending">Proses...</span>
        <span v-else>Edit</span>
      </Button>
    </DialogFooter>
  </form>
</template>
