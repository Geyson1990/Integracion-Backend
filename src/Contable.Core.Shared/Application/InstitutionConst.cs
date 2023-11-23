using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application
{
    public class InstitutionConst
    {
        public const int NameMinLength = 0;
        public const int NameMaxLength = 1000;
        public const string NameType = "VARCHAR(1000)";

        public const int ShortNameMinLength = 0;
        public const int ShortNameMaxLength = 100;
        public const string ShortNameType = "VARCHAR(100)";

        public const int RucMinLength = 0;
        public const int RucMaxLength = 100;
        public const string RucType = "VARCHAR(100)";

        public const int TokentMinLength = 0;
        public const int TokentLength = 1000;
        public const string TokentType = "VARCHAR(1000)";

        public const int ContacNameMinLength = 0;
        public const int contacNameMaxLength = 255;
        public const string ContacNameType = "VARCHAR(255)";

        public const int EmailAddressMinLength = 0;
        public const int EmailAddressMaxLength = 255;
        public const string EmailAddressType = "VARCHAR(255)";

        public const int PhoneNumberMinLength = 0;
        public const int PhoneNumberMaxLength = 255;
        public const string PhoneNumberType = "VARCHAR(255)";
        
        public const string SectorIdType = "INT";
    }
}
