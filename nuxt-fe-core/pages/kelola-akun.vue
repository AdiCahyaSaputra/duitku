<script setup lang="ts">
import { toIDR } from "@/lib/helper";
import { getAccounts } from "@/services/account.service";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: 'Kelola Akun'
});

const pageNumber = ref(1);
const isCreateModalOpen = ref(false);
const isEditModalOpen = ref<string | null>(null);

const { data, isLoading } = useQuery({
  queryKey: ["get_accounts", pageNumber],
  queryFn: () =>
    getAccounts({ paginate: true, limit: 10, pageNumber: pageNumber.value }),
  refetchOnMount: "always",
});
</script>

<template>
  <div class="w-full">
    <div class="p-4 w-full">
      <ReusableKelolaAkunFormDialog 
        title="Buat Akun"
        description="Buat yang sumber income nggak cuma 1, semoga nggak cepet tipes aja sih 😂"
        v-model:is-modal-open="isCreateModalOpen"
      >
        <template #trigger>
          <Button class="flex items-center gap-2 w-full md:w-max justify-start">
            <Icon name="lucide:calculator" />
            <span>Buat Akun</span>
          </Button>
        </template>
        <template #form>
          <ReusableKelolaAkunFormCreate @create-mutate-executed="() => isCreateModalOpen = false" />
        </template>
      </ReusableKelolaAkunFormDialog>
    </div>

    <ul class="w-full">
      <ReusableStateLoading :is-loading="isLoading">
        <template #content>
          <li class="flex justify-between items-center py-2 px-4 border-y" v-for="(account, idx) in data?.accounts"
            :key="idx">
            <div>
              <p class="text-sm">{{ account.name }}</p>
              <p class="font-bold">{{ toIDR(account.balance) }}</p>
            </div>
            <div class="flex gap-2 items-center">
              <ReusableKelolaAkunFormDialog 
                title="Buat Akun"
                description="Buat yang sumber income nggak cuma 1, semoga nggak cepet tipes aja sih 😂"
                :is-modal-open="isEditModalOpen === account.id"
                @update:is-modal-open="(value) => isEditModalOpen = value ? account.id : null"
              >
                <template #trigger>
                  <Button size="icon" variant="outline" @click="isEditModalOpen = account.id">
                    <Icon name="lucide:pen-line" class="w-4 h-4" />
                  </Button>
                </template>
                <template #form>
                  <ReusableKelolaAkunFormEdit 
                    :id="account.id"
                    :name="account.name"
                    :balance="account.balance"
                    @edit-mutate-executed="() => isEditModalOpen = null" 
                  />
                </template>
              </ReusableKelolaAkunFormDialog>
              <ReusableKelolaAkunFormDeleteDialog :id="account.id"/>
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
      <Button variant="outline" size="icon" @click="pageNumber = pageNumber - 1" :disabled="!data?.isPreviousExists">
        <Icon name="lucide:chevron-left" />
      </Button>
      <Button variant="outline" size="icon" @click="pageNumber = pageNumber + 1" :disabled="!data?.isNextExists">
        <Icon name="lucide:chevron-right" />
      </Button>
    </div>
  </div>
</template>
