using Abp.Application.Services;
using Contable.Application.HelpMemories.Dto;
using Contable.Application.Records.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IHelpMemoryExcelExporter : IApplicationService
    {
        FileDto ExportHelpMemory(List<HelpMemoryResumenReport> records, List<HelpMemoryDetalleReport> detalle, List<HelpMemoryDetalleEstadisticaReport> estadistica);


    }
}
