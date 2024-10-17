<script setup lang="ts">
import FormEdit from "@/components/reusable/kelola-kategori/form/FormEdit.vue";
import type CategoryDto from "@/dto/CategoryDto";

const props = defineProps<{
  category: CategoryDto;
}>();

const isModalOpen = ref<string | null>(null);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Edit Kategori"
    description="Biar kategori nya lebih eye-catching bisa pake Emote di depan nama nya bre"
    :is-modal-open="isModalOpen === props.category.id"
    @update:is-modal-open="
      (value) => (isModalOpen = value ? props.category.id : null)
    "
  >
    <template #trigger>
      <Button
        size="icon"
        variant="outline"
        @click="isModalOpen = props.category.id"
      >
        <Icon name="lucide:pen-line" class="w-4 h-4" />
      </Button>
    </template>
    <template #form>
      <FormEdit
        :id="props.category.id"
        :name="props.category.name"
        @edit-mutate-executed="() => (isModalOpen = null)"
      />
    </template>
  </ReusableGlobalFormDialog>
</template>
