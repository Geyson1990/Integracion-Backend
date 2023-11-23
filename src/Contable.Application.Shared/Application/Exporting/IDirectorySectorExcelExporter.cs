using Abp.Application.Services;
using Contable.Application.DirectoryResponsibles.Dto;
using Contable.Application.DirectorySectors.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectorySectorExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectorySectorExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
