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
using Jon.Util.Encryption;
using Jon.Functional.ASM;
using Jon.Services.Dbio.ASM;


namespace Jon.Services.Data.ASM
{
    public class UserPermissionsDataMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPermissionsMgr _dbPermissionsMgr = null;
        private DbUsersMgr _dbUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public UserPermissionsDataMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
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
                _dbPermissionsMgr = new DbPermissionsMgr(this.UserSession);
                _dbUsersMgr = new DbUsersMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }


        #region UserPermissions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserPermissions
         *---------------------------------------------------------------------------------------------------------------------------------*/
   
        
        private jonStatus read(ASMContext dbContext, UserId userId, out List<Jon.Services.Dbio.ASM.UserPermission> userPermissionList)
        {
            // Initialize
            userPermissionList = null;


            try
            {
                userPermissionList = dbContext.UserPermissions.Where(r => r.UserId == userId.Id).ToList();
                if (userPermissionList == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("UserId {0} not found", userId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, PermissionId permissionId, out List<Jon.Services.Dbio.ASM.UserPermission> userPermissionList)
        {
            // Initialize
            userPermissionList = null;


            try
            {
                userPermissionList = dbContext.UserPermissions.Where(r => r.PermissionId == permissionId.Id).ToList();
                if (userPermissionList == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("PermissionId {0} not found", permissionId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
      
      
        private jonStatus delete(ASMContext dbContext, UserId userId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                List<Jon.Services.Dbio.ASM.UserPermission> userPermissionList = null;
                status = read(dbContext, userId, out userPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Jon.Services.Dbio.ASM.UserPermission userPermission in userPermissionList)
                {
                    dbContext.UserPermissions.Remove(userPermission);
                }
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(UserId userId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, userId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, UserId userId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, userId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus delete(ASMContext dbContext, PermissionId permissionId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                List<Jon.Services.Dbio.ASM.UserPermission> userPermissionList = null;
                status = read(dbContext, permissionId, out userPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Jon.Services.Dbio.ASM.UserPermission userPermission in userPermissionList)
                {
                    dbContext.UserPermissions.Remove(userPermission);
                }
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(PermissionId permissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, permissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, PermissionId permissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, permissionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
