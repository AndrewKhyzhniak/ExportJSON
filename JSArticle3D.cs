using Eplan.EplApi.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    public class JSArticle3D
    {
        public string PartNr;
        public string Type = "3D";
        public List<JSProperty> Properties = new List<JSProperty>();

        public JSArticle3D(Article article)
        {
            this.PartNr = article.PartNr;
            AnyPropertyId[] propertyIds = article.Properties.ExistingIds;

            foreach (AnyPropertyId propertyId in propertyIds)
            {
                PropertyValue property = article.Properties[propertyId];
                JSONAccept(property);
            }
        }

        private void JSONAccept(PropertyValue property)
        {
            if (!property.Definition.IsIndexed)
            {
                if (!property.IsEmpty)
                {
                    switch (property.Definition.Type)
                    {
                        case PropertyDefinition.PropertyType.Long:
                        case PropertyDefinition.PropertyType.Double:
                        case PropertyDefinition.PropertyType.ValueWithUnit:
                        case PropertyDefinition.PropertyType.Bool:
                            if (property.ToDouble() != 0)
                                Properties.Add(new JSProperty(property));
                            break;
                        default:
                            if (!string.IsNullOrEmpty(property.ToString()))
                                Properties.Add(new JSProperty(property));
                            break;
                    }
                }
            }
            else
            {

            }
        }
    }
}
