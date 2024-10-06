export const defaultHeaderAPI = {
  "Content-Type": "application/json",
  Accept: "application/json",
};

export const authHeaderAPI = (token: string) => {
  return {
    ...defaultHeaderAPI,
    Authorization: `Bearer ${token}`,
  };
};

export const apiBaseUrl = "http://localhost:5143";
