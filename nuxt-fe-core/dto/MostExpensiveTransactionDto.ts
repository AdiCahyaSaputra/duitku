export default interface MostExpensiveTransactionDto {
  id: string;
  account: {
    id: string;
    name: string;
    balance: string;
  };
  amount: number;
  description: string;
  date: string;
}