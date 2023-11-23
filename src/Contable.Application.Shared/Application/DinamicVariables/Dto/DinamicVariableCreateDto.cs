using System;
using System.Collections.Generic;
using System.Text;

namespace Contable.Application.DinamicVariables.Dto
{
    public class DinamicVariableCreateDto
    {
        public string Name { get; set; }
        public int InstitutionId { get; set; }
        public DinamicVariableType Type { get; set; }
    }
}
