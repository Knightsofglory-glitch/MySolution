using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jon.Util.Status
{
    public static class jonStatusDef
    {
        #region Severity Handlers
        //
        // Severity Handlers
        //
        public static bool IsSuccess(int serverResponseStatus)
        {
            return (serverResponseStatus == 1);
        }
        public static bool IsSuccessOrWarning(int serverResponseStatus)
        {
            return (serverResponseStatus == 1 || serverResponseStatus == 2);
        }
        public static bool IsWarning(int serverResponseStatus)
        {
            return (serverResponseStatus == 2);
        }
        public static bool IsError(int serverResponseStatus)
        {
            return (serverResponseStatus > 2);
        }
        #endregion


        #region daystarStatus Methods
        //
        // daystarStatus Methods
        //
        public static bool IsSuccess(uint status)
        {
            uint result = status & Constants.SeverityFatal;
            return (((status & Constants.SeverityFatal) == Constants.SeverityInformational));
        }
        public static bool IsSeverity(jonStatus status, Severity severity)
        {
            return (status.Severity == severity);
        }
        public static bool IsSuccess(jonStatus status)
        {
            return (IsSeverity(status, Severity.Success));
        }
        public static bool IsWarning(jonStatus status)
        {
            return (IsSeverity(status, Severity.Warning));
        }
        public static bool IsError(jonStatus status)
        {
            return (IsSeverity(status, Severity.Error));
        }
        public static bool IsFatal(jonStatus status)
        {
            return (IsSeverity(status, Severity.Fatal));
        }
        public static bool IsWarning(uint status)
        {
            return (((status & Constants.SeverityFatal) == Constants.SeverityWarning));
        }
        public static bool IsSuccessOrWarning(jonStatus status)
        {
            return (IsSuccess(status) || IsWarning(status));
        }
        public static bool IsError(uint status)
        {
            return (((status & Constants.SeverityFatal) == Constants.SeverityError));
        }
        public static bool IsFatal(uint status)
        {
            return (((status & Constants.SeverityFatal) == Constants.SeverityFatal));
        }
        public static Severity StatusSeverity(jonStatus status)
        {
            return (StatusSeverity(status.Status));
        }
        public static bool IsErrorOrFatal(jonStatus status)
        {
            return (IsError(status.Status) || IsFatal(status.Status));
        }
        public static Severity StatusSeverity(uint status)
        {
            if (IsSuccess(status))
            {
                return (Severity.Success);
            }
            if (IsWarning(status))
            {
                return (Severity.Warning);
            }
            if (IsError(status))
            {
                return (Severity.Error);
            }
            if (IsFatal(status))
            {
                return (Severity.Fatal);
            }
            return (Severity.Unknown);
        }
        #endregion
    }
}
