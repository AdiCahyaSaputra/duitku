import type UserDto from "@/dto/UserDto";

export const userStore = defineStore("user", {
  state: (): { user: UserDto | null } => {
    return {
      user: null,
    };
  },
  actions: {
    async getUser() {
      if (typeof window.localStorage !== "undefined") {
        const user = localStorage.getItem("user");

        if (user) {
          this.$state.user = JSON.parse(user);
        }
      } else {
        const data = await $fetch("/api/auth/user");

        if (!!data) {
          this.$state.user = data;
        }
      }
    },
    async setUser() {
      const data = await $fetch("/api/auth/user");

      if (!!data) {
        if (typeof window.localStorage !== "undefined") {
          localStorage.setItem("user", JSON.stringify(data));
        }

        this.$state.user = data;
      }
    },

    async revokeUser() {
      if (typeof window.localStorage !== "undefined") {
        localStorage.clear();
      }

      this.$state.user = null;
    },
  },
});
