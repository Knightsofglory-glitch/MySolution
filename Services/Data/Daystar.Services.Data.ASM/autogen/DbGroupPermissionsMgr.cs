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
    public class DbGroupPermissionsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPermissionsMgr _dbPermissionsMgr = null;
        private DbGroupsMgr _dbGroupsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbGroupPermissionsMgr(Jon.Functional.ASM.UserSession userSession)
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
        public jonStatus Create(Jon.Functional.ASM.GroupPermission groupPermission, out Jon.Functional.ASM.GroupPermissionId groupPermissionId)
        {
            // Initialize
            jonStatus status = null;
            groupPermissionId = null;


            // Data rules.
            groupPermission.Created = DateTime.Now;


            // Create the groupPermission
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, groupPermission, out groupPermissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.GroupPermission groupPermission, out Jon.Functional.ASM.GroupPermissionId groupPermissionId)
        {
            // Initialize
            jonStatus status = null;
            groupPermissionId = null;


            // Data rules.
            groupPermission.Created = DateTime.Now;


            // Create the groupPermission in this transaction.
            status = create((ASMContext)trans.DbContext, groupPermission, out groupPermissionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.GroupPermissionId groupPermissionId, out Jon.Functional.ASM.GroupPermission groupPermission)
        {
            // Initialize
            jonStatus status = null;
            groupPermission = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.GroupPermission _groupPermissions = null;
                status = read(dbContext, groupPermissionId, out _groupPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupPermission = new Jon.Functional.ASM.GroupPermission();
                BufferMgr.TransferBuffer(_groupPermissions, groupPermission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.GroupPermissionId groupPermissionId, out Jon.Functional.ASM.GroupPermission groupPermission)
        {
            // Initialize
            jonStatus status = null;
            groupPermission = null;


            // Perform read
            Jon.Services.Dbio.ASM.GroupPermission _groupPermissions = null;
            status = read((ASMContext)trans.DbContext, groupPermissionId, out _groupPermissions);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupPermission = new Jon.Functional.ASM.GroupPermission();
            BufferMgr.TransferBuffer(_groupPermissions, groupPermission);

            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.GroupId groupId, out Jon.Functional.ASM.GroupPermissionList groupPermissionList)
        {
            // Initialize
            jonStatus status = null;
            groupPermissionList = null;


            // Get group
            Jon.Functional.ASM.Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group permissions
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.GroupPermission> _groupPermissionList = null;
                status = read(dbContext, groupId, out _groupPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupPermissionList = new GroupPermissionList();
                groupPermissionList.Group = group;
                foreach (Jon.Services.Dbio.ASM.GroupPermission _groupPermission in _groupPermissionList)
                {
                    // Get permission
                    PermissionId permissionId = new PermissionId(_groupPermission.PermissionId);
                    Jon.Functional.ASM.Permission permission = null;
                    status = _dbPermissionsMgr.Read(permissionId, out permission);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    groupPermissionList.PermissionList.Add(permission);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, GroupId groupId, out GroupPermissionList groupPermissionList)
        {
            // Initialize
            jonStatus status = null;
            groupPermissionList = null;


            // Get group
            Jon.Functional.ASM.Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group permissions
            List<Jon.Services.Dbio.ASM.GroupPermission> _groupPermissionList = null;
            status = read((ASMContext)trans.DbContext, groupId, out _groupPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupPermissionList = new GroupPermissionList();
            groupPermissionList.Group = group;
            foreach (Jon.Services.Dbio.ASM.GroupPermission _groupPermission in _groupPermissionList)
            {
                // Get permission
                PermissionId permissionId = new PermissionId(_groupPermission.PermissionId);
                Jon.Functional.ASM.Permission permission = null;
                status = _dbPermissionsMgr.Read(permissionId, out permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupPermissionList.PermissionList.Add(permission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(PermissionId permissionId, out PermissionGroupList permissionGroupList)
        {
            // Initialize
            jonStatus status = null;
            permissionGroupList = null;


            // Get permission
            Jon.Functional.ASM.Permission permission = null;
            status = _dbPermissionsMgr.Read(permissionId, out permission);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get permission groups
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.GroupPermission> _groupPermissionsList = null;
                status = read(dbContext, permissionId, out _groupPermissionsList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permissionGroupList = new PermissionGroupList();
                permissionGroupList.Permission = permission;
                foreach (Jon.Services.Dbio.ASM.GroupPermission _groupPermissions in _groupPermissionsList)
                {
                    // Get group
                    GroupId groupId = new GroupId(_groupPermissions.GroupId);
                    Jon.Functional.ASM.Group group = null;
                    status = _dbGroupsMgr.Read(groupId, out group);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    permissionGroupList.GroupList.Add(group);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, PermissionId permissionId, out PermissionGroupList permissionGroupList)
        {
            // Initialize
            jonStatus status = null;
            permissionGroupList = null;


            // Get permission
            Jon.Functional.ASM.Permission permission = null;
            status = _dbPermissionsMgr.Read(permissionId, out permission);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get permission groups
            List<Jon.Services.Dbio.ASM.GroupPermission> _groupPermissionsList = null;
            status = read((ASMContext)trans.DbContext, permissionId, out _groupPermissionsList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            permissionGroupList = new PermissionGroupList();
            permissionGroupList.Permission = permission;
            foreach (Jon.Services.Dbio.ASM.GroupPermission _groupPermissions in _groupPermissionsList)
            {
                // Get group
                GroupId groupId = new GroupId(_groupPermissions.PermissionId);
                Jon.Functional.ASM.Group group = null;
                status = _dbGroupsMgr.Read(groupId, out group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permissionGroupList.GroupList.Add(group);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.GroupPermission groupPermission)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, groupPermission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.GroupPermission groupPermission)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, groupPermission);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(GroupPermissionId groupPermissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, groupPermissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, GroupPermissionId groupPermissionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, groupPermissionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
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
        public jonStatus Delete(GroupId groupId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, groupId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
     
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.GroupPermission> groupPermissionList, out QueryResponse queryResponse)
        {
            // Initialize
            ////jonStatus status = null;
            groupPermissionList = null;
            queryResponse = null;


            using (ASMContext dbContext = new ASMContext())
            {
                using (IDbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Jon.Services.Dbio.ASM.GroupPermission).GetProperties().ToArray();
                        ////List<Jon.Services.Dbio.ASM.GroupPermission> _groupPermissionsList = dbContext.GroupPermissions.Where(BuildWhereClause(queryOptions, dbProperties))
                        ////        .OrderBy(BuildSortString(queryOptions.SortColumns))
                        ////        .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                        ////        .Take(queryOptions.Paging.PageSize).ToList();
                        ////if (_groupPermissionsList == null)
                        ////{
                        ////    return (new jonStatus(Severity.Warning));
                        ////}
                        groupPermissionList = new List<Jon.Functional.ASM.GroupPermission>();
                        ////foreach (Jon.Services.Dbio.ASM.GroupPermission _groupPermissions in _groupPermissionsList)
                        ////{
                        ////    GroupPermission groupPermission = new GroupPermission();
                        ////    BufferMgr.TransferBuffer(_groupPermissions, groupPermission);
                        ////    groupPermissionList.Add(groupPermission);
                        ////}
                        ////status = BuildQueryResponse(_groupPermissionsList.Count, queryOptions, out queryResponse);
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
                _dbGroupsMgr = new DbGroupsMgr(this.UserSession);
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
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.GroupPermission groupPermission, out Jon.Functional.ASM.GroupPermissionId groupPermissionId)
        {
            // Initialize
            groupPermissionId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.GroupPermission _groupPermissions = new Jon.Services.Dbio.ASM.GroupPermission();
                _groupPermissions.PermissionId = groupPermission.Permission.Id;
                _groupPermissions.GroupId = groupPermission.Group.Id;
                _groupPermissions.Created = DateTime.Now;
                dbContext.GroupPermissions.Add(_groupPermissions);
                dbContext.SaveChanges();
                if (_groupPermissions.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "GroupPermission not created"));
                }
                groupPermissionId = new GroupPermissionId(_groupPermissions.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.GroupPermissionId groupPermissionId, out Jon.Services.Dbio.ASM.GroupPermission groupPermission)
        {
            // Initialize
            groupPermission = null;


            try
            {
                groupPermission = dbContext.GroupPermissions.Where(r => r.Id == groupPermissionId.Id).SingleOrDefault();
                if (groupPermission == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupPermissionId.Id))));
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.GroupId groupId, out List<Jon.Services.Dbio.ASM.GroupPermission> groupPermissionList)
        {
            // Initialize
            groupPermissionList = null;


            try
            {
                groupPermissionList = dbContext.GroupPermissions.Where(r => r.GroupId == groupId.Id).ToList();
                if (groupPermissionList == null)
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.PermissionId permissionId, out List<Jon.Services.Dbio.ASM.GroupPermission> groupPermissionList)
        {
            // Initialize
            groupPermissionList = null;


            try
            {
                groupPermissionList = dbContext.GroupPermissions.Where(r => r.PermissionId == permissionId.Id).ToList();
                if (groupPermissionList == null)
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.GroupPermission groupPermission)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                GroupPermissionId groupPermissionId = new GroupPermissionId(groupPermission.Id);
                Jon.Services.Dbio.ASM.GroupPermission _groupPermissions = null;
                status = read(dbContext, groupPermissionId, out _groupPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(groupPermission, _groupPermissions);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.GroupPermissionId groupPermissionId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.GroupPermission _groupPermissions = null;
                status = read(dbContext, groupPermissionId, out _groupPermissions);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.GroupPermissions.Remove(_groupPermissions);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.GroupId groupId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                List<Jon.Services.Dbio.ASM.GroupPermission> groupPermissionList = null;
                status = read(dbContext, groupId, out groupPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Jon.Services.Dbio.ASM.GroupPermission groupPermission in groupPermissionList)
                {
                    dbContext.GroupPermissions.Remove(groupPermission);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.PermissionId permissionId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                List<Jon.Services.Dbio.ASM.GroupPermission> groupPermissionList = null;
                status = read(dbContext, permissionId, out groupPermissionList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Jon.Services.Dbio.ASM.GroupPermission groupPermission in groupPermissionList)
                {
                    dbContext.GroupPermissions.Remove(groupPermission);
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
