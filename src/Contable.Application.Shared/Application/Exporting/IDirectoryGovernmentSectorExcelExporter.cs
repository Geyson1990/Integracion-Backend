using Abp.Application.Services;
using Contable.Application.DirectoryGovernmentSectors.Dto;
using Contable.Application.DirectoryGovernmentTypes.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryGovernmentSectorExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryGovernmentSectorExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
