using Contable.Application.DirectoryGovernments.Dto;
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
    public class DirectoryGovernmentExcelExporter : NpoiExcelExporterBase, IDirectoryGovernmentExcelExporter
    {
        public DirectoryGovernmentExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        private void SetHeading(ISheet sheet, string title)
        {
            CreateBoldCell(sheet, 0, 0, title, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
        }

        public FileDto ExportMatrizToFile(List<DirectoryGovernmentExcelExportDto> records, bool varCheckName, bool varCheckShortName, bool varCheckAddress, bool varCheckPhoneNumber, bool varCheckUrl, bool varCheckTipo, bool varCheckSector, bool varCheckHabilitado)
        {
            return CreateExcelPackage("ENTIDADES_ESTADO_PERUANO.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("ENTIDADES_ESTADO_PERUANO");
                var initRow = 0;
                var listCuerpo1 = string.Empty;
                var result = new StringBuilder();

                List<string> listColumnas = new List<string>();
                List<string> listCuerpo = new List<string>();
                List<string> listCuerpoName = new List<string>();
                List<string> listCuerpoShortName = new List<string>();



                SetHeading(sheet, "Listado Entidades del Estado Peruano");
                initRow++;

                AddHeader(sheet, initRow,
                "Nombre",
                "Nombre corto",
                "Dirección",
                "Teléfono",
                "Pagina Web",
                "Tipo",
                "Sector",
                "Habilitado");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                     //Aspectos generales
                     _ => new ExportCell(_.Name),
                     _ => new ExportCell(_.ShortName),
                     _ => new ExportCell(_.address),
                     _ => new ExportCell(_.telephone),
                     _ => new ExportCell(_.page_web),
                     _ => new ExportCell(_.guy),
                     _ => new ExportCell(_.sector),
                     _ => new ExportCell(_.enabled)
                     );


                if (varCheckName == false)
                {
                    sheet.SetColumnHidden(0, true);
                }
                if (varCheckShortName == false)
                {
                    sheet.SetColumnHidden(1, true);
                }
                if (varCheckAddress == false)
                {
                    sheet.SetColumnHidden(2, true);
                }
                if (varCheckPhoneNumber == false)
                {
                    sheet.SetColumnHidden(3, true);
                }
                if (varCheckUrl == false)
                {
                    sheet.SetColumnHidden(4, true);
                }
                if (varCheckTipo == false)
                {
                    sheet.SetColumnHidden(5, true);
                }
                if (varCheckSector == false)
                {
                    sheet.SetColumnHidden(6, true);
                }
                if (varCheckHabilitado == false)
                {
                    sheet.SetColumnHidden(7, true);
                }

                for (var i = 0; i < 25; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }
    }
}
