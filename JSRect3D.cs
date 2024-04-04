using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EPLanExportJSON
{
    public class JSRect3D
    {

        public double X; 
        public double Y;
        public double Z;

        public double SizeX;
        public double SizeY;
        public double SizeZ;

        public JSRect3D(Rect3D rect)
        {
            X = rect.X;
            Y = rect.Y;
            Z = rect.Z;

            SizeX = rect.SizeX;
            SizeY = rect.SizeY;
            SizeZ = rect.SizeZ;
        }
    }
}
