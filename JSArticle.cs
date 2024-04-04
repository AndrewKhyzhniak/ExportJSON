using Eplan.EplApi.DataModel;
using Eplan.EplApi.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    public class JSArticle
    {
        public string PartNr;
        public string Type = "Flat";
        public List<JSProperty> Properties = new List<JSProperty>();

        public JSArticle(Article article)
        {

            PartNr = article.PartNr;
            AnyPropertyId[] propertyIds = article.Properties.ExistingIds;

            foreach (AnyPropertyId propertyId in propertyIds)
            {
                PropertyValue property = article.Properties[propertyId];
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

    public class JSArticleReference
    {

        public string IdentifyingName;
        public List<JSProperty> Properties = new List<JSProperty>();

        public JSArticleReference(ArticleReference articleReference)
        {
            IdentifyingName = articleReference.IdentifyingName;

            AnyPropertyId[] propertyIds = articleReference.Properties.ExistingIds;

            foreach (AnyPropertyId propertyId in propertyIds)
            {
                PropertyValue property = articleReference.Properties[propertyId];
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