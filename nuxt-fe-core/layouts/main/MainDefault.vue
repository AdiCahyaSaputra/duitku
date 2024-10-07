<script setup lang="ts">
const headerHeight = ref(0);
const open = ref(false);

const toggleOpen = (openState: boolean) => {
  open.value = !openState;
};

const updateHeaderHeight = (height: number) => {
  headerHeight.value = height;
};
</script>

<template>
  <div class="selection:bg-green-700 selection:text-white relative">
    <SectionMainHeader @updateHeaderHeight="updateHeaderHeight" />
    <main class="flex relative">
      <SectionMainSideBar :open="open" :headerHeight="headerHeight" />
      <ReusableAsideOpenMenu :open="open" @toggleOpen="toggleOpen" />
      <slot />
    </main>
  </div>
</template>
