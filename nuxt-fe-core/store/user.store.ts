import type UserDto from "@/dto/UserDto";
import { getFromLocalStorage } from "@/lib/helper";

export const userStore = defineStore("user", {
  state: (): { user: UserDto | null } => {
    return {
      user: (JSON.parse(getFromLocalStorage('user') ?? '')),
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
