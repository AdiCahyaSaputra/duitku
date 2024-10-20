import type TransactionWithRelationDto from "@/dto/TransactionWithRelationDto";
import ExcelJS from "exceljs";

export const exportToExcel = async (
  data: Omit<TransactionWithRelationDto, "id">[]
) => {
  const workbook = new ExcelJS.Workbook();
  const worksheet = workbook.addWorksheet("REPORT DATA");

  // Table Header
  const headerRow = worksheet.addRow([
    "Deksripsi",
    "Sumber Akun",
    "Kategori",
    "Sub Kategori",
    "Harga",
    "Tanggal",
  ]);

  headerRow.eachCell((cell) => {
    cell.border = {
      top: { style: "thin" },
      left: { style: "thin" },
      bottom: { style: "thin" },
      right: { style: "thin" },
    };

    cell.font = {
      bold: true,
    };
  });

  // Table Data
  data.forEach((transaction) => {
    worksheet.addRow([
      transaction.description,
      transaction.account.name,
      transaction.category.name,
      transaction.subCategory?.name || "-",
      transaction.amount,
      new Date(transaction.date).toLocaleDateString("id-ID"),
    ]);
  });

  worksheet.getColumn(5).numFmt =
    '_("Rp"* #,##0.00_);_("Rp"* (#,##0.00);_("Rp"* "-"??_);_(@_)';

  // Auto Fit
  worksheet.columns.forEach((column, index) => {
    const maxLength = column.values?.reduce((acc, curr) => {
      return Math.max(+(acc || 10), curr ? curr.toString().length : 0);
    }, 10);

    column.width = +(maxLength || 10) + 4; // Extra padding for the column width
  });

  const buffer = await workbook.xlsx.writeBuffer();

  // Download Buffer
  const blob = new Blob([buffer], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
  });

  const link = document.createElement("a");
  const url = URL.createObjectURL(blob);

  link.href = url;
  link.download =
    new Date().toLocaleDateString("id-ID") + "-" + "DUITKU-REPORT.xlsx";

  document.body.appendChild(link);

  link.click();

  document.body.removeChild(link);
};
