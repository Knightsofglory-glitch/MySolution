using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using Jon.Functional.ASM;
using Jon.Functional.Util;
using Jon.Services.Dbio.ASM;


namespace Jon.Services.Data
{
    public class DbMgrSessionBased : DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private Jon.Functional.ASM.UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMgrSessionBased(Jon.Functional.ASM.UserSession userSession)
            : base()
        {
            this._userSession = userSession;
            initialize();
        }
        public DbMgrSessionBased(Jon.Functional.ASM.UserSession userSession, string connectionString)
            : base(connectionString)
        {
            this._userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public Jon.Functional.ASM.UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region User Access Record
            //
            // User Access Record
            //
        public jonStatus SetCreateUAR(object record)
        {
            // Initialize
            Type type = record.GetType();
            PropertyInfo piCreatedDate = null;
            PropertyInfo piCreatedBy = null;

            // Set create date/time
            try
            {
                piCreatedDate = type.GetProperty("CreatedDate");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a CreatedDate property"));
            }         
            try
            {
                piCreatedDate.SetValue(record, DateTime.Now, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting CreatedDate property to DateTime.Now"));
            }

            // Set user Id
            try
            {
                piCreatedBy = type.GetProperty("CreatedBy");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a CreatedBy property"));
            }
            try
            {
                piCreatedBy.SetValue(record, this.UserSession.User.Id, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting CreatedBy property to user Id"));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus SetModifiedUAR(object record)
        {
            // Initialize
            Type type = record.GetType();
            PropertyInfo piLastModifiedDate = null;
            PropertyInfo piModifiedBy = null;

            // Set create date/time
            try
            {
                piLastModifiedDate = type.GetProperty("ModifiedDate");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a LastModifiedDate property"));
            }
            try
            {
                piLastModifiedDate.SetValue(record, DateTime.Now, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting LastModifiedDate property to DateTime.Now"));
            }

            // Set user Id
            try
            {
                piModifiedBy = type.GetProperty("ModifiedBy");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a ModifiedBy property"));
            }
            try
            {
                piModifiedBy.SetValue(record, this.UserSession.User.Id, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting ModifiedBy property to user Id"));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus SetDeletedUAR(object record)
        {
            // Initialize
            Type type = record.GetType();
            PropertyInfo piLastDeletedDate = null;
            PropertyInfo piDeletedBy = null;

            // Set create date/time
            try
            {
                piLastDeletedDate = type.GetProperty("DeletedDate");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a LastDeletedDate property"));
            }
            try
            {
                piLastDeletedDate.SetValue(record, DateTime.Now, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting LastDeletedDate property to DateTime.Now"));
            }

            // Set user Id
            try
            {
                piDeletedBy = type.GetProperty("DeletedBy");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a DeletedBy property"));
            }
            try
            {
                piDeletedBy.SetValue(record, this.UserSession.User.Id, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error setting DeletedBy property to user Id"));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus PreserveCreateUAR(object sourceRecord, object targetRecord)
        {
            // Initialize
            Type sourceType = sourceRecord.GetType();
            Type targetType = targetRecord.GetType();
            PropertyInfo piSourceCreatedDate = null;
            PropertyInfo piTargetCreatedDate = null;
            PropertyInfo piSourceCreatedBy = null;
            PropertyInfo piTargetCreatedBy = null;

            // Set create date/time
            try
            {
                piSourceCreatedDate = sourceType.GetProperty("CreatedDate");
                piTargetCreatedDate = targetType.GetProperty("CreatedDate");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a CreatedDate property"));
            }
            try
            {
                var objSourceCreatedDate = piSourceCreatedDate.GetValue(sourceRecord);
                piTargetCreatedDate.SetValue(targetRecord, objSourceCreatedDate, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error preserving CreatedDate property"));
            }

            // Set user Id
            try
            {
                piSourceCreatedBy = sourceType.GetProperty("CreatedBy");
                piTargetCreatedBy = targetType.GetProperty("CreatedBy");
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Record does not have a CreatedBy property"));
            }
            try
            {
                var objSourceCreatedBy = piSourceCreatedBy.GetValue(sourceRecord);
                piTargetCreatedBy.SetValue(targetRecord, objSourceCreatedBy, null);
            }
            catch (System.Exception)
            {
                return (new jonStatus(Severity.Fatal, "Error preserving CreatedBy property"));
            }
            return (new jonStatus(Severity.Success));
        }
        #endregion

        #endregion



        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        private jonStatus initialize()
        {
            // Initialize
            jonStatus status = null;
            try
            {
                status = initUserSession();
                if (! jonStatusDef.IsSuccess(status))
                {
                    return(new jonStatus(Severity.Error, "ERROR: initialize user session: " + status.Message));
                }
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus initUserSession()
        {
            // Initialize
            jonStatus status = null;


            // TEMPORARY: DO NOT VALIDATE USER SESSION IF NOT SPECIFIED
            if (this._userSession == null)
            {
                return (new jonStatus(Severity.Success));
            }

            // Load ASM application into user session
            status = loadUserApplication();
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus loadUserApplication()
        {
            // Validate user application is set
            if (this._userSession.User.ApplicationId == null || this._userSession.User.ApplicationId < BaseId.VALID_ID)
            {
                return (new jonStatus(Severity.Error, "User is not assigned to an application"));
            }

            // Get user application
            Jon.Services.Dbio.ASM.Application application = null;
            Jon.Functional.ASM.ApplicationId applicationId = new Functional.ASM.ApplicationId(this._userSession.User.ApplicationId.Value);
            using (ASMContext asmContext = new ASMContext())
            {
                if (asmContext.Applications.Where(r => r.Id == applicationId.Id).Any())
                {
                    application = asmContext.Applications.Where(r => r.Id == applicationId.Id).SingleOrDefault();
                }
                else
                {
                    return (new jonStatus(Severity.Warning, String.Format("Application id {0} not found in database", applicationId.Id.ToString())));
                }
            }


            // Load application to user session
            this._userSession.Application = new Jon.Functional.ASM.Application();
            BufferMgr.TransferBuffer(application, this._userSession.Application);


            return (new jonStatus(Severity.Success));
        }
        #endregion
    }
}
