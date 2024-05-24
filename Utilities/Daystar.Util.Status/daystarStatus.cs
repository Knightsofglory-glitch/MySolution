using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Daystar.Util.Status
{
    [Serializable]
    public class daystarStatus
    {
        public uint Status { get; set; }
        public Severity Severity { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }


        public daystarStatus()
        { }
        public daystarStatus(Severity severity)
        {
            initialize();
            Severity = severity;
        }
        public daystarStatus(Severity severity, string message)
        {
            initialize();
            Severity = severity;
            Message = message;
        }
        public daystarStatus(Severity severity, string module, string method, string message)
        {
            Severity = severity;
            Module = module;
            Method = method;
            Message = message;
        }
        public daystarStatus(uint status, Severity severity)
        {
            initialize();
            Status = status;
            Severity = severity;
        }
        public daystarStatus(uint status, string message)
        {
            initialize();
            Status = status;
            Message = message;
        }
        public daystarStatus(uint status, Severity severity, string message)
        {
            initialize();
            Status = status;
            Severity = severity;
            Message = message;
        }
        public daystarStatus(string statusMessage)
        {
            if (statusMessage.IndexOf('|') != 1)
            {
                throw new ArgumentException(String.Format("Invalid statusMessage: {0}", statusMessage));
            }
            string[] _ss = statusMessage.Split('|');
            switch (_ss[0])
            {
                case "D":
                    this.Severity = Severity.Debug;
                    break;
                case "I":
                    this.Severity = Severity.Success;
                    break;
                case "W":
                    this.Severity = Severity.Warning;
                    break;
                case "E":
                    this.Severity = Severity.Error;
                    break;
                case "F":
                    this.Severity = Severity.Fatal;
                    break;
                default:
                    throw new ArgumentException(String.Format("Invalid statusMessage Severity: {0}   for message: {1}", _ss[0], statusMessage));
            }
            this.Message = _ss[1];
            initialize();
        }

        private void initialize()
        {
            ////Type type = this.GetType();
            ////StackTrace stackTrace = new StackTrace();
            ////for (int i=0; i < 10; i++)
            ////{
            ////    MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();
            ////    if (methodBase.DeclaringType == null) { continue; }
            ////    if (type.FullName != methodBase.DeclaringType.FullName)
            ////    {
            ////        this.Module = methodBase.DeclaringType.FullName;
            ////        ParameterInfo[] parameterInfos = methodBase.GetParameters();
            ////        StringBuilder sbArgs = new StringBuilder();
            ////        for (int idx= 0; idx < parameterInfos.Length; idx++)
            ////        {
            ////            ParameterInfo pi = parameterInfos[idx];
            ////            if (pi.IsOut)
            ////            {
            ////                sbArgs.Append(" out ");
            ////            }
            ////            sbArgs.Append(pi.ParameterType.FullName + " " + pi.Name);
            ////            if (idx+1 < parameterInfos.Length)
            ////            {
            ////                sbArgs.Append(", ");
            ////            }
            ////        }
            ////        this.Method = methodBase.Name + "(" + sbArgs.ToString() + ")";
            ////        break;
            ////    }
            ////}
            ////this.Message = "";
        }

        ////public override string ToString()
        ////{
        ////    StringBuilder sbToString = new StringBuilder();
        ////    switch (this.Severity)
        ////    {
        ////        case Severity.Debug:
        ////            sbToString.Append("DEBUG: ");
        ////            break;
        ////        case Severity.Success:
        ////            sbToString.Append("SUCCESS: ");
        ////            break;
        ////        case Severity.Warning:
        ////            sbToString.Append("WARNING: ");
        ////            break;
        ////        case Severity.Error:
        ////            sbToString.Append("ERROR: ");
        ////            break;
        ////        case Severity.Fatal:
        ////            sbToString.Append("FATAL: ");
        ////            break;
        ////        default:
        ////            sbToString.Append("UNKNOWN: ");
        ////            break;
        ////    }
        ////    sbToString.Append(this.Message);
        ////    if (!string.IsNullOrEmpty(this.Module))
        ////    {
        ////        sbToString.Append(" " + this.Module);
        ////    }
        ////    if (!string.IsNullOrEmpty(this.Method))
        ////    {
        ////        sbToString.Append("." + this.Method);
        ////    }
        ////    return (sbToString.ToString());
        ////}
    }
}
