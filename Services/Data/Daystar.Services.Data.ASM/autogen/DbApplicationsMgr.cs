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
using Jon.Functional.ASM;
using Jon.Services.Dbio.ASM;
using Jon.Services.Data;


namespace Jon.Services.Data.ASM
{
    public class DbApplicationsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private string _connectionString = null;
        private ApplicationsDataMgr _applicationsDataMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbApplicationsMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbApplicationsMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
            : base(userSession)
        {
            _connectionString = connectionString;
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
        public jonStatus Create(Jon.Functional.ASM.Application application, out Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize
            jonStatus status = null;
            applicationId = null;


            // Data rules
            status = _applicationsDataMgr.Create(application);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Create the application
            using (ASMContext dbContext = new ASMContext())
            {
                status = create(dbContext, application, out applicationId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Create(DbMgrTransaction trans, Jon.Functional.ASM.Application application, out Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize
            jonStatus status = null;
            applicationId = null;


            // Data rules
            status = _applicationsDataMgr.Create(application);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Create the application in this transaction.
            status = create((ASMContext)trans.DbContext, application, out applicationId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(Jon.Functional.ASM.ApplicationId applicationId, out Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;
            application = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read(dbContext, applicationId, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Read(_application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                application = new Jon.Functional.ASM.Application();
                BufferMgr.TransferBuffer(_application, application);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, Jon.Functional.ASM.ApplicationId applicationId, out Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;
            application = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read((ASMContext)trans.DbContext, applicationId, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Read(_application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                application = new Jon.Functional.ASM.Application();
                BufferMgr.TransferBuffer(_application, application);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Read(string name, string label, string tag, out Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;
            application = null;


            // Perform read
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read(dbContext, name, label, tag, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Read(_application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                application = new Jon.Functional.ASM.Application();
                BufferMgr.TransferBuffer(_application, application);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(DbMgrTransaction trans, string name, string label, string tag, out Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;
            application = null;


            // Perform read.
            using (ASMContext dbContext = new ASMContext())
            {
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read((ASMContext)trans.DbContext, name, label, tag, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Read(_application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Transfer model
                application = new Jon.Functional.ASM.Application();
                BufferMgr.TransferBuffer(_application, application);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Update(Jon.Functional.ASM.Application application, bool bSynchronize = false)
        {
            // Initialize
            jonStatus status = null;


            // Perform update.
            using (ASMContext dbContext = new ASMContext())
            {
                status = update(dbContext, application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Update(DbMgrTransaction trans, Jon.Functional.ASM.Application application, bool bSynchronize = false)
        {
            // Initialize
            jonStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((ASMContext)trans.DbContext, application);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete.
            using (ASMContext dbContext = new ASMContext())
            {
                status = delete(dbContext, applicationId);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Delete(DbMgrTransaction trans, Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize
            jonStatus status = null;


            // Perform delete in this transaction.
            status = delete((ASMContext)trans.DbContext, applicationId);
            if (!jonStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus List(QueryOptions queryOptions, out List<Functional.ASM.vwApplicationsList> applicationList, out QueryResponse queryResponse)
        {
            // Initialize
            jonStatus status = null;
            applicationList = null;
            queryResponse = null;

            try
            {
                // Get entities
                applicationList = new List<Functional.ASM.vwApplicationsList>();

                string sSelectQuery = null;
                status = BuildSelectQueryAdo(queryOptions, "vwApplicationsList", typeof(Jon.Functional.ASM.Application), out sSelectQuery);
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
                        command.CommandText = "SELECT COUNT(*) FROM vwApplicationsList";
                        connection.Open();
                        totalRecords = (int)command.ExecuteScalar();


                        // Get records
                        command.CommandText = sSelectQuery;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Functional.ASM.vwApplicationsList vwApplications = new Functional.ASM.vwApplicationsList();
                                status = TransferSQLReader(reader, vwApplications, true);
                                if (!jonStatusDef.IsSuccess(status))
                                {
                                    return (status);
                                }
                                applicationList.Add(vwApplications);
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
                _applicationsDataMgr = new ApplicationsDataMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new jonStatus(Severity.Success));
        }


        #region Applications
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Applications
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private jonStatus create(ASMContext dbContext, Jon.Functional.ASM.Application application, out Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize
            applicationId = null;


            // Perform create
            try
            {
                Jon.Services.Dbio.ASM.Application _application = new Jon.Services.Dbio.ASM.Application();
                BufferMgr.TransferBuffer(application, _application);
                dbContext.Applications.Add(_application);
                dbContext.SaveChanges();
                if (_application.Id == 0)
                {
                    return (new jonStatus(Severity.Error, "Application not created"));
                }
                applicationId = new Jon.Functional.ASM.ApplicationId(_application.Id);
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }

        private jonStatus read(ASMContext dbContext, Jon.Functional.ASM.ApplicationId applicationId, out Jon.Services.Dbio.ASM.Application application)
        {
            // Initialize
            application = null;


            try
            {
                application = dbContext.Applications.Where(r => r.Id == applicationId.Id).SingleOrDefault();
                if (application == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", applicationId.Id))));
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
        private jonStatus read(ASMContext dbContext, string name, string label, string tag, out Jon.Services.Dbio.ASM.Application application)
        {
            // Initialize
            application = null;


            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        if (!string.IsNullOrEmpty(tag))
                        {
                            application = dbContext.Applications.Where(r => r.Name == name && r.Label == label && r.Tag == tag).SingleOrDefault();
                        }
                        else
                        {
                            application = dbContext.Applications.Where(r => r.Name == name && r.Label == label).SingleOrDefault();
                        }
                    }
                    else if (!string.IsNullOrEmpty(tag))
                    {
                        application = dbContext.Applications.Where(r => r.Name == name && r.Tag == tag).SingleOrDefault();
                    }
                    else
                    {
                        application = dbContext.Applications.Where(r => r.Name == name).SingleOrDefault();
                    }
                }
                else if (!string.IsNullOrEmpty(label))
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        application = dbContext.Applications.Where(r => r.Label == label && r.Tag == tag).SingleOrDefault();
                    }
                    else
                    {
                        application = dbContext.Applications.Where(r => r.Tag == tag).SingleOrDefault();
                    }
                }
                else
                {
                    return (new jonStatus(Severity.Error, "Name and/or Label and/or Tag required."));
                }
                if (application == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Application label {0} and tag {1} not found", label, tag))));
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
        private jonStatus read(ASMContext dbContext, string tag, out Jon.Services.Dbio.ASM.Application application)
        {
            // Initialize
            application = null;


            try
            {
                application = dbContext.Applications.Where(r =>r.Tag==tag).SingleOrDefault();
                if (application == null)
                {
                    return (new jonStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Application name {0} not found", tag))));
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
     
        private jonStatus update(ASMContext dbContext, Jon.Functional.ASM.Application application)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Functional.ASM.ApplicationId applicationId = new Jon.Functional.ASM.ApplicationId(application.Id);
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read(dbContext, applicationId, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Update(application, _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(application, _application);
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

        private jonStatus delete(ASMContext dbContext, Jon.Functional.ASM.ApplicationId applicationId)
        {
            // Initialize 
            jonStatus status = null;


            try
            {
                // Read the record.
                Jon.Services.Dbio.ASM.Application _application = null;
                status = read(dbContext, applicationId, out _application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Data rules
                status = _applicationsDataMgr.Delete(_application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Applications.Remove(_application);
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
