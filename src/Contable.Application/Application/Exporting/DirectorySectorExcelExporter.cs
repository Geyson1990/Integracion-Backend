using Contable.Application.DirectorySectors.Dto;
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
    public class DirectorySectorExcelExporter : NpoiExcelExporterBase, IDirectorySectorExcelExporter
    {
        public DirectorySectorExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }
        private void SetHeading(ISheet sheet, string title)
        {
            CreateBoldCell(sheet, 0, 0, title, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
        }
        public FileDto ExportMatrizToFile(List<DirectorySectorExcelExportDto> records, bool varCheckName, bool varCheckEnabled)
        {
            return CreateExcelPackage("SECTORES_RUBROS_DIRECTORIO_EMPRESAS_PRIVADAS.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("SECTORES_RUBROS_DIRECTORIO_EMPRESAS_PRIVADAS");
                var initRow = 0;
                var listCuerpo1 = string.Empty;
                var result = new StringBuilder();



                SetHeading(sheet, "Listado Sectores o rubros del directorio de empresas privadas");
                initRow++;

                AddHeader(sheet, initRow,
                "Nombre",
                "Habilitado");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                     //Aspectos generales
                     _ => new ExportCell(_.Name),
                     _ => new ExportCell(_.enabled)
                     );


                if (varCheckName == false)
                {
                    sheet.SetColumnHidden(0, true);
                }
                if (varCheckEnabled == false)
                {
                    sheet.SetColumnHidden(1, true);
                }


                for (var i = 0; i < 25; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }
    }
}
