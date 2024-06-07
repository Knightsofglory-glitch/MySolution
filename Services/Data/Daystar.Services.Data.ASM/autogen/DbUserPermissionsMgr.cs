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
    public class DbUserPermissionsMgr : DbMgrSessionBased
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
        public DbUserPermissionsMgr(Jon.Functional.ASM.UserSession userSession)
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
        public jonStatus Create(Jon.Functional.ASM.UserPermission userPermission, out Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize
            jonStatus status = null;
            userPermissionId = null;


            // Data rules.
            userPermission.Created = DateTime.Now;


            // Create the userPermission
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, userPermission, out userPermissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.UserPermission userPermission, out Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize
            jonStatus status = null;
            userPermissionId = null;


            // Data rules.
            userPermission.Created = DateTime.Now;


            // Create the userPermission in this transaction.
            status = create((ASMContext)trans.DbContext, userPermission, out userPermissionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.UserPermissionId userPermissionId, out Jon.Functional.ASM.UserPermission userPermission)
        {
            // Initialize
            jonStatus status = null;
            userPermission = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.UserPermission _userPermissions = null;
                status = read(dbContext, userPermissionId, out _userPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPermission = new Jon.Functional.ASM.UserPermission();
                BufferMgr.TransferBuffer(_userPermissions, userPermission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserPermissionId userPermissionId, out Jon.Functional.ASM.UserPermission userPermission)
        {
            // Initialize
            jonStatus status = null;
            userPermission = null;


            // Perform read
            Jon.Services.Dbio.ASM.UserPermission _userPermissions = null;
            status = read((ASMContext)trans.DbContext, userPermissionId, out _userPermissions);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userPermission = new Jon.Functional.ASM.UserPermission();
            BufferMgr.TransferBuffer(_userPermissions, userPermission);

            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.UserPermissionList userPermissionList)
        {
            // Initialize
            jonStatus status = null;
            userPermissionList = null;


            // Get user
            Jon.Functional.ASM.User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user permissions
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.UserPermission> _userPermissionList = null;
                status = read(dbContext, userId, out _userPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPermissionList = new UserPermissionList();
                userPermissionList.User = user;
                foreach (Jon.Services.Dbio.ASM.UserPermission _userPermission in _userPermissionList)
                {
                    // Get permission
                    PermissionId permissionId = new PermissionId(_userPermission.PermissionId);
                    Jon.Functional.ASM.Permission permission = null;
                    status = _dbPermissionsMgr.Read(permissionId, out permission);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    userPermissionList.PermissionList.Add(permission);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.UserPermissionList userPermissionList)
        {
            // Initialize
            jonStatus status = null;
            userPermissionList = null;


            // Get user
            Jon.Functional.ASM.User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user permissions
            List<Jon.Services.Dbio.ASM.UserPermission> _userPermissionList = null;
            status = read((ASMContext)trans.DbContext, userId, out _userPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userPermissionList = new UserPermissionList();
            userPermissionList.User = user;
            foreach (Jon.Services.Dbio.ASM.UserPermission _userPermission in _userPermissionList)
            {
                // Get permission
                PermissionId permissionId = new PermissionId(_userPermission.PermissionId);
                Jon.Functional.ASM.Permission permission = null;
                status = _dbPermissionsMgr.Read(permissionId, out permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPermissionList.PermissionList.Add(permission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.PermissionId permissionId, out Jon.Functional.ASM.PermissionUserList permissionUserList)
        {
            // Initialize
            jonStatus status = null;
            permissionUserList = null;


            // Get permission
            Jon.Functional.ASM.Permission permission = null;
            status = _dbPermissionsMgr.Read(permissionId, out permission);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get permission users
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.UserPermission> _userPermissionsList = null;
                status = read(dbContext, permissionId, out _userPermissionsList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permissionUserList = new PermissionUserList();
                permissionUserList.Permission = permission;
                foreach (Jon.Services.Dbio.ASM.UserPermission _userPermissions in _userPermissionsList)
                {
                    // Get user
                    Jon.Functional.ASM.UserId userId = new Jon.Functional.ASM.UserId(_userPermissions.UserId);
                    Jon.Functional.ASM.User user = null;
                    status = _dbUsersMgr.Read(userId, out user);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    permissionUserList.UserList.Add(user);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.PermissionId permissionId, out Jon.Functional.ASM.PermissionUserList permissionUserList)
        {
            // Initialize
            jonStatus status = null;
            permissionUserList = null;


            // Get permission
            Jon.Functional.ASM.Permission permission = null;
            status = _dbPermissionsMgr.Read(permissionId, out permission);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get permission users
            List<Jon.Services.Dbio.ASM.UserPermission> _userPermissionsList = null;
            status = read((ASMContext)trans.DbContext, permissionId, out _userPermissionsList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            permissionUserList = new PermissionUserList();
            permissionUserList.Permission = permission;
            foreach (Jon.Services.Dbio.ASM.UserPermission _userPermissions in _userPermissionsList)
            {
                // Get user
                Jon.Functional.ASM.UserId userId = new Jon.Functional.ASM.UserId(_userPermissions.PermissionId);
                Jon.Functional.ASM.User user = null;
                status = _dbUsersMgr.Read(userId, out user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permissionUserList.UserList.Add(user);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.UserPermission userPermission)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, userPermission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.UserPermission userPermission)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, userPermission);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, userPermissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, userPermissionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(Jon.Functional.ASM.PermissionId permissionId)
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
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.PermissionId permissionId)
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
        public jonStatus Delete(Jon.Functional.ASM.UserId userId)
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
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.UserId userId)
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
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.UserPermission> userPermissionList, out QueryResponse queryResponse)
        {
            // Initialize
            ////jonStatus status = null;
            userPermissionList = null;
            queryResponse = null;


            using (ASMContext dbContext = new ASMContext())
            {
                using (IDbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Jon.Services.Dbio.ASM.UserPermission).GetProperties().ToArray();
                        ////List<Jon.Services.Dbio.ASM.UserPermission> _userPermissionsList = dbContext.UserPermissions.Where(BuildWhereClause(queryOptions, dbProperties))
                        ////        .OrderBy(BuildSortString(queryOptions.SortColumns))
                        ////        .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                        ////        .Take(queryOptions.Paging.PageSize).ToList();
                        ////if (_userPermissionsList == null)
                        ////{
                        ////    return (new jonStatus(Severity.Warning));
                        ////}
                        ////userPermissionList = new List<UserPermission>();
                        ////foreach (Jon.Services.Dbio.ASM.UserPermission _userPermissions in _userPermissionsList)
                        ////{
                        ////    UserPermission userPermission = new UserPermission();
                        ////    BufferMgr.TransferBuffer(_userPermissions, userPermission);
                        ////    userPermissionList.Add(userPermission);
                        ////}
                        ////status = BuildQueryResponse(_userPermissionsList.Count, queryOptions, out queryResponse);
                        ////if (!jonStatusDef.IsSuccess(status))
                        ////{
                        ////    return (status);
                        ////}
                    }
                    catch (System.Exception ex)
                    {
                        return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
                }
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
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.UserPermission userPermission, out Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize
            userPermissionId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.UserPermission _userPermissions = new Jon.Services.Dbio.ASM.UserPermission();
                _userPermissions.PermissionId = userPermission.Permission.Id;
                _userPermissions.UserId = userPermission.User.Id;
                _userPermissions.Created = DateTime.Now;
                dbContext.UserPermissions.Add(_userPermissions);
                dbContext.SaveChanges();
                if (_userPermissions.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "UserPermission not created"));
                }
                userPermissionId = new UserPermissionId(_userPermissions.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, UserPermissionId userPermissionId, out Jon.Services.Dbio.ASM.UserPermission userPermission)
        {
            // Initialize
            userPermission = null;


            try
            {
                userPermission = dbContext.UserPermissions.Where(r => r.Id == userPermissionId.Id).SingleOrDefault();
                if (userPermission == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userPermissionId.Id))));
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.UserId userId, out List<Jon.Services.Dbio.ASM.UserPermission> userPermissionList)
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.UserPermission userPermission)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                UserPermissionId userPermissionId = new UserPermissionId(userPermission.Id);
                Jon.Services.Dbio.ASM.UserPermission _userPermissions = null;
                status = read(dbContext, userPermissionId, out _userPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(userPermission, _userPermissions);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.UserPermissionId userPermissionId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.UserPermission _userPermissions = null;
                status = read(dbContext, userPermissionId, out _userPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.UserPermissions.Remove(_userPermissions);
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
        #endregion

        #endregion
    }
}
