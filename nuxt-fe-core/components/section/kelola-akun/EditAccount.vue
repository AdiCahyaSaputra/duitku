<script setup lang="ts">
import FormEdit from "@/components/reusable/kelola-akun/form/FormEdit.vue";
import type AccountDto from "@/dto/AccountDto";

const props = defineProps<{
  account: AccountDto;
}>();

const isModalOpen = ref<string | null>(null);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Edit Akun"
    description="Buat yang sumber income nggak cuma 1, semoga nggak cepet tipes aja sih 😂"
    :is-modal-open="isModalOpen === props.account.id"
    @update:is-modal-open="
      (value) => (isModalOpen = value ? props.account.id : null)
    "
  >
    <template #trigger>
      <Button
        size="icon"
        variant="outline"
        @click="isModalOpen = props.account.id"
      >
        <Icon name="lucide:pen-line" class="w-4 h-4" />
      </Button>
    </template>
    <template #form>
      <FormEdit
        :id="props.account.id"
        :name="props.account.name"
        :balance="props.account.balance"
        @edit-mutate-executed="() => (isModalOpen = null)"
      />
    </template>
  </ReusableGlobalFormDialog>
</template>
