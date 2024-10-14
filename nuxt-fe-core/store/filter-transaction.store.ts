import type FilterTransactionDto from "@/dto/FilterTransactionDto";

export const filterTransactionStore = defineStore("filterTransaction", {
  state: (): FilterTransactionDto & { search?: string } => {
    return {
      search: undefined,
      categoryId: undefined,
      subCategoryId: undefined,
      accountId: undefined,
      dateStart: undefined,
      dateEnd: undefined,
    };
  },
  actions: {
    setFilter(filter: FilterTransactionDto & { search?: string }) {
      this.categoryId = filter.categoryId;
      this.subCategoryId = filter.subCategoryId;
      this.accountId = filter.accountId;
      this.dateStart = filter.dateStart;
      this.dateEnd = filter.dateEnd;
    },
    setSearchFilter(search?: string) {
      this.search = search;
    }
  },
});
