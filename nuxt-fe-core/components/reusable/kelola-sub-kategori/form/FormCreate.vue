<script setup lang="ts">
import DialogFooter from "@/components/ui/dialog/DialogFooter.vue";
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import { createSubCategory } from "@/services/sub-category.service";
import { toTypedSchema } from "@vee-validate/zod";
import { useForm } from "vee-validate";
import { z } from "zod";

const emit = defineEmits<{
  (e: "createMutateExecuted"): void;
}>();

const inputItems: { name: string; placeholder: string; type: string }[] = [
  {
    name: "name",
    placeholder: "Nama Sub Kategori",
    type: "text",
  },
];

const route = useRoute();

const formSchema = toTypedSchema(
  z.object({
    name: z.string({
      message: "Nama Sub Kategori harus di isi",
      required_error: "Nama Sub Kategori harus di isi",
    }),
    categoryId: z.string()
  }),
);

const form = useForm({
  validationSchema: formSchema,
  initialValues: {
    categoryId: route.params.id as string
  }
});

const { toast } = useToast();
const queryClient = useQueryClient();

const { isPending, mutate: createSubCategoryMutate } = useMutation({
  mutationKey: ["create_sub_category"],
  mutationFn: createSubCategory,
  onSuccess: (res) => {
    queryClient.invalidateQueries({ queryKey: ["get_sub_categories"], exact: false });

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
  createSubCategoryMutate(formData);
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
        <span v-else>Buat</span>
      </Button>
    </DialogFooter>
  </form>
</template>
