using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    public class JSProperty
    {

        public string DisplayName;
        public string Name;
        public object Value;

        public JSProperty(PropertyValue property) 
        {
            DisplayName = property.Definition.Name;
            Name = property.Definition.Id.AsArticle.ToString();
            Value = property.GetDisplayString().GetStringToDisplay(ISOCode.Language.L___).ToString();
            //Value = property.ToString(ISOCode.Language.L___);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
