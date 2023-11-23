using Abp.Application.Services;
using Contable.Application.DirectoryDialogs.Dto;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryDialogExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryDialogExcelExportDto> records, bool varCheckName, bool varCheckLastName, bool varCheckMothersLastName, bool varCheckPost, bool varCheckEntity, bool varCheckWeb, bool varCheckLandline, bool varCheckCellPhone, bool checkEnabled);
    }
}
