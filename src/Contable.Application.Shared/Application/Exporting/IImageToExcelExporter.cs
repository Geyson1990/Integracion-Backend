
using Abp.Application.Services;
using Abp.Runtime.Validation;
using Contable.Application;
using Contable.Dto;
using System;

namespace Contable.Application.Exporting
{
    public interface IImageToExcelExporter : IApplicationService
    {
        FileDto ExportImagenToFile(RequestParamDto request);

    }
}



public class RequestParamDto
{
    public string Base64Image { get; set; }
    public string nameImage { get; set; }

}