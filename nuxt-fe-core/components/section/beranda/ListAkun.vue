<script setup lang="ts">
import Badge from '@/components/ui/badge/Badge.vue';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

const data = ref([]);
</script>

<template>
  <Card class="select-none">
    <CardHeader class="space-y-3">
      <CardDescription class="gap-1 flex flex-col">
        <Badge class="px-2 w-max">🍜 Jajan/Makanan</Badge>
        <Badge variant="secondary" class="px-2 w-max">🐔 Fried Chicken</Badge>
      </CardDescription>
      <CardTitle class="leading-5">Ini abis beli makanan di mall karena laper wkwkkw</CardTitle>
    </CardHeader>
    <CardContent class="font-bold text-right py-0">Rp. 24.000</CardContent>
    <CardFooter class="flex flex-col items-end gap-2 mt-4">
      <div class="text-sm text-gray-700 flex justify-between items-center w-full">
        <span>Senin</span>
        <span >13/10/2024</span>
      </div>
      <Button class="flex items-center justify-between gap-2 w-full" size="sm">
        <span>Bank DKI</span>
        <Icon name="lucide:arrow-up-right" class="w-4 h-4"/>
      </Button>
    </CardFooter>
  </Card>
</template>
