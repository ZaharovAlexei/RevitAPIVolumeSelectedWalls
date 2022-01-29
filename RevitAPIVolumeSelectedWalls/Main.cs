using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIVolumeSelectedWalls
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите элементы по грани");

            double volumeWall = 0;
            foreach (var selectedElement in selectedElementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Parameter volumeParameter = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                    volumeWall += volumeValue;
                }
            }

            TaskDialog.Show("Selection", $"Общий объем выбранных стен: {volumeWall} м³");
            return Result.Succeeded;
        }
    }
}
