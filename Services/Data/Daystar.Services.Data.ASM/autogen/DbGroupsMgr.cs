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
    public class DbGroupsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbGroupsMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbGroupsMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
            : base(userSession, connectionString)
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
        public jonStatus Create(Jon.Functional.ASM.Group group, out Jon.Functional.ASM.GroupId groupId)
        {
            // Initialize
            jonStatus status = null;
            groupId = null;


            // Data rules.
            group.Created = DateTime.Now;


            // Create the group
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, group, out groupId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.Group group, out Jon.Functional.ASM.GroupId groupId)
        {
            // Initialize
            jonStatus status = null;
            groupId = null;


            // Data rules.
            group.Created = DateTime.Now;


            // Create the group in this transaction.
            status = create((ASMContext)trans.DbContext, group, out groupId);
            if (! jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.GroupId groupId, out Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;
            group = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Group _group = null;
                status = read(dbContext, groupId, out _group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Jon.Functional.ASM.Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(string groupname, out Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;
            group = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Group _group = null;
                status = read(dbContext, groupname, out _group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Jon.Functional.ASM.Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.GroupId groupId, out Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;
            group = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Group _group = null;
                status = read((ASMContext)trans.DbContext, groupId, out _group);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Jon.Functional.ASM.Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, string groupname, out Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;
            group = null;


            // Perform transaction read.
            Jon.Services.Dbio.ASM.Group _group = null;
            status = read((ASMContext)trans.DbContext, groupname, out _group);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            group = new Jon.Functional.ASM.Group();
            BufferMgr.TransferBuffer(_group, group);
            
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.Group group)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, group);
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
            if (! jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.vwGroupsList> groupList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            groupList = null;
            queryResponse = null;

            try
            {
                // Get entities
                groupList = new List<Jon.Functional.ASM.vwGroupsList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwGroupsList", typeof(Jon.Functional.ASM.Group), out sSelectQuery);
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
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(Jon.Functional.ASM.Group));
                        command.CommandText = "SELECT COUNT(*) FROM vwGroupsList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.ASM.vwGroupsList vwGroups = new Jon.Functional.ASM.vwGroupsList();
                                status = TransferSQLReader(reader, vwGroups, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                groupList.Add(vwGroups);
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
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }


        #region Groups
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Groups
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.Group group, out Jon.Functional.ASM.GroupId groupId)
        {
            // Initialize
            groupId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.Group _group = new Jon.Services.Dbio.ASM.Group();
                BufferMgr.TransferBuffer(group, _group);
                dbContext.Groups.Add(_group);
                dbContext.SaveChanges();
                if (_group.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "Group not created"));
                }
                groupId = new GroupId(_group.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.GroupId groupId, out Jon.Services.Dbio.ASM.Group group)
        {
            // Initialize
            group = null;


            try
            {
                group = dbContext.Groups.Where(r => r.Id == groupId.Id).SingleOrDefault();
                if (group == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupId.Id))));
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
        private jonStatus read(ASMContext dbContext, string groupname, out Jon.Services.Dbio.ASM.Group group)
        {
            // Initialize
            group = null;


            try
            {
                group = dbContext.Groups.Where(r => r.Name == groupname).SingleOrDefault();
                if (group == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupname))));
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.Group group)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                GroupId groupId = new GroupId(group.Id);
                Jon.Services.Dbio.ASM.Group _group = null;
                status = read(dbContext, groupId, out _group);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(group, _group);
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
                Jon.Services.Dbio.ASM.Group _group = null;
                status = read(dbContext, groupId, out _group);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Groups.Remove(_group);
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
