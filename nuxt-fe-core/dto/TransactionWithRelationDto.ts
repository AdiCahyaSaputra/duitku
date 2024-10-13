export default interface TransactionWithRelationDto {
  id: string;
  description: string;
  amount: number;
  date: string;
  account: {
    name: string;
  };
  category: {
    name: string;
  };
  subCategory: {
    name: string;
  } | null;
}
