using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DirectoryGovernmentTypes.Dto
{
    public class DirectoryGovernmentTypeGetAllInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Name ASC";
            }
        }
        public bool CheckName { get; set; }
        public bool CheckIndice { get; set; }
        public bool CheckEnabled { get; set; }
    }
}
