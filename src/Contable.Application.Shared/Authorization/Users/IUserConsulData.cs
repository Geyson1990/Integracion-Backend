using Abp.Application.Services;
using Contable.Application.CrisisCommittees.Dto;
using Contable.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contable.Authorization.Users
{
    public interface IUserConsulData: IApplicationService
    {
        UserDNIDto GetUserDataDni(string Dni);
        string GetUserDataRuc(int Ruc);
    }
}
