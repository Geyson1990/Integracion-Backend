using Abp.Application.Services;
using Contable.Application.DirectoryConflictTypes.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryConflictTypeExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryConflictTypeExcelExportDto> records, bool varCheckName, bool varCheckEnabled);
    }
}
