using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioneNet
{
    class GlobalVariables
    {
        private static string _globalVar = "";
        private static string _DocToSendMail = "";
        private static string _DocRefNo = "";
        private static string _docName = "";
        private static string _TransType = "";
        private static string _attrdesc = "";
        public static string FormName
        {
            get { return _globalVar; }
            set { _globalVar = value; }
        }
        public static string doctosend
        {
            get { return _DocToSendMail; }
            set { _DocToSendMail = value; }
        }
        public static string docRefNo
        {
            get { return _DocRefNo; }
            set { _DocRefNo = value; }
        }
        public static string docName
        {
            get { return _docName; }
            set { _docName = value; }
        }
        public static string TransType
        {
            get { return _TransType; }
            set { _TransType = value; }
        }

        public static string attrdesc
        {
            get { return _attrdesc; }
            set { _attrdesc = value; }
        }
    }
}
