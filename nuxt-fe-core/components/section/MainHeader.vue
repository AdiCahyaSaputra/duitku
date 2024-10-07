<script setup lang="ts">
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";

const headerElement = ref<HTMLElement | null>(null);

const emit = defineEmits(['updateHeaderHeight']);

const { user, revokeAuthToken } = useUser();

onMounted(() => {
  if (headerElement.value) {
    const height = headerElement.value.offsetHeight;

    emit('updateHeaderHeight', height);
  }
});
</script>

<template>
  <nav ref="headerElement" class="py-4 border-b sticky top-0 z-20 bg-white">
    <div class="lg:px-[2rem] px-4 flex justify-between items-center">
      <NuxtLink to="/beranda">
        <h1 class="text-xl font-bold">
          Duit<span class="text-emerald-500">Ku</span>
        </h1>
      </NuxtLink>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="outline" size="icon">
            <Avatar class="bg-transparent">
              <AvatarFallback class="bg-transparent font-bold">{{
                user?.name[0].toUpperCase()
              }}</AvatarFallback>
            </Avatar>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end">
          <DropdownMenuLabel class="text-base">Pengaturan</DropdownMenuLabel>
          <DropdownMenuSeparator />
          <NuxtLink to="/pengaturan/mata-uang">
            <DropdownMenuItem class="cursor-pointer">
              <Icon name="lucide:badge-dollar-sign" class="w-4 h-4 mr-2" />
              <span class="text-sm text-black/80">Mata Uang</span>
            </DropdownMenuItem>
          </NuxtLink>

          <NuxtLink to="pengaturan/profil">
            <DropdownMenuItem class="cursor-pointer">
              <Icon name="lucide:user" class="w-4 h-4 mr-2" />
              <span class="text-sm text-black/80">Profil Mu</span>
            </DropdownMenuItem>
          </NuxtLink>

          <DropdownMenuSeparator />

          <DropdownMenuItem
            class="focus:bg-transparent flex flex-col items-start"
          >
            <p class="font-bold">{{ user?.name }}</p>
            <p class="text-xs">{{ user?.email }}</p>
          </DropdownMenuItem>
          <Button
            variant="destructive"
            size="sm"
            class="w-full flex justify-between items-center mt-2"
            @click="revokeAuthToken"
          >
            <span class="text-sm font-bold text-white">Keluar</span>
            <Icon name="lucide:log-out" class="w-4 h-4" />
          </Button>
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  </nav>
</template>
