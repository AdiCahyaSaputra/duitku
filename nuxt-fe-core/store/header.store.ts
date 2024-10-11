export const headerStore = defineStore("header", {
  state: (): { headerHeight: number } => {
    return {
      headerHeight: 0,
    };
  },
  actions: {
    setHeaderHeight(headerHeight: number) {
      this.headerHeight = headerHeight;
    },
  },
});
