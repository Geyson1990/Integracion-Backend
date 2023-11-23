using Abp.Application.Services;
using Contable.Application.DirectoryGovernments.Dto;
using Contable.Application.DirectoryIndustries.Dto;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.Exporting
{
    public interface IDirectoryIndustryExcelExporter : IApplicationService
    {
        FileDto ExportMatrizToFile(List<DirectoryIndustryExcelExportDto> records, bool checkNameEmpresa, bool checkSector, bool checkDireccion, bool checkTelefono, bool checkPaginaWeb, bool checkDepartamento, bool checkProvincia, bool checkDistrito, bool varCheckHabilitado);
    }
}
