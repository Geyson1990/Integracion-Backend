using Abp.Application.Services;
using Contable.Application.Records.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface ISocialConflictExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<SocialConflictMatrizExportDto> records);
        FileDto ExportManagementToFile(List<SocialConflictManagementExportDto> records);
        FileDto ExportStateToFile(List<SocialConflictStateExportDto> records);
        FileDto ExportActorToFile(List<SocialConflictActorExcelExportDto> records);
        FileDto ExportMatrizFactsToFile(List<SocialConflictMatrizFactsDto> records);
        FileDto ExportMatrizRecommendationsToFile(List<SocialConflictMatrizRecommendationsDto> records);
        FileDto ExportGestionesRealizadasToFile(List<SocialConflictMatrizGestionesRealizadasDto> records);
        FileDto ExportHechosViolenciaToFile(List<SocialConflictMatrizHechosViolenciaDto> records);
        FileDto ExportSituacionActualToFile(List<SocialConflictMatrizSituationDto> records);
        FileDto ExportRiskActualToFile(List<SocialConflictMatrizRiskDto> records);
        FileDto ExportEstadoActualToFile(List<SocialConflictMatrizEstadoActualDto> records);
        

    }
}
