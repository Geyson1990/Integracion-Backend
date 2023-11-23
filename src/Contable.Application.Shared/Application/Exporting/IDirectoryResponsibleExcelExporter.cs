using Abp.Application.Services;
using Contable.Application.DirectoryGovernmentSectors.Dto;
using Contable.Application.DirectoryResponsibles.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryResponsibleExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryResponsibleExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
