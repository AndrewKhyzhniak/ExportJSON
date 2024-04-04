using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.E3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    internal class EPLANAddInAction :  IEplAction
    {

        ProjectManager projectManager = new ProjectManager();
        Project currentProject = null;

        class ArticleEqualityComparer : IEqualityComparer<Article>
        {
            public bool Equals(Article x, Article y)
            {
                return x.ObjectIdentifier == y.ObjectIdentifier;
            }

            public int GetHashCode(Article obj)
            {
                return 1;
            }
        }

        class ArticleReferenceEqualityComparer : IEqualityComparer<ArticleReference>
        {
            public bool Equals(ArticleReference x, ArticleReference y)
            {
                return x.ObjectIdentifier == y.ObjectIdentifier;
            }

            public int GetHashCode(ArticleReference obj)
            {
                return 1;
            }
        }

        class FunctionComparer : IEqualityComparer<Function>
        {
            public bool Equals(Function x, Function y)
            {
                return x.ObjectIdentifier == y.ObjectIdentifier;
            }

            public int GetHashCode(Function obj)
            {
                return 1;
            }
        }

        class Function3DComparer : IEqualityComparer<Function3D>
        {
            public bool Equals(Function3D x, Function3D y)
            {
                return x.ObjectIdentifier == y.ObjectIdentifier;
            }

            public int GetHashCode(Function3D obj)
            {
                return 1;
            }
        }

        //string[] testFilterArray = { "+MCC6.1", "+A3" };
        //string[] testFilterArray = { "+САУ" };
        string[] testFilterArray = { "+VC30J01" };

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            currentProject = projectManager.CurrentProject;

            ArticleEqualityComparer articleComparer = new ArticleEqualityComparer();
            ArticleReferenceEqualityComparer articleReferenceComparer = new ArticleReferenceEqualityComparer();
            FunctionComparer functionComparer = new FunctionComparer();
            Function3DComparer function3DComparer = new Function3DComparer();

            List<Function> currentFunctions = GetFilteredFunctions().Distinct(functionComparer).ToList();
            List<Function3D> currentFunctions3D = GetFiltered3DFunctions().Distinct(function3DComparer).ToList();

            JSRoot jSRoot = new JSRoot();
            JSRoot3D jSRoot3D = new JSRoot3D();

            Progress articleProgress = new Progress("Articles processed");
            articleProgress.SetTitle("Articles processed");
            articleProgress.ShowImmediately();

            int articleCount = currentFunctions.Count() + currentFunctions3D.Count();

            foreach (Function function in currentFunctions)
            {
                articleProgress.BeginPart(100.0 / articleCount, function.Name);

                JSFunction jsFunction = new JSFunction(function);
                jSRoot.Functions.Add(jsFunction);

                Article[] articles = function.Articles.Distinct(articleComparer).ToArray();

                foreach (Article article in articles)
                {
                    jsFunction.Articles.Add(new JSArticle(article));
                }

                foreach (ArticleReference articleReference in function.ArticleReferences.Distinct(articleReferenceComparer))
                {
                    jsFunction.ArticleReferences.Add(new JSArticleReference(articleReference));
                }

                articleProgress.EndPart();
            }

            foreach (Function3D function in currentFunctions3D)
            {
                articleProgress.BeginPart(100.0 / articleCount, function.Name);

                JSFunction3D jsFunction = new JSFunction3D(function);
                jSRoot3D.Functions.Add(jsFunction);

                Article[] articles = function.Articles.Distinct(articleComparer).ToArray();

                foreach (Article article in articles)
                {
                    jsFunction.Articles3D.Add(new JSArticle3D(article));
                }

                foreach (ArticleReference articleReference in function.ArticleReferences.Distinct(articleReferenceComparer))
                {
                    jsFunction.ArticleReferences3D.Add(new JSArticleReference3D(articleReference));
                }

                articleProgress.EndPart();
            }

            articleProgress.SetTitle("Сериализация JSON...");

            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            serializer.Serialize(writer, jSRoot);
            writer.Flush();

            articleProgress.EndPart(true);

            System.Windows.Forms.SaveFileDialog sDialog = new System.Windows.Forms.SaveFileDialog();
            sDialog.Filter = "JSON файлы|*.json";
            sDialog.DefaultExt = "json";

            if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(sDialog.FileName, writer.ToString());
            }

            Newtonsoft.Json.JsonSerializer serializer3D = new Newtonsoft.Json.JsonSerializer();
            serializer3D.Formatting = Newtonsoft.Json.Formatting.Indented;
            System.IO.StringWriter writer3D = new System.IO.StringWriter();
            serializer3D.Serialize(writer3D, jSRoot3D);
            writer3D.Flush();

            System.IO.File.WriteAllText(sDialog.FileName.Split('.')[0] + ".3D" + ".json", writer3D.ToString());

            return true;
        }

        private List<Function3D> GetFiltered3DFunctions()
        {
            Functions3DFilter functions3DFilter = new Functions3DFilter();
            Function3D[] functions3D = new DMObjectsFinder(currentProject).GetFunctions3D(functions3DFilter);

            foreach (string s in testFilterArray)
            {
                functions3D = functions3D.Where(f => f.Name.IndexOf(s) != -1).ToArray();
            }
            return functions3D.ToList();
        }

        private List<Function> GetFilteredFunctions()
        {
            List<Function> currentFunctions = currentProject.Pages.SelectMany(p => p.Functions).ToList();
            foreach (string s in testFilterArray)
            {
                currentFunctions = currentFunctions.Where(f => f.Name.IndexOf(s) != -1).ToList();
            }

            return currentFunctions;
        }

        private static void PrintArticle(Article article)
        {
            ArticlePropertyList properties = article.Properties;
            AnyPropertyId[] propertyIds = properties.ExistingIds;

            foreach (AnyPropertyId propertyId in propertyIds)
            {
                PrintProperty(properties, propertyId);
            }
        }

        private static void PrintProperty(ArticlePropertyList properties, AnyPropertyId propertyId)
        {
            if (!properties.Exists(propertyId))
                return;
            PropertyValue propertyValue = properties[propertyId];
            if (!propertyValue.Definition.IsIndexed)
            {
                PrintSimpleProperty(propertyValue);
            }
            else
            {
                int[] indexes = propertyValue.Indexes;
                if (indexes.Length != 0)
                {
                    foreach (int index in indexes)
                    {
                        if (!propertyValue[index].IsEmpty)
                        {
                            string displayString = propertyValue[index].GetDisplayString().GetStringToDisplay(ISOCode.Language.L___).ToString();
                            if (!string.IsNullOrWhiteSpace(displayString))
                                ;
                        }
                    }
                }
            }
        }

        private static void PrintSimpleProperty(PropertyValue propertyValue)
        {
            if (!propertyValue.IsEmpty)
            {
                string displayString = propertyValue.GetDisplayString().GetStringToDisplay(ISOCode.Language.L___).ToString();
                if (!string.IsNullOrWhiteSpace(displayString))
                { } //Debug.WriteLine($"{propertyValue.Definition.Id.AsArticle} = {propertyValue.Definition.Name} = {displayString}");
            }
        }

        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }

        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = EPLANAddIn.ACTIONID1;
            Ordinal = 20;
            return true;
        }
    }
}
