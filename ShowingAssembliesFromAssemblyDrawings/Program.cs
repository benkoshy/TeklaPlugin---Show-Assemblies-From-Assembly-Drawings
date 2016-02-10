using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG3D = Tekla.Structures.Geometry3d;
using TSD = Tekla.Structures.Drawing;
using TS = Tekla.Structures;
using Tekla.Structures.Model.Operations;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;


namespace ShowingAssembliesFromAssemblyDrawings
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAssembly();
        }

        // This application assumes that users have already opened a drawing - an assembly drawing.
        // The goal is for the user to see that particular assembly
        // in the model, and to see ONLY that assembly in the model.
        // Therefore all parts which are not associated with this assembly need to be hidden.
        // All the clutter needs to be removed from view.
        // And all parts associated with this assembly need to be visible.

        // Summary: our goals is for users to see, given the particular assembly drawing that they are in, 
        // just that ONE assembly within the model, and to have all other parts hidden.

        // Methodology: 
        // 1. We obtain the assembly drawing and go through all the drawing objects within the assembly drawing. We get the identifiers of these
        ////  drawing objects and place them in a list (assemblyPartIdentifierList)
        // 2. We obtain the identifiers of all the objects in the model and place them on this list too.
        ///3. We want to hide all objects which are not associated with the assembly drawing which is currently open.
        ///4. Consequently, we obtain a List of identifiers which contains all the objects in the model APART from the assembly parts of the current assembly
        ///// we are working on.
        ///5. We convert this list of identifiers into a TSM.Model Object list, and then again convert this list into an ArrayList.
        ///6. The array list should contain all the objects we need to hide.
        ///7. We use the array list to select all the objects which need to be hidden, in the model.
        ///8. We want to call a macro which hides all the objects currently selected.

        // Two questions, (1) is the above approach a good and efficient wa to solve the above problem>
        // Given a drawing is open, how can one call a macro which hides all selected objects?
        // Your comemnts and criticism would be very much apprieciated.


        private static void GetAssembly()
        {
            try
            {
                // creates model and drawing handle

                TSM.Model myModel = new TSM.Model();

                TSD.DrawingHandler dh = new TSD.DrawingHandler();

                if (dh.GetConnectionStatus() == false)
                {
                    System.Windows.Forms.MessageBox.Show("Could not connect drawing handler.");
                    return;
                }

                // users must have an Assembly drawing open. We want to see the assembly contained in the assembly drawing in the
                // UI. Hence we first obtain the identifiers of the drawing objects.

                TSD.Drawing currentDrawing = dh.GetActiveDrawing();

                if (currentDrawing == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please ensure that an assembly drawing is open");
                    return;
                }

                TSD.AssemblyDrawing currentAssemblyDrawing = currentDrawing as TSD.AssemblyDrawing;

                if (currentAssemblyDrawing == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please ensure that an assembly drawing is open");
                    return;
                }

                if (currentAssemblyDrawing != null)
                {
                    // in order to select in the model space we need to create an array list

                    ArrayList assemblyObjectToBeViewed = new ArrayList();

                    // we need to add a model object to the array list. We are adding the assembly as the model object for the model object selector to select                    
                    
                    assemblyObjectToBeViewed.Add(myModel.SelectModelObject(currentAssemblyDrawing.AssemblyIdentifier));

                    TSM.UI.ModelObjectSelector selector = new TSM.UI.ModelObjectSelector();
                    selector.Select(assemblyObjectToBeViewed);

                    TSM.Operations.Operation.RunMacro("ShowOnlySelected.cs");                    
                    TSM.Operations.Operation.RunMacro("FitToView.cs");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please ensure you have an Assembly drawing open");
                }
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }    
    }
}
