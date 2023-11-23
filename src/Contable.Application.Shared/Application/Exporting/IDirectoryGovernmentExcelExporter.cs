using Abp.Application.Services;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryGovernmentExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryGovernmentExcelExportDto> records, bool varCheckName, bool varCheckShortName, bool varCheckAddress, bool varCheckPhoneNumber, bool varCheckUrl, bool varCheckTipo, bool varCheckSector, bool varCheckHabilitado);
    }
}
