using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryIndustries.Dto
{
    public class DirectoryIndustryExcelExportDto : EntityDto
    {
        public string NameEmpresa { get; set; }
        public string Sector { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Habilitado { get; set; }

        public bool checkNameEmpresa { get; set; }
        public bool checkSector { get; set; }
        public bool checkDireccion { get; set; }
        public bool checkTelefono { get; set; }
        public bool checkPaginaWeb { get; set; }
        public bool checkDepartamento { get; set; }
        public bool checkProvincia { get; set; }
        public bool checkDistrito { get; set; }
        public bool checkHabilitado { get; set; }
    }
}
