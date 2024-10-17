export const useApi = () => {
  const api = $fetch.create({
    baseURL: "/duit-ku/api",
    async onResponseError({ response, error }): Promise<any> {
      if (!response.ok) {
        throw response._data;
      }
    },
  });

  return api;
}
