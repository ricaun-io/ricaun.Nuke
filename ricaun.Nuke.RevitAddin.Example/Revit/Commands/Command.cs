using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Xaml;
using System;

namespace ricaun.Nuke.RevitAddin.Example.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            _ = typeof(AmbientPropertyValue).Assembly;

            return Result.Succeeded;
        }
    }
}
