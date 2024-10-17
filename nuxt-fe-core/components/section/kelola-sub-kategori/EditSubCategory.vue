<script setup lang="ts">
import FormEdit from "@/components/reusable/kelola-sub-kategori/form/FormEdit.vue";
import type SubCategoryDto from "@/dto/SubCategoryDto";

const props = defineProps<{
  subCategory: SubCategoryDto;
}>();

const isModalOpen = ref<string | null>(null);
</script>

<template>
  <ReusableGlobalFormDialog
    title="Edit Kategori"
    description="Biar kategori nya lebih eye-catching bisa pake Emote di depan nama nya bre"
    :is-modal-open="isModalOpen === props.subCategory.id"
    @update:is-modal-open="
      (value) => (isModalOpen = value ? props.subCategory.id : null)
    "
    :show-mobile-button="false"
  >
    <template #trigger>
      <Button
        size="icon"
        variant="outline"
        @click="isModalOpen = props.subCategory.id"
      >
        <Icon name="lucide:pen-line" class="w-4 h-4" />
      </Button>
    </template>
    <template #form>
      <FormEdit
        :id="props.subCategory.id"
        :name="props.subCategory.name"
        :categoryId="props.subCategory.categoryId"
        @edit-mutate-executed="() => (isModalOpen = null)"
      />
    </template>
  </ReusableGlobalFormDialog>
</template>
