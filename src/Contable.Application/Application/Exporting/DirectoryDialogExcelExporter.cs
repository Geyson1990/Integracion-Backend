using Contable.Application.DirectoryDialogs.Dto;
using Contable.Application.Exporting.Dto;
using Contable.DataExporting.Excel.NPOI;
using Contable.Dto;
using Contable.Storage;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public class DirectoryDialogExcelExporter : NpoiExcelExporterBase, IDirectoryDialogExcelExporter
    {
        public DirectoryDialogExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        private void SetHeading(ISheet sheet, string title)
        {
            CreateBoldCell(sheet, 0, 0, title, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
        }

        public FileDto ExportMatrizToFile(List<DirectoryDialogExcelExportDto> records, bool varCheckName, bool varCheckLastName, bool varCheckMothersLastName, bool varCheckPost, bool varCheckEntity, bool varCheckWeb, bool varCheckLandline, bool varCheckCellPhone, bool varCheckEnabled)
        {
            return CreateExcelPackage("DIRECTORIO_DIALOGO_CONFLICTOS_SOCIALES.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("DIRECTORIO_DIALOGO_CONFLICTOS_SOCIALES");
                var initRow = 0;
                var listCuerpo1 = string.Empty;
                var result = new StringBuilder();



                SetHeading(sheet, "Listado Directorio de Diálogo y Conflictos Sociales");
                initRow++;

                AddHeader(sheet, initRow,
                "Nombre",
                "Apellido Paterno",
                "Apellido Materno",
                "Cargo",
                "Entidad",
                "Correo Electrónico",
                "Teléfono Fijo",
                "Teléfono celular",
                "Habilitado");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                     //Aspectos generales
                     _ => new ExportCell(_.name),
                     _ => new ExportCell(_.last_name),
                     _ => new ExportCell(_.mothers_last_name),
                     _ => new ExportCell(_.post),
                     _ => new ExportCell(_.entity),
                     _ => new ExportCell(_.web),
                     _ => new ExportCell(_.landline),
                     _ => new ExportCell(_.cell_phone),
                     _ => new ExportCell(_.enabled)
                     );


                if (varCheckName == false)
                {
                    sheet.SetColumnHidden(0, true);
                }
                if (varCheckLastName == false)
                {
                    sheet.SetColumnHidden(1, true);
                }
                if (varCheckMothersLastName == false)
                {
                    sheet.SetColumnHidden(2, true);
                }
                if (varCheckPost == false)
                {
                    sheet.SetColumnHidden(3, true);
                }
                if (varCheckEntity == false)
                {
                    sheet.SetColumnHidden(4, true);
                }
                if (varCheckWeb == false)
                {
                    sheet.SetColumnHidden(5, true);
                }
                if (varCheckLandline == false)
                {
                    sheet.SetColumnHidden(6, true);
                }
                if (varCheckCellPhone == false)
                {
                    sheet.SetColumnHidden(7, true);
                }
                if (varCheckEnabled == false)
                {
                    sheet.SetColumnHidden(8, true);
                }

                for (var i = 0; i < 25; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }
    }
}
