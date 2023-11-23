using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application
{
    public class SocialConflictAlertSourcesConsts
    {
        public const string SocialConflictAlertIdType = "INT";
        public const string AlertRiskIdType = "INT";

        public const int SourceMinLength = 0;
        public const int SourceMaxLength = 1000;
        public const string SourceType = "VARCHAR(1000)";

        public const int SourceTypeMinLength = 0;
        public const int SourceTypeMaxLength = 1000;
        public const string SourceTypeType = "VARCHAR(1000)";

        public const int LinkMinLength = 0;
        public const int LinkMaxLength = 1000;
        public const string LinkType = "VARCHAR(1000)";
    }
}
