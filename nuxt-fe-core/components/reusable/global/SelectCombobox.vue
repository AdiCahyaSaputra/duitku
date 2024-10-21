<script setup lang="ts">
import {
  Command,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

const emit = defineEmits<{
  (e: 'updateValue', selectedValue: string): void
}>();

const props = defineProps<{
  items: {
    value: string;
    label: string;
  }[];
  name: string;
  isLoading: boolean;
  defaultSelectedValue?: string;
}>();

const open = ref(false);
const searchTerm = ref("");
const selectedValue = ref(props.defaultSelectedValue);

const filterData = computed(() => {
  const data = searchTerm.value === ''
    ? props.items
    : props.items.filter(item =>
      item.label.toLowerCase().includes(searchTerm.value.toLocaleLowerCase())
    );

  return data;
});

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
      <Command v-model:searchTerm="searchTerm">
        <CommandInput :placeholder="'Cari ' + props.name" />
        <CommandEmpty>{{ filterData.length < 1 ? (props.isLoading ? "Lagi di cari.." : props.name + " nggak ketemu 🥲") : '' }}</CommandEmpty>
        <CommandList>
          <CommandGroup>
            <CommandItem v-for="(item, idx) in filterData" :key="item.value" :value="item"
              @select="() => handleSelect(item.value)">
              {{ item.label }}
            </CommandItem>
          </CommandGroup>
          <Button variant="secondary" class="w-full" @click="() => handleSelect('')">
            Kosongin
          </Button>
        </CommandList>
      </Command>
    </PopoverContent>
  </Popover>
</template>
