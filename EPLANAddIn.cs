using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.Gui;
using System.Windows;

namespace EPLanExportJSON
{
    public class EPLANAddIn : IEplAddIn
    {
        internal const string ACTIONID1 = "{AF9FB713-7120-4F73-A47E-C47F132C2EE0}";
        internal const string JUNIORTAB = "Export JSON";

        public bool OnExit()
        {
            return true;
        }

        public bool OnInit()
        {
            return true;
        }

        public bool OnInitGui()
        {
            try
            {
                RibbonBar jrRibbon = new RibbonBar();
                if (jrRibbon.Tabs.Any(t => t.Name == JUNIORTAB))
                    throw new Exception($"Tab named {JUNIORTAB} already exists");
                RibbonTab jrRibbonTab = jrRibbon.AddTab(JUNIORTAB);
                RibbonCommandGroup jrCommandGroup = jrRibbonTab.AddCommandGroup("Export JSON");
                jrCommandGroup.AddCommand("Export JSON", ACTIONID1, CommandIcon.Accumulator);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
            return true;
        }
        public bool OnRegister(ref bool bLoadOnStart)
        {
            try
            {
                bLoadOnStart = true;
                RibbonBar jrRibbon = new RibbonBar();
                if (jrRibbon.Tabs.Any(t => t.Name == JUNIORTAB))
                    throw new Exception($"Tab named {JUNIORTAB} already exists");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public bool OnUnregister()
        {
            RibbonBar jrRibbon = new RibbonBar();
            RibbonTab jrTab = jrRibbon.GetTab(JUNIORTAB);
            jrTab.Remove();
            return true;
        }
    }

}
