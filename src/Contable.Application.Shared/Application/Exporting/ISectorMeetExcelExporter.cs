using Abp.Application.Services;
using Contable.Application.HelpMemories.Dto;
using Contable.Application.Records.Dto;
using Contable.Application.SectorMeets.Dto;
using Contable.Application.SocialConflicts.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface ISectorMeetExcelExporter : IApplicationService
    {
        FileDto ExportSectorMeet(List<SectorMeetResumenReport> records, List<SectorMeetDetalleReport> detalle, List<SectorMeetEstadisticaReport> estadistica);


    }
}
