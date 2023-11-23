﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application
{
    public class DialogSpaceConsts
    {
        public const string SocialConflictIdType = "INT";
        public const string PersonIdType = "INT";
        public const string LastDialogSpaceDocumentIdType = "INT";
        
        public const int CodeMinLength = 0;
        public const int CodeMaxLength = 20;
        public const string CodeType = "VARCHAR(20)";

        public const int CaseNameMinLength = 0;
        public const int CaseNameMaxLength = 5000;
        public const string CaseNameType = "VARCHAR(5000)";

        public const string YearType = "INT";
        public const string CountType = "INT";
        public const string GenerationType = "BIT";
        public const string DialogSpaceTypeIdType = "INT";

        public const string TimeType = "DATETIME";
        public const string IntegerType = "INT";
    }
}
