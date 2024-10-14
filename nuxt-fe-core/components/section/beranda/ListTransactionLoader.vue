<script setup lang="ts">
import Card from "@/components/ui/card/Card.vue";
import CardContent from "@/components/ui/card/CardContent.vue";
import CardFooter from "@/components/ui/card/CardFooter.vue";
import CardHeader from "@/components/ui/card/CardHeader.vue";
import Skeleton from "@/components/ui/skeleton/Skeleton.vue";
</script>

<template>
  <div class="grid lg:grid-cols-4 md:grid-cols-2 grid-cols-1 gap-4 animate-pulse">
    <Card class="select-none" v-for="(_, idx) in [...'aaaa']" :key="idx">
      <CardHeader class="space-y-3">
        <CardDescription class="gap-1 flex flex-col">
          <Skeleton class="w-full bg-primary py-2" />
          <Skeleton class="w-1/2 py-2" />
        </CardDescription>
        <CardTitle class="leading-5">
          <Skeleton class="h-16 w-full" />
        </CardTitle>
      </CardHeader>
      <CardContent class="font-bold flex justify-end py-0">
        <Skeleton class="w-20 h-6 bg-primary" />
      </CardContent>
      <CardFooter class="flex flex-col items-end gap-2 mt-4">
        <div class="text-sm text-gray-700 flex justify-between items-center w-full">
          <Skeleton class="py-2 w-8" />
          <Skeleton class="py-2 w-12" />
        </div>
        <Skeleton class="py-4 w-full bg-primary" />
      </CardFooter>
    </Card>
  </div>
</template>
