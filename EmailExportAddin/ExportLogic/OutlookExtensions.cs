using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ExportLogic
{
    public static class OutlookExtensions
    {
        const string PrAttachFlags = "http://schemas.microsoft.com/mapi/proptag/0x37140003";
        const string PrContentId = "http://schemas.microsoft.com/mapi/proptag/0x3712001E";

        public static bool HasAnyAttachment(this MailItem mail)
        {
            Attachments attachments = null;
            try
            {
                attachments = mail.Attachments;
                if (attachments.Count == 0)
                    return false;
                for (int i = 1; i <= attachments.Count; i++)
                {
                    Attachment att = attachments[i];
                    bool result = IsNormalAttachment(att);
                    Marshal.ReleaseComObject(att);
                    if (result == true)
                        return true;
                }
                return false;
            }
            finally
            {
                if(attachments != null)
                    Marshal.ReleaseComObject(attachments);
            }
        }

        private static bool IsNormalAttachment(Attachment attachment)
        {
            try
            {
                var value = (int)attachment.PropertyAccessor.GetProperty(PrAttachFlags);
                if (value == 0)
                    return true;
                var cid = (string)attachment.PropertyAccessor.GetProperty(PrContentId);
                return value != 4 && (string.IsNullOrEmpty(cid) || !cid.StartsWith(attachment.FileName));
            }
            catch (COMException)
            {
                return true;
            }
        }
    }
}
