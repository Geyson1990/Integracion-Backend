using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Contable.Application.Exporting.Dto;
using Contable.Application.HelpMemories.Dto;
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
    public class HelpMemoryExcelExporter : NpoiExcelExporterBase, IHelpMemoryExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IWebHostEnvironment _env;

        public HelpMemoryExcelExporter(
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

        public FileDto ExportHelpMemory(List<HelpMemoryResumenReport> records, List<HelpMemoryDetalleReport> detalle, List<HelpMemoryDetalleEstadisticaReport> estadistica)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("Ayuda_Memoria" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("RESUMEN");
                var initRow = 0;

                SetHeading(sheet, "Reporte de Ayuda de Memorias");
                initRow++;

                AddHeader(sheet, initRow,
                "Año de Emisión",
                "Mes de Emisión",
                "Cantidad de Ayuda de Memoria",
                "Cantidad Casos Conflictivos",
                "Cantidad Conflictividad Social");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.AnioEmision),
                _ => new ExportCell(_.MesEmision),
                _ => new ExportCell(_.CantidadAyudaMemoria),
                _ => new ExportCell(_.CantidadCasosConflictivos),
                _ => new ExportCell(_.CantidadConflictividadSocial)
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
                "Fecha de Emisión",
                "Año de Emisión",
                "Mes de Emisión",
                "Código de Memoria",
                "Entidad Solicitante",
                "Persona Solicitante",
                "Código Conflicto",
                "Nombre Conflicto",
                "Estado Actual Conflicto",
                "Cantidad de Espacios de Diálogo",
                "Codigo de Espacios de Diálogo",
                "Unidad Territorial Conflicto",
                "Tipología Conflicto",
                "Tipología Conflicto Detalle",
                "Código Sensible",
                "Nombre Sensible",
                "Descripcion Estado Actual Sensible",
                "Unidad Territorial Sensible",
                "Tipología Sensible");

                initRowDet++;

                AddObjects(excelPackage, sheetDet, initRowDet, detalle,
                //Aspectos generales
                _ => new ExportCell(_.FechaEmision),
                _ => new ExportCell(_.AnioEmision),
                _ => new ExportCell(_.MesEmision),
                _ => new ExportCell(_.codigo),
                _ => new ExportCell(_.entidadSolicitante),
                _ => new ExportCell(_.personaSolicitante),
                _ => new ExportCell(_.codigoConflicto),
                _ => new ExportCell(_.nombreConflicto),
                _ => new ExportCell(_.estadoConflicto),
                _ => new ExportCell(_.cantidadEspaciosDialogo),
                _ => new ExportCell(_.codigoEspaciosDialogo),
                _ => new ExportCell(_.unidadTerritorialConflicto),
                _ => new ExportCell(_.tipologia),
                _ => new ExportCell(_.tipologiaDetallada),
                _ => new ExportCell(_.codigoSensible),
                _ => new ExportCell(_.nombreSensible),
                _ => new ExportCell(_.estadoSensible),
                _ => new ExportCell(_.unidadTerritorialSensible),
                _ => new ExportCell(_.tipologiaSensible)
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
                        for (var cell = 0; cell <= 17; cell++)
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
                sheetDet.SetColumnWidth(9, 10000);
                sheetDet.SetColumnWidth(10, 20000);
                sheetDet.SetColumnWidth(11, 10000);
                sheetDet.SetColumnWidth(12, 10000);
                sheetDet.SetColumnWidth(13, 10000);
                sheetDet.SetColumnWidth(14, 10000);
                sheetDet.SetColumnWidth(15, 15000);
                sheetDet.SetColumnWidth(16, 10000);
                sheetDet.SetColumnWidth(17, 10000);
                sheetDet.SetColumnWidth(18, 10000);


                // Estadistica 

                var sheetEst = excelPackage.CreateSheet("ESTADÍSTICA");
                var initRowEst = 0;

                SetHeading(sheetEst, "Detalle de Estadística de Ayuda de Memoria por espacios de diálogo");

                initRowEst++;

                AddHeader(sheetEst, initRowEst,
                "Fecha de Emisión",
                "Año de Emisión",
                "Mes de Emisión",
                "Código de Memoria",
                "Entidad Solicitante",
                "Persona Solicitante",
                "Código Conflicto Social",
                "Nombre Conflicto Social",
                "Estado Actual",
                "Unidad Territorial Conflicto Social",
                "Tipología Conflicto Social",
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
                _ => new ExportCell(_.entidadSolicitante),
                _ => new ExportCell(_.personaSolicitante),
                _ => new ExportCell(_.codigoConflicto),
                _ => new ExportCell(_.nombreConflicto),
                _ => new ExportCell(_.estadoConflicto),
                _ => new ExportCell(_.unidadTerritorialConflicto),
                _ => new ExportCell(_.tipologiaConflicto),
                _ => new ExportCell(_.codigoEspacio),
                _ => new ExportCell(_.denominacionEspacio),
                _ => new ExportCell(_.tipoEspacio)
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
                        for (var cell = 0; cell <= 13; cell++)
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
                sheetEst.SetColumnWidth(15, 10000);
                sheetEst.SetColumnWidth(16, 10000);

            });
        }


        private void SetHeading(ISheet sheet, string tittle)
        {
            CreateBoldCell(sheet, 0, 0, tittle, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));

        }

    }
}

