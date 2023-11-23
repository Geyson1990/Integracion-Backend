using Abp.Extensions;
using Abp.Runtime.Validation;
using Contable.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.ProspestiveRisks.Dto
{
    public class ProspectiveRiskGetAllDateInputDto : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime startDate { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Department.Name ASC, Province.Name ASC";
            }
        }
    }
}
