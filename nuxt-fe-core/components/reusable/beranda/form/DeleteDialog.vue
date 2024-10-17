<script setup lang="ts">
import { useToast } from "@/components/ui/toast";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";
import { deleteTransaction } from "@/services/transaction.service";

const props = defineProps<{
  id: string;
}>();

const isOpen = ref(false);

const { toast } = useToast();
const queryClient = useQueryClient();

const { mutate: deleteTransactionMutate, isPending } = useMutation({
  mutationKey: ["delete_transaction", props.id],
  mutationFn: async (id: string) => await deleteTransaction(id),
  onSuccess: (res) => {
    isOpen.value = false;

    queryClient.invalidateQueries({ queryKey: ["get_total_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_most_expense"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_total_assets"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_accounts"], exact: false });
    queryClient.invalidateQueries({ queryKey: ["get_transactions"], exact: false });

    toast({
      title: "Notifikasi",
      description: res.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    isOpen.value = false;

    toast({
      title: "Waduh Error",
      description: err.title,
    });
  },
});

const handleDelete = () => {
  deleteTransactionMutate(props.id);
};
</script>

<template>
  <ReusableGlobalBaseDeleteDialog
    title="Hapus Transaksi Ini?"
    description="Jika transaksi ini dihapus maka saldo nya akan di refund"
    :is-pending="isPending"
    v-model:is-open="isOpen"
    @handle-delete="handleDelete"
  >
    <template #trigger>
      <Button
        @click="isOpen = true"
        class="w-max"
        variant="destructive"
        size="sm"
      >
        <Icon name="lucide:trash-2" class="w-4 h-4" />
      </Button>
    </template>
  </ReusableGlobalBaseDeleteDialog>
</template>
