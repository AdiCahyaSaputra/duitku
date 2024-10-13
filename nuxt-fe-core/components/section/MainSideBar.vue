<script setup lang="ts">
const props = defineProps<{
  headerHeight: number;
  open: boolean;
}>();

const navbarItems = {
  mainContent: [
    {
      url: "/beranda",
      label: "Beranda",
      iconName: "lucide:wallet",
    },
  ],
  dashboardContent: [
    {
      url: "/akun",
      label: "Kelola Akun",
      iconName: "lucide:calculator",
    },
    {
      url: "/kategori",
      label: "Kelola Kategori",
      iconName: "lucide:book-minus",
    },
  ],
  reportContent: [
    {
      url: "/laporan",
      label: "Laporan",
      iconName: "lucide:table-2",
    },
  ],
};
</script>

<template>
  <aside
    class="lg:pl-[2rem] lg:border-r lg:pr-4 py-4 flex flex-col justify-between lg:w-1/5 lg:sticky lg:px-0 lg:z-10 px-4 bg-secondary lg:bg-transparent lg:translate-y-0 transition-transform fixed inset-0 z-20"
    :class="open ? 'translate-y-0' : '-translate-y-[200%]'"
    :style="{ 
      top: `${props.headerHeight}px`,
      height: `calc(100vh - ${props.headerHeight}px)`
    }"
  >
    <div>
      <ul class="space-y-2 mt-2">
        <li v-for="(item, idx) in navbarItems.mainContent" :key="idx">
          <ReusableAsideNavItem
            :url="item.url"
            :label="item.label"
            :iconName="item.iconName"
          />
        </li>

        <li v-for="(item, idx) in navbarItems.dashboardContent" :key="idx">
          <ReusableAsideNavItem
            :url="item.url"
            :label="item.label"
            :iconName="item.iconName"
          />
        </li>

        <li v-for="(item, idx) in navbarItems.reportContent" :key="idx">
          <ReusableAsideNavItem
            :url="item.url"
            :label="item.label"
            :iconName="item.iconName"
          />
        </li>
      </ul>
    </div>
  </aside>
</template>
