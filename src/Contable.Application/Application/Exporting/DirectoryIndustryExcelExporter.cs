using Contable.Application.DirectoryIndustries.Dto;
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
    public class DirectoryIndustryExcelExporter : NpoiExcelExporterBase, IDirectoryIndustryExcelExporter
    {
        public DirectoryIndustryExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }
        private void SetHeading(ISheet sheet, string title)
        {
            CreateBoldCell(sheet, 0, 0, title, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
        }

        public FileDto ExportMatrizToFile(List<DirectoryIndustryExcelExportDto> records, bool varCheckNameEmpresa, bool varCheckSector, bool varCheckAddress, bool varCheckPhoneNumber, bool varCheckUrl, bool varCheckDepartamento, bool varCheckProvincia, bool varCheckDistrito, bool varCheckHabilitado)
        {
            return CreateExcelPackage("DIRECTORIO_EMPRESAS_PRIVADAS.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("DIRECTORIO_EMPRESAS_PRIVADAS");
                var initRow = 0;
                var listCuerpo1 = string.Empty;
                var result = new StringBuilder();

                SetHeading(sheet, "Listado Directorio de Empresas privadas");
                initRow++;

                AddHeader(sheet, initRow,
                "Nombre Empresa",
                "Sector",
                "Dirección",
                "Teléfono",
                "Pagina Web",
                "Departamento",
                "Provincia",
                "Dictrito",
                "Habilitado");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                     //Aspectos generales
                     _ => new ExportCell(_.NameEmpresa),
                     _ => new ExportCell(_.Sector),
                     _ => new ExportCell(_.Address),
                     _ => new ExportCell(_.PhoneNumber),
                     _ => new ExportCell(_.Url),
                     _ => new ExportCell(_.Departamento),
                     _ => new ExportCell(_.Provincia),
                     _ => new ExportCell(_.Distrito),
                     _ => new ExportCell(_.Habilitado)
                     );


                if (varCheckNameEmpresa == false)
                {
                    sheet.SetColumnHidden(0, true);
                }
                if (varCheckSector == false)
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
                if (varCheckDepartamento == false)
                {
                    sheet.SetColumnHidden(5, true);
                }
                if (varCheckProvincia == false)
                {
                    sheet.SetColumnHidden(6, true);
                }
                if (varCheckDistrito == false)
                {
                    sheet.SetColumnHidden(7, true);
                }
                if (varCheckHabilitado == false)
                {
                    sheet.SetColumnHidden(8, true);
                }

                for (var i = 0; i < 25; i++)
                    sheet.SetColumnWidth(i, 5000);
            });

        }
    }
}
