using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace EmailExportAddin
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon1()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            switch (ribbonID)
            {
                case "Microsoft.Outlook.Mail.Read":
                    return GetResourceText("EmailExportAddin.Mail.Read.Ribbon.xml");
                case "Microsoft.Outlook.Explorer":
                    return GetResourceText("EmailExportAddin.Ribbon1.xml");
                default:
                    return string.Empty;
            }
            
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public void OnAction(IRibbonControl control)
        {
            System.Diagnostics.Debugger.Break();
            switch (control.Id)
            {
                case "MailReadSaveMailItem":         
                case "RibbonMenuSaveMailItems":         
                case "ContextMenuSaveMailItem":
                case "ContextMenuSaveMailItems":
                    ShowExportMailWindow(control);
                    break;               
            }           
        }

        private void ShowExportMailWindow(IRibbonControl control) 
        {
            List<MailItem> mails = new List<MailItem>();
            Explorer exp = null;
            Inspector inspector = null;
            Selection sel = null;
            inspector = control.Context as Inspector;
            if (inspector != null)
            {
                var mail = inspector.CurrentItem as MailItem;
                if (mail != null)
                    mails.Add(mail);
            }
            else
            {
                exp = control.Context as Explorer;
                if (exp != null)
                    sel = exp.Selection;
                else
                    sel = control.Context as Selection;
                if (sel != null)
                {
                    for (int i = 1; i <= sel.Count; i++)
                    {
                        var mail = sel[i] as MailItem;
                        if (mail != null)
                            mails.Add(mail);
                    }
                }
            }

            if (mails.Count > 0)
            {
                var cmd = new ExportMailsCommand { Mails = mails };
                cmd.Execute();
            }

            foreach (var mail in mails)
            {
                Marshal.ReleaseComObject(mail);
            }
            if (inspector != null)
                Marshal.ReleaseComObject(inspector);            
            if (sel != null)
                Marshal.ReleaseComObject(sel);
            if (exp != null)
                Marshal.ReleaseComObject(exp);            
            
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
