<script setup lang="ts">
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";

const emit = defineEmits(["updateValue"]);

const props = defineProps<{
  items: {
    value: string;
    label: string;
  }[];
  name: string;
  isLoading: boolean;
}>();

const open = ref(false);
const selectedValue = ref("");

const handleSelect = (value: string) => {
  open.value = false;
  selectedValue.value = value;

  emit('updateValue', selectedValue.value);
}
</script>

<template>
  <Popover v-model:open="open">
    <PopoverTrigger as-child>
      <Button variant="outline" role="combobox" :aria-expanded="open" class="w-full justify-between">
        <span v-if="selectedValue">{{ props.items.find(item => item.value === selectedValue)?.label }}</span>
        <span v-else class="text-muted-foreground font-normal">Pilih {{ props.name }}</span>
        <Icon name="lucide:chevron-down" class="ml-2 h-4 w-4 shrink-0 opacity-50" />
      </Button>
    </PopoverTrigger>
    <PopoverContent class="w-full p-0" align="start">
      <Command>
        <CommandInput :placeholder="'Cari ' + props.name" />
        <CommandEmpty>{{ props.isLoading ? "Lagi di cari.." : props.name + " nggak ketemu 🥲" }}</CommandEmpty>
        <CommandList>
          <CommandGroup>
            <CommandItem v-for="(item, idx) in props.items" :key="idx" :value="item.value" @select="() => handleSelect(item.value)">
              {{ item.label }}
            </CommandItem>
          </CommandGroup>
        </CommandList>
      </Command>
    </PopoverContent>
  </Popover>
</template>
