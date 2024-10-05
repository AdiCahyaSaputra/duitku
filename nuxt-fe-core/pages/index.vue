<script setup lang="ts">
  const name: string = "Adi Cahya Saputra";

  definePageMeta({
    layout: "main-default"
  })

</script>

<template>
  <div>
    <h1>{{ name }}</h1>

    <Button>Ini Button Shadcn/UI</Button>
  </div>
</template>
