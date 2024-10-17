<script setup lang="ts">
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import { deleteAccount } from "@/services/account.service";

const props = defineProps<{
  id: string;
}>();

const isOpen = ref(false);

const { toast } = useToast();
const queryClient = useQueryClient();

const { mutate: deleteAccountMutate, isPending } = useMutation({
  mutationKey: ["delete_account", props.id],
  mutationFn: async (id: string) => await deleteAccount(id),
  onSuccess: (res) => {
    isOpen.value = false;

    queryClient.invalidateQueries({ queryKey: ["get_total_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_most_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_total_assets"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_accounts"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_transactions"], exact: false });

    toast({
      title: "Notifikasi",
      description: res?.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    isOpen.value = false;

    toast({
      title: "Waduh Error nih",
      description: err.title,
    });
  }
});

const handleDelete = async () => {
  deleteAccountMutate(props.id);
};
</script>

<template>
  <ReusableGlobalBaseDeleteDialog
    title="Hapus Akun Ini?"
    description="Jika akun ini dihapus maka transaksi nya bakal otomatis ke hapus juga"
    v-model:is-open="isOpen"
    :is-pending="isPending"
    @handle-delete="handleDelete"
  >
    <template #trigger>
      <Button @click="isOpen = true" variant="destructive" size="icon">
        <Icon name="lucide:trash-2" class="w-4 h-4" />
      </Button>
    </template>
  </ReusableGlobalBaseDeleteDialog>
</template>
