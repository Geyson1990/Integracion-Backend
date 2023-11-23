using Abp.Application.Services;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Application.DirectoryGovernmentTypes.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryGovernmentTypeExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryGovernmentTypeExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
