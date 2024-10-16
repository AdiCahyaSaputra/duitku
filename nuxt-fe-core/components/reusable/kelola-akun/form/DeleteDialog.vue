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
    isOpen.value = false

    queryClient.invalidateQueries({
      queryKey: ["get_transactions"],
      exact: false,
    });

    queryClient.invalidateQueries({
      queryKey: ["get_total_assets"],
      exact: false,
    });

    queryClient.invalidateQueries({
      queryKey: ["get_total_expense"],
      exact: false,
    });

    queryClient.invalidateQueries({
      queryKey: ["get_accounts"],
      exact: false,
    });

    toast({
      title: "Notifikasi",
      description: res?.message,
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
  deleteAccountMutate(props.id);
};
</script>

<template>
  <AlertDialog :open="isOpen">
    <AlertDialogTrigger asChild>
      <Button @click="isOpen = true" variant="destructive" size="icon">
        <Icon name="lucide:trash-2" class="w-4 h-4" />
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent>
      <AlertDialogHeader>
        <AlertDialogTitle>Hapus Akun Ini?</AlertDialogTitle>
        <AlertDialogDescription>
          Jika akun ini dihapus maka transaksi nya bakal otomatis ke hapus juga
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
