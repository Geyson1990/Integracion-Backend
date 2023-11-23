
using Abp.UI;
using Contable.DataExporting.Excel.NPOI;
using Contable.Dto;
using Contable.Storage;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;

namespace Contable.Application.Exporting
{
    public class ImageToExcelExporter : NpoiExcelExporterBase, IImageToExcelExporter
    {
        public ImageToExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }
        public static byte[] Base64ToByteArray(string base64)
        {
            // Comprueba si la cadena es un archivo PNG
            if (base64.StartsWith("data:image/png;base64,"))
            {
                // La cadena es un archivo PNG
                return Convert.FromBase64String(base64.Substring(22));
            }
            else if (base64.StartsWith("data:image/svg+xml;base64,"))
            {
                // La cadena es un archivo SVG
                return Convert.FromBase64String(base64.Substring(24));
            }
            else
            {
                // La cadena no es un archivo PNG ni SVG
                throw new ArgumentException("La cadena no es un archivo PNG ni SVG");
            }
        }
        private void SetHeading(XSSFWorkbook excelPackage, ISheet sheet, string stringInBase64)
        {
    
            byte[] data = Base64ToByteArray(stringInBase64);
            int pictureIndex = excelPackage.AddPicture(data, PictureType.PNG);
            IClientAnchor anchor = excelPackage.GetCreationHelper().CreateClientAnchor();
            anchor.Row1 = 1;
            anchor.Col1 = 1;
            sheet.CreateDrawingPatriarch().CreatePicture(anchor, pictureIndex).Resize();
        }

        public FileDto ExportImagenToFile(RequestParamDto request)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string fechaExcel = fecha.ToString();
                return CreateExcelPackage(
                    request.nameImage + "_" + fechaExcel + ".xlsx",
                        excelPackage =>
                        {
                            var sheet = excelPackage.CreateSheet(request.nameImage);
                            SetHeading(excelPackage, sheet, request.Base64Image);
                        }
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Aviso", "Error al soliocitar el documento excel");
            }
        }
    }
}
