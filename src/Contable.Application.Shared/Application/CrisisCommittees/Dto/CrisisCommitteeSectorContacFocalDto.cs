using Abp.Application.Services.Dto;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application.CrisisCommittees.Dto
{
    public class CrisisCommitteeSectorContacFocalDto: EntityDto
    {
        public string Name { get; set; }     
        public string Cargo { get; set; }  
        public string EmailAddress { get; set; }        
        public string PhoneNumber { get; set; }
        public int Index { get; set; }
        public bool Remove { get; set; }

    }
}
