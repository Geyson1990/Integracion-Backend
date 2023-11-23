using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Contable.Application.SocialConflictAlerts.Dto
{
    public class SocialConflictAlertSourcesLocationDto : EntityDto
    {

        public int SocialConflictAlertId { get; set; }
        public string Source { get; set; }
        public string SourceType { get; set; }
        public string Link { get; set; }

        public bool Remove { get; set; }
    }

}
