using Eplan.EplApi.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLanExportJSON
{
    public class JSRoot
    {
        public List<JSFunction> Functions = new List<JSFunction>();
        public List<JSArticleReference> ArticleReferences = new List<JSArticleReference>();
        public List<JSArticle> Articles3D = new List<JSArticle>();
    }

    public class JSRoot3D
    {
        public List<JSFunction3D> Functions = new List<JSFunction3D>();
        public List<JSArticleReference3D> ArticleReferences = new List<JSArticleReference3D>();
        public List<JSArticle3D> Articles3D = new List<JSArticle3D>();
    }
}
