using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryIndustries.Dto
{
    public class DirectoryIndustryGetAllInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Name ASC";
            }
        }
        public bool checkNameEmpresa { get; set; }
        public bool checkSector { get; set; }
        public bool checkDireccion { get; set; }
        public bool checkTelefono { get; set; }
        public bool checkPaginaWeb { get; set; }
        public bool checkDepartamento { get; set; }
        public bool checkProvincia { get; set; }
        public bool checkDistrito { get; set; }
        public bool CheckHabilitado { get; set; }
    }
}
