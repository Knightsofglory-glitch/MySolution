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
    public class DbGroupUsersMgr : DbMgrSessionBased
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
        public DbGroupUsersMgr(Jon.Functional.ASM.UserSession userSession)
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
        public jonStatus Create(Jon.Functional.ASM.GroupUser groupUser, out Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize
            jonStatus status = null;
            groupUserId = null;


            // Data rules.
            groupUser.Created = DateTime.Now;


            // Create the groupUser
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, groupUser, out groupUserId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.GroupUser groupUser, out Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize
            jonStatus status = null;
            groupUserId = null;


            // Data rules.
            groupUser.Created = DateTime.Now;


            // Create the groupUser in this transaction.
            status = create((ASMContext)trans.DbContext, groupUser, out groupUserId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.GroupUserId groupUserId, out Jon.Functional.ASM.GroupUser groupUser)
        {
            // Initialize
            jonStatus status = null;
            groupUser = null;

           
            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUser = new Jon.Functional.ASM.GroupUser();
                BufferMgr.TransferBuffer(_groupUser, groupUser);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.GroupUserId groupUserId, out Jon.Functional.ASM.GroupUser groupUser)
        {
            // Initialize
            jonStatus status = null;
            groupUser = null;


            // Perform read
            Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
            status = read((ASMContext)trans.DbContext, groupUserId, out _groupUser);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupUser = new Jon.Functional.ASM.GroupUser();
            BufferMgr.TransferBuffer(_groupUser, groupUser);

            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.GroupId groupId, out Jon.Functional.ASM.GroupUserList groupUserList)
        {
            // Initialize
            jonStatus status = null;
            groupUserList = null;


            // Get group
            Jon.Functional.ASM.Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group users
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.GroupUser> _groupUserList = null;
                status = read(dbContext, groupId, out _groupUserList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUserList = new GroupUserList();
                groupUserList.Group = group;
                foreach (Jon.Services.Dbio.ASM.GroupUser _groupUser in _groupUserList)
                {
                    // Get user
                    Jon.Functional.ASM.UserId userId = new Jon.Functional.ASM.UserId(_groupUser.UserId);
                    Jon.Functional.ASM.User user = null;
                    status = _dbUsersMgr.Read(userId, out user);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    groupUserList.UserList.Add(user);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.GroupId groupId, out Jon.Functional.ASM.GroupUserList groupUserList)
        {
            // Initialize
            jonStatus status = null;
            groupUserList = null;


            // Get group
            Jon.Functional.ASM.Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group users
            List<Jon.Services.Dbio.ASM.GroupUser> _groupUserList = null;
            status = read((ASMContext)trans.DbContext, groupId, out _groupUserList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupUserList = new GroupUserList();
            groupUserList.Group = group;
            foreach (Jon.Services.Dbio.ASM.GroupUser _groupUser in _groupUserList)
            {
                // Get user
                Jon.Functional.ASM.UserId userId = new Jon.Functional.ASM.UserId(_groupUser.UserId);
                Jon.Functional.ASM.User user = null;
                status = _dbUsersMgr.Read(userId, out user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUserList.UserList.Add(user);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.UserGroupList userGroupList)
        {
            // Initialize
            jonStatus status = null;
            userGroupList = null;


            // Get user
            Jon.Functional.ASM.User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user groups
            using (ASMContext dbContext = new ASMContext())
            {
                List<Jon.Services.Dbio.ASM.GroupUser> _groupUserList = null;
                status = read(dbContext, userId, out _groupUserList);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupList = new UserGroupList();
                userGroupList.User = user;
                foreach (Jon.Services.Dbio.ASM.GroupUser _groupUser in _groupUserList)
                {
                    // Get group
                    Jon.Functional.ASM.GroupId groupId = new Jon.Functional.ASM.GroupId(_groupUser.GroupId);
                    Jon.Functional.ASM.Group group = null;
                    status = _dbGroupsMgr.Read(groupId, out group);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    userGroupList.GroupList.Add(group);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.UserGroupList userGroupList)
        {
            // Initialize
            jonStatus status = null;
            userGroupList = null;


            // Get user
            Jon.Functional.ASM.User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user groups
            List<Jon.Services.Dbio.ASM.GroupUser> _groupUserList = null;
            status = read((ASMContext)trans.DbContext, userId, out _groupUserList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userGroupList = new UserGroupList();
            userGroupList.User = user;
            foreach (Jon.Services.Dbio.ASM.GroupUser _groupUser in _groupUserList)
            {
                // Get group
                Jon.Functional.ASM.GroupId groupId = new Jon.Functional.ASM.GroupId(_groupUser.UserId);
                Jon.Functional.ASM.Group group = null;
                status = _dbGroupsMgr.Read(groupId, out group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupList.GroupList.Add(group);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.GroupUser groupUser)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, groupUser);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.GroupUser groupUser)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, groupUser);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, groupUserId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, groupUserId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(Jon.Functional.ASM.GroupId groupId)
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
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.GroupId groupId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, groupId);
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
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.GroupUser> groupUserList, out QueryResponse queryResponse)
        {
            // Initialize
            ////jonStatus status = null;
            groupUserList = null;
            queryResponse = null;


            using (ASMContext dbContext = new ASMContext())
            {
                using (IDbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Jon.Services.Dbio.ASM.GroupUser).GetProperties().ToArray();
                        ////List<Jon.Services.Dbio.ASM.GroupUser> _groupUsersList = dbContext.GroupUsers.Where(BuildWhereClause(queryOptions, dbProperties))
                        ////        .OrderBy(BuildSortString(queryOptions.SortColumns))
                        ////        .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                        ////        .Take(queryOptions.Paging.PageSize).ToList();
                        ////if (_groupUsersList == null)
                        ////{
                        ////    return (new jonStatus(Severity.Warning));
                        ////}
                        ////groupUserList = new List<GroupUser>();
                        ////foreach (Jon.Services.Dbio.ASM.GroupUser _groupUser in _groupUsersList)
                        ////{
                        ////    GroupUser groupUser = new GroupUser();
                        ////    BufferMgr.TransferBuffer(_groupUser, groupUser);
                        ////    groupUserList.Add(groupUser);
                        ////}
                        ////status = BuildQueryResponse(_groupUsersList.Count, queryOptions, out queryResponse);
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
                _dbGroupsMgr = new DbGroupsMgr(this.UserSession);
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


        #region GroupUsers
        /*----------------------------------------------------------------------------------------------------------------------------------
         * GroupUsers
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.GroupUser groupUser, out Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize
            groupUserId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.GroupUser _groupUser = new Jon.Services.Dbio.ASM.GroupUser();
                _groupUser.GroupId = groupUser.Group.Id;
                _groupUser.UserId = groupUser.User.Id;
                _groupUser.Created = DateTime.Now;
                dbContext.GroupUsers.Add(_groupUser);
                dbContext.SaveChanges();
                if (_groupUser.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "GroupUser not created"));
                }
                groupUserId = new Jon.Functional.ASM.GroupUserId(_groupUser.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.GroupUserId groupUserId, out Jon.Services.Dbio.ASM.GroupUser groupUser)
        {
            // Initialize
            groupUser = null;


            try
            {
                groupUser = dbContext.GroupUsers.Where(r => r.Id == groupUserId.Id).SingleOrDefault();
                if (groupUser == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupUserId.Id))));
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.GroupId groupId, out List<Jon.Services.Dbio.ASM.GroupUser> groupUserList)
        {
            // Initialize
            groupUserList = null;


            try
            {
                groupUserList = dbContext.GroupUsers.Where(r => r.GroupId == groupId.Id).ToList();
                if (groupUserList == null)
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.UserId userId, out List<Jon.Services.Dbio.ASM.GroupUser> groupUserList)
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.GroupUser groupUser)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Functional.ASM.GroupUserId groupUserId = new Jon.Functional.ASM.GroupUserId(groupUser.Id);
                Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(groupUser, _groupUser);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.GroupUserId groupUserId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.GroupUsers.Remove(_groupUser);
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
                List<Jon.Services.Dbio.ASM.GroupUser> groupUserList = null;
                status = read(dbContext, groupId, out groupUserList);
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
        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.UserId userId)
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
        #endregion

        #endregion
    }
}
