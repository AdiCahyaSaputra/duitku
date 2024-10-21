export const headerStore = defineStore("header", {
  state: (): { headerHeight: number } => {
    let headerHeight = 0;

    if (typeof window.localStorage !== "undefined") {
      headerHeight = +(localStorage.getItem("header-height") ?? '0');
    }

    return {
      headerHeight,
    };
  },
  actions: {
    setHeaderHeight(headerHeight: number) {
      if (typeof window.localStorage !== "undefined") {
        localStorage.setItem("header-height", headerHeight.toString());
      }

      this.headerHeight = headerHeight;
    },
  },
});
