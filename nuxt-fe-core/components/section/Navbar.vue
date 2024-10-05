<script setup lang="ts">
  interface NavItem {
    name: string;
    url: string;
  }

  const navItems: NavItem[] = [
    {
      name: 'Beranda',
      url: '/',
    },
    {
      name: 'Profil',
      url: '/profil',
    }
  ]
</script>

<template>
  <nav>
    <ul class="flex gap-4">
      <li v-for="(navItem, idx) in navItems" :key="idx">
        <NuxtLink :to="navItem.url">{{ navItem.name }}</NuxtLink>
      </li>
    </ul>
  </nav>
</template>
