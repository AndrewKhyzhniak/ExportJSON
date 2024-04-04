using Eplan.EplApi.Base.Enums;
using Eplan.EplApi.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    public class JSFunction
    {
        public string Name;
        public string VisibleName;
        public FunctionCategory Category;
        long ObjectIdentifier;
        public List<JSArticle> Articles = new List<JSArticle>();
        public List<JSArticleReference> ArticleReferences = new List<JSArticleReference>();
        public List<JSProperty> Properties = new List<JSProperty>();

        public JSFunction(Function function)
        {
            Name = function.Name;
            Category = function.FunctionCategory;
            ObjectIdentifier = function.ObjectIdentifier;
            VisibleName = function.VisibleName;

            AnyPropertyId[] propertyIds = function.Properties.ExistingIds;
            foreach (AnyPropertyId propertyId in propertyIds)
            {
                PropertyValue property = function.Properties[propertyId];
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
}
