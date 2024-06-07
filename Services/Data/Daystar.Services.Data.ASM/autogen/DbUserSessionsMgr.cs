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
    public class DbUserSessionsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        AESEncryption _aesEncryption = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbUserSessionsMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbUserSessionsMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
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
        public jonStatus Create(Jon.Functional.ASM.UserSession userSession, out Jon.Functional.ASM.UserSessionId userSessionId)
        {
            // Initialize
            jonStatus status = null;
            userSessionId = null;


            // Data rules.
            userSession.Created = DateTime.Now;


            // Create the user session
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, userSession, out userSessionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.UserSession userSession, out Jon.Functional.ASM.UserSessionId userSessionId)
        {
            // Initialize
            jonStatus status = null;
            userSessionId = null;


            // Data rules.
            userSession.Created = DateTime.Now;


            // Create the user session in this transaction.
            status = create((ASMContext)trans.DbContext, userSession, out userSessionId);
            if (! jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.UserSessionId userSessionId, out Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            userSession = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.UserSession _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userSession = new Jon.Functional.ASM.UserSession();
                BufferMgr.TransferBuffer(_userSession, userSession);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserSessionId userSessionId, out Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            userSession = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.UserSession _userSession = null;
                status = read((ASMContext)trans.DbContext, userSessionId, out _userSession);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userSession = new Jon.Functional.ASM.UserSession();
                BufferMgr.TransferBuffer(_userSession, userSession);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, userSession);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, userSession);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(Jon.Functional.ASM.UserSessionId userSessionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, userSessionId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.UserSessionId userSessionId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, userSessionId);
            if (! jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.vwUserSessionsList> userSessionList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            userSessionList = null;
            queryResponse = null;

            try
            {
                // Get entities
                userSessionList = new List<Jon.Functional.ASM.vwUserSessionsList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwUserSessionsList", typeof(Jon.Functional.ASM.UserSession), out sSelectQuery);
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
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(Jon.Functional.ASM.UserSession));
                        command.CommandText = "SELECT COUNT(*) FROM vwUserSessionsList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.ASM.vwUserSessionsList vwUserSessions = new Jon.Functional.ASM.vwUserSessionsList();
                                status = TransferSQLReader(reader, vwUserSessions, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                userSessionList.Add(vwUserSessions);
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
                _aesEncryption = new AESEncryption();
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }


        #region UserSessions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserSessions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.UserSession userSession, out Jon.Functional.ASM.UserSessionId userSessionId)
        {
            // Initialize
            userSessionId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.UserSession _userSession = new Jon.Services.Dbio.ASM.UserSession();
                BufferMgr.TransferBuffer(userSession, _userSession);
                dbContext.UserSessions.Add(_userSession);
                dbContext.SaveChanges();
                if (_userSession.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "UserSession not created"));
                }
                userSessionId = new UserSessionId(_userSession.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        private jonStatus read(ASMContext dbContext, UserSessionId userSessionId, out Jon.Services.Dbio.ASM.UserSession userSession)
        {
            // Initialize
            userSession = null;


            try
            {
                userSession = dbContext.UserSessions.Where(r => r.Id == userSessionId.Id).SingleOrDefault();
                if (userSession == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userSessionId.Id))));
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
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                UserSessionId userSessionId = new UserSessionId(userSession.Id);
                Jon.Services.Dbio.ASM.UserSession _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(userSession, _userSession);
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
        private jonStatus delete(ASMContext dbContext, UserSessionId userSessionId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.UserSession _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.UserSessions.Remove(_userSession);
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
