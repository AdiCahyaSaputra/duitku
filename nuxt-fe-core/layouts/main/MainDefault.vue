<script setup lang="ts">
import { headerStore } from "@/store/header.store";

const { setHeaderHeight } = headerStore();

const headerHeight = ref(0);
const open = ref(false);

const toggleOpen = (openState: boolean) => {
  open.value = !openState;
};

const updateHeaderHeight = (height: number) => {
  headerHeight.value = height;
  setHeaderHeight(height);
};
</script>

<template>
  <div class="relative">
    <SectionMainHeader @update-header-height="updateHeaderHeight" />
    <main class="flex relative">
      <SectionMainSideBar :open="open" :headerHeight="headerHeight" @toggle-open="toggleOpen" />
      <ReusableAsideOpenMenu :open="open" @toggle-open="toggleOpen" />
      <slot />
    </main>
  </div>
</template>
