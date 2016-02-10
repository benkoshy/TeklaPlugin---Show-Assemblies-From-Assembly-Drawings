namespace Tekla.Technology.Akit.UserScript
{
    public class Script
    {
        public static void Run(Tekla.Technology.Akit.IScript akit)
        {
            // change the filepath below to where the .EXE file is located

            string DepFolder = @"c:\myDeploymentFolder\";
            string AppName = "ShowingAssembliesFromAssemblyDrawings.exe";

            if (System.IO.File.Exists(DepFolder + AppName))
            {
                System.Diagnostics.Process Process = new System.Diagnostics.Process();
                Process.EnableRaisingEvents = false;
                Process.StartInfo.FileName = DepFolder + AppName;
                Process.Start();
                Process.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Application .exe not found at the set location. It should exist here: " + DepFolder + "\n you can either change the path of where the exe is meant to be located by editing this .cs file using Notepad, or you can place the exe file in the correct location. Note: do not change the name of the exe file.");
            }
        }
    }
}