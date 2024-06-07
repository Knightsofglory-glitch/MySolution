using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Services.Dbio.ASM;
using Jon.Dio.MyDatabase;


namespace Jon.Services.Data
{
    public class DbMgrTransaction
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbContext _dbContext = null;
        private IDbContextTransaction _transaction = null;
        private string _database = null;
        private string _name = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMgrTransaction(string name)
        {
            _name = name;
            initialize();
        }
        public DbMgrTransaction(string database, string name)
        {
            this._database = database;
            _name = name;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public string Database
        {
            get
            {
                return (this._database);
            }
        }
        public DbContext DbContext
        {
            get
            {
                return (this._dbContext);
            }
        }
        public string TransactionName
        {
            get
            {
                return (this._name);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public jonStatus BeginTransaction()
        {
            try
            {
                _transaction = _dbContext.Database.BeginTransaction();
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Error, String.Format("EXCEPTION: starting transaction {0}: {1}", _name, ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus RollbackTransaction()
        {
            if (_transaction == null)
            {
                return (new jonStatus(Severity.Error, String.Format("Error: there is no current transaction to rollback {0}", _name)));
            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            return (new jonStatus(Severity.Success));
        }
        public jonStatus CommitTransaction()
        {
            if (_transaction == null)
            {
                return (new jonStatus(Severity.Error, String.Format("Error: there is no current transaction to commit {0}", _name)));
            }
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;

            return (new jonStatus(Severity.Success));
        }
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
                // TODO: placeholder for now
                if (this._database == null || this._database == "ASM")
                {
                    _dbContext = new ASMContext();
                }
                else if (this._database == "Mydatabase")
                {
                    _dbContext = new MyDatabaseContext();
                }
                if (_dbContext == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: DbMgrTransaction: Invalid database specified: {0}", this._database)));
                }
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }
        #endregion
    }
}
