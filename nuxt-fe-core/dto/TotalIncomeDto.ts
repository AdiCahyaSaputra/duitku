import type AccountDto from "./AccountDto";

export default interface TotalIncomeDto {
  totalAsset: number;
  accounts: AccountDto[];
}
