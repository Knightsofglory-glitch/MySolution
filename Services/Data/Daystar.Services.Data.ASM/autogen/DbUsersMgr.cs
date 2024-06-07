using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using Jon.Util.Encryption;
using Jon.Functional.ASM;
using Jon.Services.Dbio.ASM;


namespace Jon.Services.Data.ASM
{
    public class DbUsersMgr : DbMgrSessionBased
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
        public DbUsersMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbUsersMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
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
        public jonStatus Create(Jon.Functional.ASM.User user, out Jon.Functional.ASM.UserId userId)
        {
            // Initialize
            jonStatus status = null;
            userId = null;


            // Data rules.
            user.Created = DateTime.Now;
            user.Password = _aesEncryption.Encrypt(user.Password);


            // Create the user
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, user, out userId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.User user, out Jon.Functional.ASM.UserId userId)
        {
            // Initialize
            jonStatus status = null;
            userId = null;


            // Data rules.
            user.Created = DateTime.Now;
            user.Password = _aesEncryption.Encrypt(user.Password);


            // Create the user in this transaction.
            status = create((ASMContext)trans.DbContext, user, out userId);
            if (! jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;

            try
            {
                // Perform read
                using (ASMContext dbContext = new ASMContext())
                {
                    Jon.Services.Dbio.ASM.User _user = null;
                    status = read(dbContext, userId, out _user);

                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    user = new Jon.Functional.ASM.User();
                    BufferMgr.TransferBuffer(_user, user);
                }
            }
            catch (Exception ex)
            {
                return (new jonStatus(Severity.Fatal, ex.Message));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.UserId userId, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.User _user = null;
                status = read((ASMContext)trans.DbContext, userId, out _user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new Jon.Functional.ASM.User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(string username, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.User _user = null;
                status = read(dbContext, username, out _user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new Jon.Functional.ASM.User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, string username, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;


            // Perform transaction read.
            Jon.Services.Dbio.ASM.User _user = null;
            status = read((ASMContext)trans.DbContext, username, out _user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            user = new Jon.Functional.ASM.User();
            BufferMgr.TransferBuffer(_user, user);
            
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.LoginRequest loginRequest, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.User _user = null;
                status = read(dbContext, loginRequest.Username, loginRequest.Password, out _user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user = new Jon.Functional.ASM.User();
                BufferMgr.TransferBuffer(_user, user);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.ApplicationId applicationId, out List<Jon.Functional.ASM.User> userList)
        {
            // Initialize
            jonStatus status = null;
            userList = null;

            try
            {
                // Perform read
                using (ASMContext dbContext = new ASMContext())
                {
                    List<Jon.Services.Dbio.ASM.User> _userList = null;
                    status = read(dbContext, applicationId, out _userList);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    userList = new List<Jon.Functional.ASM.User>();
                    foreach (Jon.Services.Dbio.ASM.User _user in _userList)
                    {
                        Jon.Functional.ASM.User user = new Jon.Functional.ASM.User();
                        BufferMgr.TransferBuffer(_user, user);
                        userList.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                return (new jonStatus(Severity.Fatal, ex.Message));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.ApplicationId applicationId, out List<Jon.Functional.ASM.User> userList)
        {
            // Initialize
            jonStatus status = null;
            userList = null;

            try
            {
                // Perform read
                using (ASMContext dbContext = new ASMContext())
                {
                    List<Jon.Services.Dbio.ASM.User> _userList = null;
                    status = read((ASMContext)trans.DbContext, applicationId, out _userList);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }

                    userList = new List<Jon.Functional.ASM.User>();
                    foreach (Jon.Services.Dbio.ASM.User _user in _userList)
                    {
                        Jon.Functional.ASM.User user = new Jon.Functional.ASM.User();
                        BufferMgr.TransferBuffer(_user, user);
                        userList.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                return (new jonStatus(Severity.Fatal, ex.Message));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(Jon.Functional.ASM.ApplicationId applicationId, string username, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;

            try
            {
                // Perform read
                using (ASMContext dbContext = new ASMContext())
                {
                    Jon.Services.Dbio.ASM.User _user = null;
                    status = read(dbContext, applicationId, username, out _user);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    user = new Jon.Functional.ASM.User();
                    BufferMgr.TransferBuffer(_user, user);
                }
            }
            catch (Exception ex)
            {
                return (new jonStatus(Severity.Fatal, ex.Message));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.ApplicationId applicationId, string username, out Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            user = null;

            try
            {
                // Perform read
                using (ASMContext dbContext = new ASMContext())
                {
                    Jon.Services.Dbio.ASM.User _user = null;
                    status = read((ASMContext)trans.DbContext, applicationId, username, out _user);
                    if (!jonStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    user = new Jon.Functional.ASM.User();
                    BufferMgr.TransferBuffer(_user, user);
                }
            }
            catch (Exception ex)
            {
                return (new jonStatus(Severity.Fatal, ex.Message));
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Update(Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.User user)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, user);
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
            if (! jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus List(QueryOptions queryOptions, out List<Jon.Functional.ASM.vwUsersList> userList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            userList = null;
            queryResponse = null;

            try
            {
                // Get entities
                userList = new List<Jon.Functional.ASM.vwUsersList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwUsersList", typeof(Jon.Functional.ASM.User), out sSelectQuery);
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
                        string sWhereClause = BuildWhereClauseForAdo(queryOptions, typeof(Jon.Functional.ASM.User));
                        command.CommandText = "SELECT COUNT(*) FROM vwUsersList " + sWhereClause;
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Jon.Functional.ASM.vwUsersList vwUsers = new Jon.Functional.ASM.vwUsersList();
                                status = TransferSQLReader(reader, vwUsers, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                userList.Add(vwUsers);
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

        public jonStatus SetPassword(PasswordPair passwordPair)
        {
            // Initialize
            jonStatus status = null;


            // Data rules.
            passwordPair.Modified = DateTime.Now;
            passwordPair.Password = _aesEncryption.Encrypt(passwordPair.Password);


            // Set password
            using (ASMContext dbContext = new ASMContext())
            {
                status = setPassword(dbContext, passwordPair);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus SetPassword(DbMgrTransaction trans, PasswordPair passwordPair)
        {
            // Initialize
            jonStatus status = null;


            // Data rules.
            passwordPair.Modified = DateTime.Now;
            passwordPair.Password = _aesEncryption.Encrypt(passwordPair.Password);


            // Set the password in this transaction.
            status = setPassword((ASMContext)trans.DbContext, passwordPair);
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


        #region Users
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Users
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.User user, out Jon.Functional.ASM.UserId userId)
        {
            // Initialize
            userId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.User _user = new Jon.Services.Dbio.ASM.User();
                BufferMgr.TransferBuffer(user, _user);
                dbContext.Users.Add(_user);
                dbContext.SaveChanges();
                if (_user.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "User not created"));
                }
                userId = new UserId(_user.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }

        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.UserId userId, out Jon.Services.Dbio.ASM.User user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Id == userId.Id).SingleOrDefault();
                if (user == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userId.Id))));
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
        private jonStatus read(ASMContext dbContext, string username, out Jon.Services.Dbio.ASM.User user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Username == username).SingleOrDefault();
                if (user == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", username))));
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
        private jonStatus read(ASMContext dbContext, string username, string password, out Jon.Services.Dbio.ASM.User user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Username == username && r.Password == password).SingleOrDefault();
                if (user == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Username {0}  password {1} not found", username, password))));
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.ApplicationId applicationId, out List<Jon.Services.Dbio.ASM.User> userList)
        {
            // Initialize
            userList = null;


            try
            {
                userList = dbContext.Users.Where(r => r.ApplicationId == applicationId.Id).ToList();
                if (userList == null)
                {
                    return (new jonStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Error searching users for application: {0}", applicationId.Id))));
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
        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.ApplicationId applicationId, string username, out Jon.Services.Dbio.ASM.User user)
        {
            // Initialize
            user = null;


            try
            {
                user = dbContext.Users.Where(r => r.Username == username && r.ApplicationId == applicationId.Id).SingleOrDefault();
                if (user == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Error searching username {0} for applicationId: {1}", username, applicationId.Id))));
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

        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.User user)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                UserId userId = new UserId(user.Id);
                Jon.Services.Dbio.ASM.User _user = null;
                status = read(dbContext, userId, out _user);
                if (! jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(user, _user);
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
                Jon.Services.Dbio.ASM.User _user = null;
                status = read(dbContext, userId, out _user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Users.Remove(_user);
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

        private jonStatus setPassword(ASMContext dbContext, PasswordPair passwordPair)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the user.
                UserId userId = new UserId(passwordPair.Id);
                Jon.Services.Dbio.ASM.User _user = null;
                status = read(dbContext, userId, out _user);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the password.
                _user.Password = passwordPair.Password;
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
