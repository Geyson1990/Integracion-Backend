using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Contable.Application.Exporting.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.DataExporting.Excel.NPOI;
using Contable.Dto;
using Contable.Storage;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Contable.Application.Exporting
{
    public class SocialConflictExcelExporter : NpoiExcelExporterBase, ISocialConflictExcelExporter
    {
        public SocialConflictExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        private void SetHeading(ISheet sheet, string title)
        {
            CreateBoldCell(sheet, 0, 0, title, HorizontalAlignment.Center);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7));
        }

        public FileDto ExportMatrizToFile(List<SocialConflictMatrizExportDto> records)
        {
            return CreateExcelPackage("CASOS_CONFLICTIVIDAD.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS");
                var initRow = 0;

                SetHeading(sheet, "Listado de Conflictos (Matriz de Casos)");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de caso",
                "Nombre del caso detallado",
                "Nombre del caso resumido",
                "Problemática",
                "Nivel de riesgo",
                "Fecha",
                "Observación",
                "Estado del caso",
                "Fecha de estado",
                "Observación",
                "Unidad Territorial",
                "Departamento",
                "Provincia",
                "Distrito",
                "Centro Poblado",
                "Localidad-Comunidad-Otros",
                "Cobertura geográfica",
                "Coordinador de la UT",
                "Responsable del caso",
                "Responsable de la SSPI (Analista)",
                "Espacio de diálogo",
                "Tipología del conflicto",
                "Tipología detallada",
                "Sector responsable del ejecutivo",
                "Nivel de gobierno",
                "Actor",
                "Posición",
                "Interés",
                "Fecha",
                "Tipo de gestión",
                "Gestión",
                "Responsable de la gestión",
                "Fecha",
                "Situación actual (Interna)",
                "Proyección y acciones propuestas",
                "Persona que registra",
                "Estado (Nombre del caso detallado)",
                "Estado (Nombre del caso resumido)",
                "Estado (Problemática)",
                "Estado (Nivel de riesgo)",
                "Estado (Estado del caso)",
                "Estado (Gestión realizada)",
                "Estado (Situación actual)",
                "Registrado por",
                "Fecha de registro",
                "Última actualización por",
                "Última Fecha de actualización");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Description),
                _ => new ExportCell(_.Problem),
                //Nivel de Riesgo
                _ => new ExportCell(_.LastRisk),
                _ => new ExportCell(_.LastRiskTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastRiskDescription),
                //Estado del caso
                _ => new ExportCell(_.LastCondition),
                _ => new ExportCell(_.LastConditionTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastConditionDescription),
                //Aspectos generales
                _ => new ExportCell(_.TerritorialUnits),
                _ => new ExportCell(_.Departments),
                _ => new ExportCell(_.Provinces),
                _ => new ExportCell(_.Districts),
                _ => new ExportCell(_.Regions),
                _ => new ExportCell(_.Ubications),
                _ => new ExportCell(_.GeographicType == GeographycType.National ? "Nacional" : _.GeographicType == GeographycType.Location ? "Regional" : "Local"),
                _ => new ExportCell(_.CoordinatorName),
                _ => new ExportCell(_.ManagerName),
                _ => new ExportCell(_.AnalystName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.TypologyDescription),
                _ => new ExportCell(_.SubTypologyDescription),
                _ => new ExportCell(_.SectorDescription),
                _ => new ExportCell(_.GovernmentLevel == GovernmentLevel.National ? "Nacional" : _.GovernmentLevel == GovernmentLevel.Region ? "Regional" : _.GovernmentLevel == GovernmentLevel.Location ? "Local" : ""),
                //Actores
                _ => new ExportCell(_.ActorDescriptions),
                _ => new ExportCell(_.ActorPositions),
                _ => new ExportCell(_.ActorInterests),
                //Gestiones realizada
                _ => new ExportCell(_.LastManagementTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastManagement),
                _ => new ExportCell(_.LastConditionDescription),
                _ => new ExportCell(_.LastManagementManager),
                //Situación actual
                _ => new ExportCell(_.LastStateTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastState),
                _ => new ExportCell(_.LastStateDescription),
                _ => new ExportCell(_.LastStateManager),
                //Estado (Aprobado/No aprobado)
                _ => new ExportCell(_.CaseNameVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.DescriptionVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.ProblemVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.RiskVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.ConditionVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.ManagementVerification ? "Aprobado" : "No aprobado"),
                _ => new ExportCell(_.StateVerification ? "Aprobado" : "No aprobado"),
                //Aspectos generales
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastModificationUser),
                _ => new ExportCell(_.LastModificationTime, "dd/mm/yyyy", ExportCellType.Date)
                );


                for (var i = 0; i < records.Count; i++)
                {
                    if (sheet.GetRow(i + initRow) != null)
                    {
                        if (records[i].LastRiskTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[5], records[i].LastRiskTime.Value, "dd/mm/yyyy");
                        if (records[i].LastStateTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[8], records[i].LastStateTime.Value, "dd/mm/yyyy");
                        if (records[i].LastManagementTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[28], records[i].LastManagementTime.Value, "dd/mm/yyyy");
                        if (records[i].LastStateTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[32], records[i].LastStateTime.Value, "dd/mm/yyyy");

                        SetCellDataFormat(sheet.GetRow(i + initRow).Cells[44], records[i].CreationTime, "dd/mm/yyyy");

                        if (records[i].LastModificationTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[46], records[i].LastModificationTime.Value, "dd/mm/yyyy");
                    }
                }

                for (var i = 0; i < 46; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }

        public FileDto ExportManagementToFile(List<SocialConflictManagementExportDto> records)
        {
            return CreateExcelPackage("GESTIONES_REALIZADAS.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("GESTIONES");
                var initRow = 0;

                SetHeading(sheet, "Listado de Conflictos (Gestiones Realizadas)");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de caso",
                "Nivel de riesgo",
                "Fecha (Nivel de riesgo)",
                "Observación (Nivel de riesgo)",
                "Estado del caso",
                "Fecha de estado",
                "Nombre del caso detallado",
                "Unidad Territorial",
                "Departamento",
                "Provincia",
                "Distrito",
                "Centro Poblado",
                "Localidad-Comunidad-Otros",
                "Fecha (Gestión realizada)",
                "Tipo de gestión",
                "Gestión",
                "Personas de sociedad civil - N° Hombres",
                "Personas de sociedad civil - N° Mujeres",
                "Funcionarios del estado - N° Hombres",
                "Funcionarios del estado - N° Mujeres",
                "Personas de la empresa - N° Hombres",
                "Personas de la empresa - N° Mujeres",
                "Responsable de la gestión",
                "Estado (Aprobado /No aprobado)");

                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.CaseCode),
                //Nivel de Riesgo
                _ => new ExportCell(_.LastCaseRisk),
                _ => new ExportCell(_.LastCaseRiskTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastCaseRiskDescription),
                //Estado del Caso
                _ => new ExportCell(_.LastCaseCondition),
                _ => new ExportCell(_.LastCaseConditionTime, "dd/mm/yyyy", ExportCellType.Date),
                //Aspectos generales
                _ => new ExportCell(_.CaseName),
                //Ubicación
                _ => new ExportCell(_.TerritorialUnits),
                _ => new ExportCell(_.Departments),
                _ => new ExportCell(_.Provinces),
                _ => new ExportCell(_.Districts),
                _ => new ExportCell(_.Regions),
                _ => new ExportCell(_.Ubications),
                //Gestiones realizadas
                _ => new ExportCell(_.ManagementTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.Management),
                _ => new ExportCell(_.ManagementDescription),
                _ => new ExportCell(_.CivilMen.HasValue ? $"{_.CivilMen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.CivilWomen.HasValue ? $"{_.CivilMen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.StateMen.HasValue ? $"{_.StateMen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.StateWomen.HasValue ? $"{_.StateWomen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.CompanyMen.HasValue ? $"{_.CompanyMen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.CompanyWomen.HasValue ? $"{_.CompanyWomen.Value}" : "", ExportCellType.Numeric),
                _ => new ExportCell(_.ManagementManager),
                _ => new ExportCell(_.Verification ? "Aprobado" : "No aprobado")
                );

                for (var i = 0; i < records.Count; i++)
                {
                    if (sheet.GetRow(i + initRow) != null)
                    {
                        if (records[i].LastCaseRiskTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[2], records[i].LastCaseRiskTime.Value, "dd/mm/yyyy");
                        if (records[i].LastCaseConditionTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[5], records[i].LastCaseConditionTime.Value, "dd/mm/yyyy");

                        SetCellDataFormat(sheet.GetRow(i + initRow).Cells[13], records[i].ManagementTime, "dd/mm/yyyy");
                    }
                }

                for (var i = 0; i < 23; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }

        public FileDto ExportStateToFile(List<SocialConflictStateExportDto> records)
        {
            return CreateExcelPackage("SITUACION_ACTUAL.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("SITUACIÓN_ACTUAL");
                var initRow = 0;

                SetHeading(sheet, "Listado de Conflictos (Situación Actual)");
                initRow++;


                AddHeader(sheet, initRow,
                "Código de caso",
                "Nivel de riesgo",
                "Fecha (Nivel de riesgo)",
                "Observación (Nivel de riesgo)",
                "Estado del caso",
                "Fecha de estado",
                "Nombre del caso detallado",
                "Unidad Territorial",
                "Departamento",
                "Provincia",
                "Distrito",
                "Centro Poblado",
                "Localidad-Comunidad-Otros",
                "Fecha (Situación actual)",
                "Situación actual (Interna)",
                "Proyección y acciones propuestas",
                "Persona que registra",
                "Estado (Aprobado /No aprobado)");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.CaseCode),
                //Nivel de Riesgo
                _ => new ExportCell(_.LastCaseRisk),
                _ => new ExportCell(_.LastCaseRiskTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.LastCaseRiskDescription),
                //Estado del Caso
                _ => new ExportCell(_.LastCaseCondition),
                _ => new ExportCell(_.LastCaseConditionTime, "dd/mm/yyyy", ExportCellType.Date),
                //Aspectos generales
                _ => new ExportCell(_.CaseName),
                //Ubicación
                _ => new ExportCell(_.TerritorialUnits),
                _ => new ExportCell(_.Departments),
                _ => new ExportCell(_.Provinces),
                _ => new ExportCell(_.Districts),
                _ => new ExportCell(_.Regions),
                _ => new ExportCell(_.Ubications),
                //Situacion actual
                _ => new ExportCell(_.StateTime, "dd/mm/yyyy", ExportCellType.Date),
                _ => new ExportCell(_.State),
                _ => new ExportCell(_.StateDescription),
                _ => new ExportCell(_.StateManager),
                _ => new ExportCell(_.Verification ? "Aprobado" : "No aprobado")
                );

                for (var i = 0; i < records.Count; i++)
                {
                    if (sheet.GetRow(i + initRow) != null)
                    {
                        if (records[i].LastCaseRiskTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[2], records[i].LastCaseRiskTime.Value, "dd/mm/yyyy");
                        if (records[i].LastCaseConditionTime.HasValue)
                            SetCellDataFormat(sheet.GetRow(i + initRow).Cells[5], records[i].LastCaseConditionTime.Value, "dd/mm/yyyy");

                        SetCellDataFormat(sheet.GetRow(i + initRow).Cells[13], records[i].StateTime, "dd/mm/yyyy");
                    }
                }

                for (var i = 0; i < 18; i++)
                    sheet.SetColumnWidth(i, 5000);
            });
        }

        public FileDto ExportActorToFile(List<SocialConflictActorExcelExportDto> records)
        {
            return CreateExcelPackage("ACTORES.xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("ACTORES");
                var initRow = 0;

                SetHeading(sheet, "Listado de Actores de conflictos sociales");
                initRow++;

                AddHeader(sheet, initRow,
                "Nombre y Apellidos",
                "DNI",
                "Cargo",
                "Institución",
                "Número de Teléfono",
                "Tipo de Actor",
                "Capacidad de Movilización",
                "Código del conflicto",
                "Nombre del conflicto",
                "Regiones",
                "Tipo de conflicto");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Name),
                _ => new ExportCell(_.Document),
                _ => new ExportCell(_.Job),
                _ => new ExportCell(_.Community),
                _ => new ExportCell(_.PhoneNumber),
                _ => new ExportCell(_.ActorType),
                _ => new ExportCell(_.ActorMovement),
                _ => new ExportCell(_.CaseCode),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Regions),
                _ => new ExportCell(_.Site)
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
                        for (var cell = 0; cell <= 10; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }
                
                sheet.SetColumnWidth(0, 10000);
                sheet.SetColumnWidth(1, 5000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 15000);
                sheet.SetColumnWidth(4, 5000);
                sheet.SetColumnWidth(5, 8000);
                sheet.SetColumnWidth(6, 8000);
                sheet.SetColumnWidth(7, 5000);
                sheet.SetColumnWidth(8, 25000);
                sheet.SetColumnWidth(9, 15000);
                sheet.SetColumnWidth(10, 5000);
            });
        }

        public FileDto ExportMatrizFactsToFile(List<SocialConflictMatrizFactsDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoHechos_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_HECHOS");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y sus hechos relevantes");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripcion Facts",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.DescriptionFact),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 15000);
                sheet.SetColumnWidth(5, 5000);
                sheet.SetColumnWidth(6, 5000);
            });
        }

        public FileDto ExportMatrizRecommendationsToFile(List<SocialConflictMatrizRecommendationsDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoRecomendaciones_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_RECOMENDACIONES");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y sus recomendaciones");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Recomendaciones",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.Recommendations),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 15000);
                sheet.SetColumnWidth(5, 5000);
                sheet.SetColumnWidth(6, 5000);
            });
        }

        public FileDto ExportGestionesRealizadasToFile(List<SocialConflictMatrizGestionesRealizadasDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoGestionRealizada_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_GESTION_REALIZADA");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y sus gestiones realizadas");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripción- Gestion",
                "Nro. Civil- Hombres",
                "Nro. Civil- Mujeres",
                "Nro. Estado - Hombres",
                "Nro. Estado - Mujer",
                "Nro. Compañia - Hombres",
                "Nro. Compañía - Mujeres",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.GestionDescription),
                _ => new ExportCell(_.NroCivilMen),
                _ => new ExportCell(_.NroCivilWoMen),
                _ => new ExportCell(_.NroStateMen),
                _ => new ExportCell(_.NroStateWoMen),
                _ => new ExportCell(_.NroCompanyMen),
                _ => new ExportCell(_.NroCompanyWoMen),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 20000);
                sheet.SetColumnWidth(5, 5000);
                sheet.SetColumnWidth(6, 5000);
                sheet.SetColumnWidth(7, 5000);
                sheet.SetColumnWidth(8, 5000);
                sheet.SetColumnWidth(9, 5000);
                sheet.SetColumnWidth(10, 5000);
                sheet.SetColumnWidth(11, 7000);
                sheet.SetColumnWidth(12, 5000);
            });
        }

        public FileDto ExportHechosViolenciaToFile(List<SocialConflictMatrizHechosViolenciaDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoHechoViolencia_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_HECHOS_DE_VIOLENCIA");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y hechos de violencia");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripción- Violencia",
                "Responsable - Violencia",
                "Acciones",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.ViolenciaDescription),
                _ => new ExportCell(_.ViolenciaResponsable),
                _ => new ExportCell(_.Acciones),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 20000);
                sheet.SetColumnWidth(5, 20000);
                sheet.SetColumnWidth(6, 20000);
                sheet.SetColumnWidth(7, 7000);
                sheet.SetColumnWidth(8, 5000);
            });
        }

        public FileDto ExportSituacionActualToFile(List<SocialConflictMatrizSituationDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoSituacionActual_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_SITUACIÓN_ACTUAL");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y su situación actual");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripción- Situación",
                "Estado - Situación",
                "Estado - Manager",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.DescriptionSituation),
                _ => new ExportCell(_.StateSituation),
                _ => new ExportCell(_.StateManager),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 20000);
                sheet.SetColumnWidth(5, 20000);
                sheet.SetColumnWidth(6, 20000);
                sheet.SetColumnWidth(7, 7000);
                sheet.SetColumnWidth(8, 5000);
            });
        }

        public FileDto ExportRiskActualToFile(List<SocialConflictMatrizRiskDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoNivelRiesgo_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_NIVEL_RIESGO");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y su nivel de riesgo");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripción del Riesgo",
                "Nivel de Estado de Riesgo",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.DescriptionRisk),
                _ => new ExportCell(_.NivelRisk),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 20000);
                sheet.SetColumnWidth(5, 5000);
                sheet.SetColumnWidth(6, 6000);
                sheet.SetColumnWidth(7, 5000);
            });
        }

        public FileDto ExportEstadoActualToFile(List<SocialConflictMatrizEstadoActualDto> records)
        {
            DateTime fecha = DateTime.Now;
            string fechaExcel = fecha.ToString();
            return CreateExcelPackage("CasoEstadoActual_" + fechaExcel + ".xlsx", excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("CASOS_ESTADO_ACTUAL");
                var initRow = 0;

                SetHeading(sheet, "Listado de casos y su estadoo actual");
                initRow++;

                AddHeader(sheet, initRow,
                "Código de Caso",
                "Nombre de Caso",
                "Diálogo",
                "Problema",
                "Descripción- Estado",
                "Creador Usuario",
                "Fecha Creación");
                initRow++;

                AddObjects(excelPackage, sheet, initRow, records,
                //Aspectos generales
                _ => new ExportCell(_.Code),
                _ => new ExportCell(_.CaseName),
                _ => new ExportCell(_.Dialog),
                _ => new ExportCell(_.Problem),
                _ => new ExportCell(_.Description),
                _ => new ExportCell(_.CreatorUser),
                _ => new ExportCell(_.CreationTime.ToString())
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
                        for (var cell = 0; cell <= 6; cell++)
                        {
                            sheet.GetRow(i + initRow).Cells[cell].CellStyle = cellCentertTopAlignment;
                        }
                    }
                }

                sheet.SetColumnWidth(0, 5000);
                sheet.SetColumnWidth(1, 10000);
                sheet.SetColumnWidth(2, 15000);
                sheet.SetColumnWidth(3, 20000);
                sheet.SetColumnWidth(4, 20000);
                sheet.SetColumnWidth(5, 6000);
                sheet.SetColumnWidth(6, 5000);
            });
        }
    }
}
