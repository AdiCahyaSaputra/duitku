<script setup lang="ts">
import { getCategoryById } from "@/services/category.service";
import { getSubCategories } from "@/services/sub-category.service";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Detail Kategori",
});

const pageNumber = ref(1);
const route = useRoute();

const { data: categoryByIdResponse, isLoading: categoryByIdFetchLoading } =
  useQuery({
    queryKey: ["get_category_by_id", route.params.id],
    queryFn: () => getCategoryById(route.params.id as string),
    refetchOnMount: "always",
  });

const { data: subCategoriesResponse, isLoading: subCategoriesFetchLoading } =
  useQuery({
    queryKey: ["get_sub_categories", route.params.id],
    queryFn: () =>
      getSubCategories({
        paginate: true,
        pageNumber: pageNumber.value,
        limit: 10,
        categoryId: route.params.id as string,
      }),
    refetchOnMount: "always",
  });
</script>

<template>
  <div class="w-full">
    <div class="flex gap-2 p-4 items-start border-b w-full">
      <NuxtLink to="/kelola-kategori">
        <Button size="icon" variant="outline">
          <Icon name="lucide:chevron-left"/>
        </Button>
      </NuxtLink>
      <div>
        <h1 class="text-base font-bold">Detail Kategori</h1>
        <p class="text-xs">{{ categoryByIdResponse?.category.name }}</p>
      </div>
    </div>

    <div class="p-4 w-full">
      <SectionKelolaSubKategoriCreateSubCategory />
    </div>

    <ul class="w-full">
      <ReusableStateLoading :is-loading="subCategoriesFetchLoading">
        <template #content>
          <li
            class="flex justify-between items-center py-2 px-4 border-y"
            v-for="(subCategory, idx) in subCategoriesResponse?.subCategories"
            :key="idx"
          >
            <div>
              <p class="text-sm">{{ subCategory.name }}</p>
            </div>
            <div class="flex gap-2 items-center">
              <SectionKelolaSubKategoriEditSubCategory :sub-category="subCategory" />
              <ReusableKelolaSubKategoriFormDeleteDialog :id="subCategory.id" />
            </div>
          </li>
        </template>

        <template #loadingFallback>
          <li class="flex justify-between items-center py-2 px-4 border-y">
            <p>Lagi dicari..</p>
          </li>
        </template>
      </ReusableStateLoading>
    </ul>

    <div class="p-4 space-x-2">
      <Button
        variant="outline"
        size="icon"
        @click="pageNumber = pageNumber - 1"
        :disabled="!subCategoriesResponse?.isPreviousExists"
      >
        <Icon name="lucide:chevron-left" />
      </Button>
      <Button
        variant="outline"
        size="icon"
        @click="pageNumber = pageNumber + 1"
        :disabled="!subCategoriesResponse?.isNextExists"
      >
        <Icon name="lucide:chevron-right" />
      </Button>
    </div>
  </div>
</template>
