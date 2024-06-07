using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jon.Functional.ASM;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using Jon.Services.Data;
using Jon.Dio.MyDatabase;


/**********************************************************************************************************************************
 * 
 * Class: DbMyTablesMgr
 * 
 * Template: CRUDL (Create, Read, Update, Delete and List)
 * 
 * NOTE: This is auto-generated name.  Do not change this name manually!
 *       Custom name codes in the DbMyTablesManager.  
 *       
 * (c) 2022, Jon Television
 **********************************************************************************************************************************/
namespace Jon.Services.Data.MyDatabase
{
    public class DbMyTablesMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private string _connectionString = null;
        private DbMyTablesManager _dbMyTablesManager = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMyTablesMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbMyTablesMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
            : base(userSession)
        {
            _connectionString = connectionString;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region CRUDL 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUDL
        //----------------------------------------------------------------------------------------------------------------------------------
        public jonStatus Create(Jon.Functional.MySolution.MyTable myTable, out Jon.Functional.MySolution.MyTableId MyTableId)
        {
            // Initialize
            jonStatus status = null;
            MyTableId = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Create(RuleCalloutEvents.PRE_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Create the MyTable
            //================================================================================
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = create(dbContext, myTable, out MyTableId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            myTable.Id = MyTableId.Id;


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Create(RuleCalloutEvents.POST_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.MySolution.MyTable myTable, out Jon.Functional.MySolution.MyTableId MyTableId)
        {
            // Initialize
            jonStatus status = null;
            MyTableId = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Create(RuleCalloutEvents.PRE_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            //================================================================================
            // Create the MyTable in this transaction.
            //================================================================================
            status = create((MyDatabaseContext)trans.DbContext, myTable, out MyTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Create(RuleCalloutEvents.POST_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(Jon.Functional.MySolution.MyTableId myTableId, out Jon.Functional.MySolution.MyTable myTable)
        {
            // Initialize
            jonStatus status = null;
            myTable = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.PRE_CALLOUT, myTableId, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform read
            //================================================================================
            Jon.Dio.MyDatabase.MyTable _myTable = null;
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = read(dbContext, myTableId, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                myTable = new Functional.MySolution.MyTable();
                BufferMgr.TransferBuffer(_myTable, myTable);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.POST_CALLOUT, myTableId, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.MySolution.MyTableId myTableId, out Jon.Functional.MySolution.MyTable myTable)
        {
            // Initialize
            jonStatus status = null;
            myTable = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.PRE_CALLOUT, myTableId, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform read
            //================================================================================
            Jon.Dio.MyDatabase.MyTable _myTable = null;
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = read((MyDatabaseContext)trans.DbContext, myTableId, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                myTable = new Jon.Functional.MySolution.MyTable();
                BufferMgr.TransferBuffer(_myTable, myTable);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.POST_CALLOUT, myTableId, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(string name, out Jon.Functional.MySolution.MyTable myTable)
        {
            // Initialize
            jonStatus status = null;
            myTable = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.PRE_CALLOUT, name, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform read
            //================================================================================
            Jon.Dio.MyDatabase.MyTable _myTable = null;
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = read(dbContext, name, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                myTable = new Jon.Functional.MySolution.MyTable();
                BufferMgr.TransferBuffer(_myTable, myTable);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.POST_CALLOUT, name, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, string name, out Jon.Functional.MySolution.MyTable myTable)
        {
            // Initialize
            jonStatus status = null;
            myTable = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.PRE_CALLOUT, name, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform read
            //================================================================================
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                Jon.Dio.MyDatabase.MyTable _myTable = null;
                status = read((MyDatabaseContext)trans.DbContext, name, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                myTable = new Jon.Functional.MySolution.MyTable();
                BufferMgr.TransferBuffer(_myTable, myTable);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Read(RuleCalloutEvents.POST_CALLOUT, name, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Update(Jon.Functional.MySolution.MyTable myTable, bool bSynchronize = false)
        {
            // Initialize
            jonStatus status = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Update(RuleCalloutEvents.PRE_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform update
            //================================================================================
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = update(dbContext, myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Update(RuleCalloutEvents.POST_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.MySolution.MyTable myTable, bool bSynchronize = false)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Update(RuleCalloutEvents.PRE_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform update in this transaction.
            //================================================================================
            status = update((MyDatabaseContext)trans.DbContext, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Update(RuleCalloutEvents.POST_CALLOUT, myTable);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(Jon.Functional.MySolution.MyTableId myTableId)
        {
            // Initialize
            jonStatus status = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Delete(RuleCalloutEvents.PRE_CALLOUT, myTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform delete.
            //================================================================================
            using (MyDatabaseContext dbContext = new MyDatabaseContext())
            {
                status = delete(dbContext, myTableId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Delete(RuleCalloutEvents.POST_CALLOUT, myTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.MySolution.MyTableId myTableId)
        {
            // Initialize
            jonStatus status = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Delete(RuleCalloutEvents.PRE_CALLOUT, myTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Perform delete in this transaction.
            //================================================================================
            status = delete((MyDatabaseContext)trans.DbContext, myTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.Delete(RuleCalloutEvents.POST_CALLOUT, myTableId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.MySolution.vwMyTablesList> myTableList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            myTableList = null;
            queryResponse = null;


            //--------------------------------------------------------------------------------
            // PRE Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.List(RuleCalloutEvents.PRE_CALLOUT, queryOptions, myTableList, queryResponse);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }


            //================================================================================
            // Get list of entities
            //================================================================================
            try
            {
                // Get entities
                myTableList = new List<Jon.Functional.MySolution.vwMyTablesList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwMyTablesList", typeof(Jon.Functional.MySolution.MyTable), out sSelectQuery);
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
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(Jon.Functional.MySolution.MyTable));
                        command.CommandText = "SELECT COUNT(*) FROM vwMyTablesList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.MySolution.vwMyTablesList vwMyTables = new Jon.Functional.MySolution.vwMyTablesList();
                                status = TransferSQLReader(reader, vwMyTables, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                myTableList.Add(vwMyTables);
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


            //--------------------------------------------------------------------------------
            // POST Data rules
            //--------------------------------------------------------------------------------
            status = _dbMyTablesManager.List(RuleCalloutEvents.POST_CALLOUT, queryOptions, myTableList, queryResponse);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
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
                _dbMyTablesManager = new DbMyTablesManager(this.UserSession);

                this.ConnectionString = ConnectionMgr.MyDatabaseConnectionString;
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }

        #region CRUDL 
        /*----------------------------------------------------------------------------------------------------------------------------------
         * CRUDL
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(MyDatabaseContext dbContext, Jon.Functional.MySolution.MyTable myTable, out Jon.Functional.MySolution.MyTableId myTableId)
        {
            // Initialize
            myTableId = null;


            // Perform create
            try
            {
                Jon.Dio.MyDatabase.MyTable _myTable = new Jon.Dio.MyDatabase.MyTable();
                BufferMgr.TransferBuffer(myTable, _myTable);
                dbContext.MyTables.Add(_myTable);
                dbContext.SaveChanges();
                if (_myTable.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "MyTable not created"));
                }
                myTableId = new Jon.Functional.MySolution.MyTableId(_myTable.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }

        private jonStatus read(MyDatabaseContext dbContext, Jon.Functional.MySolution.MyTableId myTableId, out Jon.Dio.MyDatabase.MyTable myTable)
        {
            // Initialize
            myTable = null;


            try
            {
                myTable = dbContext.MyTables.Where(r => r.Id == myTableId.Id).SingleOrDefault();
                if (myTable == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", myTableId.Id))));
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
        private jonStatus read(MyDatabaseContext dbContext, string name, out Jon.Dio.MyDatabase.MyTable myTable)
        {
            // Initialize
            myTable = null;


            try
            {
                myTable = dbContext.MyTables.Where(r => r.Name == name).SingleOrDefault();
                if (myTable == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("MyTable name {0} not found", name))));
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

        private jonStatus update(MyDatabaseContext dbContext, Jon.Functional.MySolution.MyTable myTable)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Functional.MySolution.MyTableId myTableId = new Jon.Functional.MySolution.MyTableId(myTable.Id);
                Jon.Dio.MyDatabase.MyTable _myTable = null;
                status = read(dbContext, myTableId, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(myTable, _myTable);
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

        private jonStatus delete(MyDatabaseContext dbContext, Jon.Functional.MySolution.MyTableId myTableId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Dio.MyDatabase.MyTable _myTable = null;
                status = read(dbContext, myTableId, out _myTable);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.MyTables.Remove(_myTable);
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

