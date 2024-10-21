export const headerStore = defineStore("header", {
  state: (): { headerHeight: number } => {
    return {
      headerHeight: 0,
    };
  },
  actions: {
    setHeaderHeight(headerHeight: number) {
      // if (typeof window.localStorage !== "undefined") {
      //   localStorage.setItem("header-height", headerHeight.toString());
      // }

      this.headerHeight = headerHeight;
    },
  },
});
