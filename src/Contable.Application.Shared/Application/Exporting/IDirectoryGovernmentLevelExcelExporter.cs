using Abp.Application.Services;
using Contable.Application.DirectoryGovernmentLevels.Dto;
using Contable.Application.DirectorySectors.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryGovernmentLevelExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryGovernmentLevelExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
