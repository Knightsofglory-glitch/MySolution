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
    public class DbPermissionsMgr : DbMgrSessionBased
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
        public DbPermissionsMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbPermissionsMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
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
        public jonStatus Create(Jon.Functional.ASM.Permission permission, out Jon.Functional.ASM.PermissionId permissionId)
        {
            // Initialize
            jonStatus status = null;
            permissionId = null;


            // Data rules.
            permission.Created = DateTime.Now;


            // Create the permission
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, permission, out permissionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.Permission permission, out Jon.Functional.ASM.PermissionId permissionId)
        {
            // Initialize
            jonStatus status = null;
            permissionId = null;


            // Data rules.
            permission.Created = DateTime.Now;


            // Create the permission in this transaction.
            status = create((ASMContext)trans.DbContext, permission, out permissionId);
            if (! jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(PermissionId permissionId, out Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;
            permission = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Permission _permission = null;
                status = read(dbContext, permissionId, out _permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permission = new Jon.Functional.ASM.Permission();
                BufferMgr.TransferBuffer(_permission, permission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(string permissionname, out Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;
            permission = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Permission _permission = null;
                status = read(dbContext, permissionname, out _permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permission = new Jon.Functional.ASM.Permission();
                BufferMgr.TransferBuffer(_permission, permission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.PermissionId permissionId, out Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;
            permission = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Permission _permission = null;
                status = read((ASMContext)trans.DbContext, permissionId, out _permission);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                permission = new Jon.Functional.ASM.Permission();
                BufferMgr.TransferBuffer(_permission, permission);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, string permissionname, out Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;
            permission = null;


            // Perform transaction read.
            Jon.Services.Dbio.ASM.Permission _permission = null;
            status = read((ASMContext)trans.DbContext, permissionname, out _permission);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            permission = new Jon.Functional.ASM.Permission();
            BufferMgr.TransferBuffer(_permission, permission);
            
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.Permission permission)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, permission);
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
            if (! jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.vwPermissionsList> permissionList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            permissionList = null;
            queryResponse = null;

            try
            {
                // Get entities
                permissionList = new List<Jon.Functional.ASM.vwPermissionsList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwPermissionsList", typeof(Jon.Functional.ASM.Permission), out sSelectQuery);
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
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(Jon.Functional.ASM.Permission));
                        command.CommandText = "SELECT COUNT(*) FROM vwPermissionsList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.ASM.vwPermissionsList vwPermissions = new Jon.Functional.ASM.vwPermissionsList();
                                status = TransferSQLReader(reader, vwPermissions, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                permissionList.Add(vwPermissions);
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


        #region Permissions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Permissions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.Permission permission, out Jon.Functional.ASM.PermissionId permissionId)
        {
            // Initialize
            permissionId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.Permission _permission = new Jon.Services.Dbio.ASM.Permission();
                BufferMgr.TransferBuffer(permission, _permission);
                dbContext.Permissions.Add(_permission);
                dbContext.SaveChanges();
                if (_permission.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "Permission not created"));
                }
                permissionId = new PermissionId(_permission.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.PermissionId permissionId, out Jon.Services.Dbio.ASM.Permission permission)
        {
            // Initialize
            permission = null;


            try
            {
                permission = dbContext.Permissions.Where(r => r.Id == permissionId.Id).SingleOrDefault();
                if (permission == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", permissionId.Id))));
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
        private jonStatus read(ASMContext dbContext, string permissionname, out Jon.Services.Dbio.ASM.Permission permission)
        {
            // Initialize
            permission = null;


            try
            {
                permission = dbContext.Permissions.Where(r => r.Name == permissionname).SingleOrDefault();
                if (permission == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", permissionname))));
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.Permission permission)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                PermissionId permissionId = new PermissionId(permission.Id);
                Jon.Services.Dbio.ASM.Permission _permission = null;
                status = read(dbContext, permissionId, out _permission);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(permission, _permission);
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
                Jon.Services.Dbio.ASM.Permission _permission = null;
                status = read(dbContext, permissionId, out _permission);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Permissions.Remove(_permission);
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
