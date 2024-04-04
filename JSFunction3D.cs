using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.E3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace EPLanExportJSON
{
    public class JSFunction3D
    {
        public string VisibleName;
        public List<JSProperty> Properties = new List<JSProperty>();
        public List<JSArticle3D> Articles3D = new List<JSArticle3D>();
        public List<JSArticleReference3D> ArticleReferences3D = new List<JSArticleReference3D>();
        public JSRect3D BoundingBox;
        Rect3D OriginalBox;

        public JSFunction3D(Function3D function) 
        {
            VisibleName = function.VisibleName;
            OriginalBox = function.GetBoundingBox(true);
            BoundingBox = new JSRect3D(OriginalBox);

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
