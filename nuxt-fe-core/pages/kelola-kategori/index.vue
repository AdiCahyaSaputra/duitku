<script setup lang="ts">
import { getCategories } from '@/services/category.service';

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Kelola Kategori",
});

const pageNumber = ref(1);

const { 
  data, 
  isLoading 
} = useQuery({
  queryKey: ["get_categories", pageNumber],
  queryFn: () => getCategories({ paginate: true, pageNumber: pageNumber.value, limit: 10 }),
  refetchOnMount: 'always'
});
</script>

<template>
  <div class="w-full">
    <div class="p-4 w-full">
      <SectionKelolaKategoriCreateCategory />
    </div>

    <ul class="w-full">
      <ReusableStateLoading :is-loading="isLoading">
        <template #content>
          <li
            class="flex justify-between items-center py-2 px-4 border-y"
            v-for="(category, idx) in data?.categories"
            :key="idx"
          >
            <div>
              <NuxtLink :to="'/kelola-kategori/' + category.id">
                <Button class="text-sm gap-4 items-center flex" size="sm" variant="ghost">
                  <span>{{ category.name }}</span>
                  <Icon name="lucide:square-arrow-out-up-right" class="order-2"/>
                </Button>
              </NuxtLink>
            </div>
            <div class="flex gap-2 items-center">
              <SectionKelolaKategoriEditCategory :category="category" />
              <ReusableKelolaKategoriFormDeleteDialog :id="category.id" />
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
        :disabled="!data?.isPreviousExists"
      >
        <Icon name="lucide:chevron-left" />
      </Button>
      <Button
        variant="outline"
        size="icon"
        @click="pageNumber = pageNumber + 1"
        :disabled="!data?.isNextExists"
      >
        <Icon name="lucide:chevron-right" />
      </Button>
    </div>
  </div>
</template>
