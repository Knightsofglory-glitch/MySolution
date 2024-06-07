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
    public class UsersDataMgr : DbMgrSessionBased
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
        public UsersDataMgr(Jon.Functional.ASM.UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public UsersDataMgr(Jon.Functional.ASM.UserSession userSession, string connectionString)
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
        #endregion

        #endregion
    }
}
