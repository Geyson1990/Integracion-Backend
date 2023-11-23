using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Contable.Application.Exporting.Dto;
using Contable.Application.HelpMemories.Dto;
using Contable.Application.SectorMeets.Dto;
using Contable.Application.SocialConflictTaskManagements.Dto;
using Contable.Application.TaskManagements.Dto;
using Contable.DataExporting.Excel.NPOI;
using Contable.Dto;
using Contable.Storage;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace Contable.Application.Exporting
{
    public class SectorMeetExcelExporter : NpoiExcelExporterBase, ISectorMeetExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IWebHostEnvironment _env;

        public SectorMeetExcelExporter(
            IWebHostEnvironment env,
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _env = env;
        }

        public FileDto ExportSectorMeet(List<SectorMeetResumenReport> records, List<SectorMeetDetalleReport> detalle, List<SectorMeetEstadisticaReport> estadistica)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("Reuniones_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("RESUMEN");
                var initRow = 0;

                SetHeading(sheet, "Reporte de Reuniones ");
                initRow++;

                AddHeader(sheet, initRow,
                "Año de Emisión",
                "Mes de Emisión",
                "Cantidad de Reuniones",
                "Cantidad Casos Conflictivos",
                "Cantidad Pre-Casos Conflictivos");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.AnioEmision),
                _ => new ExportCell(_.MesEmision),
                _ => new ExportCell(_.CantidadSectorMeet),
                _ => new ExportCell(_.CantidadCasosConflictivos),
                _ => new ExportCell(_.CantidadPreCaso)
                );

                var cellCentertTopAlignment = sheet.Workbook.CreateCellStyle();
                cellCentertTopAlignment = sheet.Workbook.CreateCellStyle();
                cellCentertTopAlignment.Alignment = HorizontalAlignment.Left;
                cellCentertTopAlignment.VerticalAlignment = VerticalAlignment.Top;
                cellCentertTopAlignment.WrapText = true;

                for (var i = 0; i < records.Count; i++)
                {
                    if (sheet.GetRow(i + initRow) != null)
                    {
                        for (var cell = 0; cell <= 4; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 10000);
                sheet.SetColumnWidth(3, 10000);
                sheet.SetColumnWidth(4, 10000);




                // Detalle 

                var sheetDet = excelPackage.CreateSheet("DETALLE");
                var initRowDet = 0;

                SetHeading(sheetDet, "Detalle del Reporte de Ayuda de Memorias");

                initRowDet++;

                AddHeader(sheetDet, initRowDet,
                "Fecha de Reunión",
                "Año de Reunión",
                "Mes de Reunión",
                "Código",
                "Nombre",
                "Unidad Territorial",
                "Código Conflicto",
                "Nombre Conflicto",
                "Estado Actual Conflicto",
                "Cantidad de Espacios de Diálogo",
                "Códgio de Espacios de Diálogo",
                "Unidad Territorial Conflicto",
                "Tipología Conflicto",
                "Tipología Detallada"
                );

                initRowDet++;

                AddObjects(excelPackage, sheetDet, initRowDet, detalle,
                //Aspectos generales
                _ => new ExportCell(_.FechaEmision),
                _ => new ExportCell(_.AnioEmision),
                _ => new ExportCell(_.MesEmision),
                _ => new ExportCell(_.codigo),
                _ => new ExportCell(_.nombreConflicto),
                _ => new ExportCell(_.unidadTerritorial),
                _ => new ExportCell(_.codigoConflicto),
                _ => new ExportCell(_.nombreConflicto),
                _ => new ExportCell(_.estadoConflicto),
                _ => new ExportCell(_.cantidadEspaciosDialogo),
                _ => new ExportCell(_.codigoEspaciosDialogo),
                _ => new ExportCell(_.unidadTerritorialConflicto),
                _ => new ExportCell(_.tipologia),
                _ => new ExportCell(_.tipologiaDetallada)
                );

                var cellCentertTopAlignmentDet = sheetDet.Workbook.CreateCellStyle();
                cellCentertTopAlignmentDet = sheetDet.Workbook.CreateCellStyle();
                cellCentertTopAlignmentDet.Alignment = HorizontalAlignment.Left;
                cellCentertTopAlignmentDet.VerticalAlignment = VerticalAlignment.Top;
                cellCentertTopAlignmentDet.WrapText = true;

                for (var i = 0; i < detalle.Count; i++)
                {
                    if (sheetDet.GetRow(i + initRowDet) != null)
                    {
                        for (var cell = 0; cell <= 13; cell++)
                        {
                            sheetDet.GetRow(i + initRowDet).Cells[cell].CellStyle = cellCentertTopAlignmentDet;
                        }
                    }
                }

                sheetDet.SetColumnWidth(0, 10000);
                sheetDet.SetColumnWidth(1, 10000);
                sheetDet.SetColumnWidth(2, 10000);
                sheetDet.SetColumnWidth(3, 10000);
                sheetDet.SetColumnWidth(4, 20000);
                sheetDet.SetColumnWidth(5, 20000);
                sheetDet.SetColumnWidth(6, 10000);
                sheetDet.SetColumnWidth(7, 20000);
                sheetDet.SetColumnWidth(8, 20000);
                sheetDet.SetColumnWidth(9, 20000);
                sheetDet.SetColumnWidth(10, 10000);
                sheetDet.SetColumnWidth(11, 10000);
                sheetDet.SetColumnWidth(12, 10000);
                sheetDet.SetColumnWidth(13, 15000);



                // Estadistica 

                var sheetEst = excelPackage.CreateSheet("ESTADÍSTICA");
                var initRowEst = 0;

                SetHeading(sheetEst, "Detalle de Estadística de Reuniones por espacios de diálogo");

                initRowEst++;

                AddHeader(sheetEst, initRowEst,
                "Fecha de Reunión",
                "Año de Reunión",
                "Mes de Reunión",
                "Código de Reunión",
                "Nombre de Reunión",
                "Unidad Territorial",
                "Código Conflicto",
                "Nombre Conflicto",
                "Estado Actual Conflicto",
                "Unidad Territorial Conflicto",
                "Tipología Conflicto",
                "Tipología Detallada Conflicto",
                "Código Espacio Diálogo",
                "Denominación Espacio Diálogo",
                "Tipo de Espacio Diálogo");

                initRowEst++;

                AddObjects(excelPackage, sheetEst, initRowEst, estadistica,
                //Aspectos generales
                _ => new ExportCell(_.FechaEmision),
                _ => new ExportCell(_.AnioEmision),
                _ => new ExportCell(_.MesEmision),
                _ => new ExportCell(_.codigo),
                _ => new ExportCell(_.nombreReunion),
                _ => new ExportCell(_.unidadTerritorial),
                _ => new ExportCell(_.codigoConflicto),
                _ => new ExportCell(_.nombreConflicto),
                _ => new ExportCell(_.estadoConflicto),
                _ => new ExportCell(_.unidadTerritorialConflicto),
                _ => new ExportCell(_.tipologia),
                _ => new ExportCell(_.tipologiaDetallada),
                _ => new ExportCell(_.codigoEspaciosDialogo),
                _ => new ExportCell(_.denominacionEspaciosDialogo),
                _ => new ExportCell(_.tipoEspaciosDialogo)
                );

                var cellCentertTopAlignmentEst = sheetDet.Workbook.CreateCellStyle();
                cellCentertTopAlignmentEst = sheetDet.Workbook.CreateCellStyle();
                cellCentertTopAlignmentEst.Alignment = HorizontalAlignment.Left;
                cellCentertTopAlignmentEst.VerticalAlignment = VerticalAlignment.Top;
                cellCentertTopAlignmentEst.WrapText = true;

                for (var i = 0; i < estadistica.Count; i++)
                {
                    if (sheetEst.GetRow(i + initRowEst) != null)
                    {
                        for (var cell = 0; cell <= 14; cell++)
                        {
                            sheetEst.GetRow(i + initRowEst).Cells[cell].CellStyle = cellCentertTopAlignmentEst;
                        }
                    }
                }

                sheetEst.SetColumnWidth(0, 10000);
                sheetEst.SetColumnWidth(1, 10000);
                sheetEst.SetColumnWidth(2, 10000);
                sheetEst.SetColumnWidth(3, 10000);
                sheetEst.SetColumnWidth(4, 20000);
                sheetEst.SetColumnWidth(5, 20000);
                sheetEst.SetColumnWidth(6, 10000);
                sheetEst.SetColumnWidth(7, 20000);
                sheetEst.SetColumnWidth(8, 20000);
                sheetEst.SetColumnWidth(9, 20000);
                sheetEst.SetColumnWidth(10, 10000);
                sheetEst.SetColumnWidth(11, 10000);
                sheetEst.SetColumnWidth(12, 10000);
                sheetEst.SetColumnWidth(13, 15000);
                sheetEst.SetColumnWidth(14, 10000);

        });

        }
        private void SetHeading(ISheet sheet, string tittle)
        {
            CreateBoldCell(sheet, 0, 0, tittle, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));

        }
    }
}

