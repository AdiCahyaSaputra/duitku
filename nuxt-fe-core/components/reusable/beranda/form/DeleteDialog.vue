<script setup lang="ts">
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
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
    isOpen.value = false

    queryClient.invalidateQueries({ queryKey: ['get_transactions'], exact: false });

    toast({
      title: "Notifikasi",
      description: res.message,
    });
  },
  onError: (err: ApiErrorDto) => {
    isOpen.value = false

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
  <AlertDialog :open="isOpen">
    <AlertDialogTrigger asChild>
      <Button @click="isOpen = true" class="w-max" variant="destructive" size="sm">
        <Icon name="lucide:trash-2" class="w-4 h-4" />
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent>
      <AlertDialogHeader>
        <AlertDialogTitle>Hapus Transaksi Ini?</AlertDialogTitle>
        <AlertDialogDescription>
          Jika transaksi ini dihapus maka saldo nya akan di refund
        </AlertDialogDescription>
      </AlertDialogHeader>
      <AlertDialogFooter>
        <AlertDialogCancel @click="isOpen = false">Ga Jadi 🙏</AlertDialogCancel>
        <AlertDialogAction
          @click="handleDelete"
          class="bg-destructive hover:bg-destructive/90"
          :disabled="isPending"
        >
          {{ isPending ? 'Lagi proses..' : 'Iya Hapus 👌' }}
        </AlertDialogAction>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>
