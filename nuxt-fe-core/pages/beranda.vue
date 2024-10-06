<script setup lang="ts">
import { useUser } from "@/composables/use-user";

const { user, revokeAuthToken, isLoading } = useUser();
</script>

<template>
  <div>
    susp
    <h1>{{ user?.name }}</h1>

    <Button @click="revokeAuthToken">Logout</Button>
  </div>
</template>
