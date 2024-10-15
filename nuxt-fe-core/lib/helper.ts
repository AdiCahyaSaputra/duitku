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

export const castStringDateIntoDotNetDate = (dateStr: string) => {
  // DotNet use ISO 8601 Format with high precision (up to 7 decimal places for Fractional Seconds) "2024-10-11T06:53:34.3478836Z"
  const date = new Date(dateStr);
  const isoString = date.toISOString();
  const ms = date.getMilliseconds().toString().padStart(3, "0");
  const additionalPrecision = "0000";

  return isoString.replace(/\.\d+Z$/, `.${ms}${additionalPrecision}Z`);
};

export const createQueryStringParams = (params: { [key: string]: any }) => {
  const queryString = [];

  for (const [key, value] of Object.entries(params)) {
    if (!!value) {
      queryString.push(`${key}=${value}`);
    }
  }

  return queryString.join("&");
};

export const debounceAction = (callback: Function, delay: number = 1000) => {
  return setTimeout(callback, delay);
};

export const toComboboxCommandListFriendly = <T>(
  data: T[],
  valueKey: keyof T,
  labelKey: keyof T,
) => {
  return data.map((item) => ({
    value: item[valueKey] as string, // It should be an Guid
    label: item[labelKey] as string,
  }));
};

export const toIDR = (price: number) => {
  return new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
    currencySign: 'accounting'
  }).format(price);
}

export const getCurrentDate = () => {
  const date = new Date();

  return {
    date: date.getDate(), 
    month: date.getMonth() + 1, // month itu kayak index array klo di date js mah wkwkwk
    year: date.getFullYear(), // month itu kayak index array klo di date js mah wkwkwk
  };
}