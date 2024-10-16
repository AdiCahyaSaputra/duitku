<script setup lang="ts">
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import FormTransaction from "./FormTransaction.vue";

const isModalOpen = ref(false);
</script>

<template>
  <Dialog v-model:open="isModalOpen">
    <DialogTrigger asChild>
      <Button class="w-full md:flex hidden items-center justify-between">
        <Icon name="lucide:circle-fading-plus" class="w-4 h-4" />
        <span>Buat Transaksi</span>
      </Button>
    </DialogTrigger>
    <DialogTrigger asChild>
      <Button size="icon" class="md:hidden">
        <Icon name="lucide:circle-fading-plus" />
      </Button>
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle class="text-left">
          Buat Transaksi
        </DialogTitle>
        <DialogDescription class="text-left">Catet transaksi disini bre</DialogDescription>
      </DialogHeader>

      <FormTransaction @create-mutate-executed="() => (isModalOpen = false)" />
    </DialogContent>
  </Dialog>
</template>
