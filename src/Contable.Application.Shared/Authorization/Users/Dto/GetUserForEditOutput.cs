using System;
using System.Collections.Generic;
using Contable.Authorization.Institution.InstitutionDto;
using Contable.Organizations.Dto;

namespace Contable.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public UserAlertResponsibleDto[] Responsibles { get; set; }
        public GetInstitutionDto[] Institutions { get; set; }
    }
}