<script setup lang="ts">
import { useUser } from "@/composables/use-user";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "Beranda",
});

const { user, revokeAuthToken } = useUser();
</script>

<template>
  <div>
    <h1>Hi {{ user?.name }}</h1>
    <Button @click="revokeAuthToken">Logout</Button>
  </div>
</template>
