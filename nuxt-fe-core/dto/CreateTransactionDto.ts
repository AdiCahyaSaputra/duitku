export default interface CreateTransactionDto { 
  accountId: string;
  categoryId: string;
  subCategoryId: string | null;
  date: string;
  description: string;
  amount: number;
}
