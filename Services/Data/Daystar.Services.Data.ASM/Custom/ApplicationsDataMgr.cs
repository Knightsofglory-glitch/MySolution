using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using Jon.Functional.ASM;
using Jon.Functional.ASM;
using Jon.Services.Dbio.ASM;


namespace Jon.Services.Data.ASM
{
    public class ApplicationsDataMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private string _connectionString = null;
        private Jon.Functional.ASM.UserSession _useSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public ApplicationsDataMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public ApplicationsDataMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
            : base(userSession)
        {
            _connectionString = connectionString;

            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region CRUDL 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUDL
        //----------------------------------------------------------------------------------------------------------------------------------
        public jonStatus Create(Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;



            // Set user access record
            //status = SetCreateUAR(application);
            //if (!jonStatusDef.IsSuccess(status))
            //{
            //    return (status);
            //}
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(Jon.Services.Dbio.ASM.Application application)
        {
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Update(Jon.Functional.ASM.Application application, Jon.Services.Dbio.ASM.Application _application)
        {
            // Initialize
            jonStatus status = null;


            
            // Preserve created UAR
            //status = PreserveCreateUAR(_application, application);
            //if (!jonStatusDef.IsSuccess(status))
            //{
            //    return (status);
            //}

            // Set modified UAR
            //status = SetModifiedUAR(application);
            //if (!jonStatusDef.IsSuccess(status))
            //{
            //    return (status);
            //}
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(Jon.Services.Dbio.ASM.Application application)
        {
            return (new jonStatus(Severity.Success));
        }

        public jonStatus List()
        {
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
            return (new jonStatus(Severity.Success));
        }
        #endregion
    }
}
