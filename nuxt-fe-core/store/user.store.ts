import type UserDto from "@/dto/UserDto";

export const userStore = defineStore("user", {
  state: (): { user: UserDto | null } => {
    return {
      user: null,
    };
  },
  actions: {
    setUser(user: UserDto | null) {
      if (typeof window.localStorage !== "undefined") {
        localStorage.setItem("user", JSON.stringify(user));
      }

      this.user = user;
    },
  },
});
