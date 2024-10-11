export const transformDotNetTimestamp = (datetime: string) => {
  const date = new Date(datetime);

  const daysOfWeek = [
    "Minggu",
    "Senin",
    "Selasa",
    "Rabu",
    "Kamis",
    "Jumat",
    "Sabtu",
  ];
  const dayName = daysOfWeek[date.getDay()];

  const formattedDate = `${String(date.getDate()).padStart(2, "0")}/${String(date.getMonth() + 1).padStart(2, "0")}/${date.getFullYear()}`;

  const result = [dayName, formattedDate];

  return result;
};

export const createQueryStringParams = (params: { [key: string]: any }) => {
  const queryString = [];

  for(const [key, value] of Object.entries(params)) {
    if(!!value) {
      queryString.push(`${key}=${value}`);
    }
  }

  return queryString.join('&');
}
