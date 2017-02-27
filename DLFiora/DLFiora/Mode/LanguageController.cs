using System.Collections.Generic;
using DLFiora.Model.Enum;

namespace DLFiora.Model
{
    abstract class LanguageController
    {
        public Dictionary<EnumContext, string> Dictionary = new Dictionary<EnumContext, string>();
    }
}
