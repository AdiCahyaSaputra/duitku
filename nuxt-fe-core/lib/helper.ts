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

export const toISOLocaleDateString = (dateInstance: Date) => {
  const localeDateString = dateInstance.toLocaleDateString('id-ID');

  const [date, month, year] = localeDateString.split('/');

  return `${year}-${month.padStart(2, '0')}-${date.padStart(2, '0')}`;
}

export const parseOCRText = (ocr: string) => {
  // (e.g., 30 Sep 2024, 2024-09-30, 30/09/2024)
  const datePattern = /\b(\d{1,2}[\/\s-]\w+[\/\s-]\d{2,4})\b|\b(\d{4}[\/-]\d{2}[\/-]\d{2})\b/;
  const totalPricePattern = /Total.*?[\D]*([\d.,]+)/;

  const dateMatch = ocr.match(datePattern);
  const totalPriceMatch = ocr.match(totalPricePattern);

  const transactionDate = dateMatch ? new Date(dateMatch[0]) : new Date();
  const totalPrice = totalPriceMatch ? totalPriceMatch[1] : 0;

  return {
    date: transactionDate,
    amount: totalPrice,
    debug: {
      date: dateMatch,
      amount: totalPriceMatch,
    }
  }
}

export const getFromLocalStorage = (key: string): string | null => {
  if(import.meta.client) {
    if (typeof window.localStorage !== "undefined") {
      return localStorage.getItem(key);
    }
  }

  return null
}
