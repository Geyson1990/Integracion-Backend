using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Authorization.Users.Dto
{
    public class UserDNIDto : EntityDto
    {
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombres { get; set; }
        public string DNI { get; set; }

    }
}
