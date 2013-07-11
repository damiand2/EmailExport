using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ExportLogic.ExportMailsCommand cmd = new ExportLogic.ExportMailsCommand {  Mails = new MailItem[0]};
            cmd.Execute();
        }
    }
}
