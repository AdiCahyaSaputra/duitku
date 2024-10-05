<script setup lang="ts">
  const wellcomeMessage = "Hello ";
</script>

<template>
  <button style="background-color: blue; color: white;">
    {{ wellcomeMessage }} <slot />
  </button>
</template>
