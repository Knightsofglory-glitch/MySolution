using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class UserGroupsDataMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbGroupsMgr _dbGroupsMgr = null;
        private DbUsersMgr _dbUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public UserGroupsDataMgr(Jon.Functional.ASM.UserSession userSession)
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
              
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }


        #region UserGroups
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserGroups
         *---------------------------------------------------------------------------------------------------------------------------------*/
      private jonStatus read(ASMContext dbContext, UserGroupId userGroupId, out Jon.Services.Dbio.ASM.GroupUser groupUser)
        {
            // Initialize
            groupUser = null;


            try
            {
                groupUser = dbContext.GroupUsers.Where(r => r.Id == userGroupId.Id).SingleOrDefault();
                if (groupUser == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userGroupId.Id))));
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
        private jonStatus read(ASMContext dbContext, GroupId groupId, out List<Jon.Services.Dbio.ASM.GroupUser> groupUsersList)
        {
            // Initialize
            groupUsersList = null;


            try
            {
                groupUsersList = dbContext.GroupUsers.Where(r => r.GroupId == groupId.Id).ToList();
                if (groupUsersList == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("GroupId {0} not found", groupId.Id))));
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
        private jonStatus read(ASMContext dbContext, UserId userId, out List<Jon.Services.Dbio.ASM.GroupUser> groupUserList)
        {
            // Initialize
            groupUserList = null;


            try
            {
                groupUserList = dbContext.GroupUsers.Where(r => r.UserId == userId.Id).ToList();
                if (groupUserList == null)
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

   
        private jonStatus delete(ASMContext dbContext, UserId userId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                List<Jon.Services.Dbio.ASM.GroupUser> groupUserList = null;
                status = read(dbContext, userId, out groupUserList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Jon.Services.Dbio.ASM.GroupUser groupUser in groupUserList)
                {
                    dbContext.GroupUsers.Remove(groupUser);
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





        #endregion

        #endregion
    }
}
