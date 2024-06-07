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
    public class DbUserGroupsMgr : DbMgrSessionBased
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
        public DbUserGroupsMgr(Jon.Functional.ASM.UserSession userSession)
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
        public jonStatus Create(Jon.Functional.ASM.UserGroup userGroup, out Jon.Functional.ASM.UserGroupId userGroupId)
        {
            // Initialize
            jonStatus status = null;
            userGroupId = null;


            // Data rules.


            // Create the userGroup
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, userGroup, out userGroupId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.UserGroup userGroup, out Jon.Functional.ASM.UserGroupId userGroupId)
        {
            // Initialize
            jonStatus status = null;
            userGroupId = null;


            // Data rules.


            // Create the userGroup in this transaction.
            status = create((ASMContext)trans.DbContext, userGroup, out userGroupId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(Jon.Functional.ASM.UserGroupId userGroupId, out Jon.Functional.ASM.UserGroup userGroup)
        {
            // Initialize
            jonStatus status = null;
            userGroup = null;

           
            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.GroupUser _groupUsers = null;
                status = read(dbContext, userGroupId, out _groupUsers);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroup = new UserGroup();
                BufferMgr.TransferBuffer(_groupUsers, userGroup);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserGroupId userGroupId, out Jon.Functional.ASM.UserGroup userGroup)
        {
            // Initialize
            jonStatus status = null;
            userGroup = null;


            // Perform read
            Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
            status = read((ASMContext)trans.DbContext, userGroupId, out _groupUser);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userGroup = new Jon.Functional.ASM.UserGroup();
            BufferMgr.TransferBuffer(_groupUser, userGroup);

            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.UserGroupList userGroupList)
        {
            // Initialize
            jonStatus status = null;
            userGroupList = null;


            // Get the user
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
                    GroupId groupId = new GroupId(_groupUser.GroupId);
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


            // Get the user
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
                    GroupId groupId = new GroupId(_groupUser.GroupId);
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

        public jonStatus Update(Jon.Functional.ASM.UserGroup userGroup)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, userGroup);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.UserGroup userGroup)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, userGroup);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(Jon.Functional.ASM.UserGroupId userGroupId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, userGroupId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.UserGroupId userGroupId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, userGroupId);
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

        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.vwUserGroupsList> userGroupList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            userGroupList = null;
            queryResponse = null;

            try
            {
                // Get entities
                userGroupList = new List<Jon.Functional.ASM.vwUserGroupsList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwUserGroupsList", typeof(UserGroup), out sSelectQuery);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }


                int totalRecords = 0;
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        // Get total records
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(UserGroup));
                        command.CommandText = "SELECT COUNT(*) FROM vwUserGroupsList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.ASM.vwUserGroupsList vwUserGroups = new Jon.Functional.ASM.vwUserGroupsList();
                                status = TransferSQLReader(reader, vwUserGroups, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                userGroupList.Add(vwUserGroups);
                            }
                        }
                    }
                }

                // Build query response
                status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
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


        #region UserGroups
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserGroups
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, UserGroup userGroup, out UserGroupId userGroupId)
        {
            // Initialize
            userGroupId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.GroupUser _groupUser = new Jon.Services.Dbio.ASM.GroupUser();
                _groupUser.GroupId = userGroup.Group.Id;
                _groupUser.UserId = userGroup.User.Id;
                _groupUser.Created = DateTime.Now;
                dbContext.GroupUsers.Add(_groupUser);
                dbContext.SaveChanges();
                if (_groupUser.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "User Group not created"));
                }
                userGroupId = new UserGroupId(_groupUser.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
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
        private jonStatus update(ASMContext dbContext, UserGroup userGroup)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                UserGroupId userGroupId = new UserGroupId(userGroup.Id);
                Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
                status = read(dbContext, userGroupId, out _groupUser);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(userGroup, _groupUser);
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
        private jonStatus delete(ASMContext dbContext, UserGroupId userGroupId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.GroupUser _groupUser = null;
                status = read(dbContext, userGroupId, out _groupUser);
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
        private jonStatus delete(ASMContext dbContext, GroupId groupId)
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
        #endregion

        #endregion
    }
}
