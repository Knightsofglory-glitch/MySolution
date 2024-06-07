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
    public class DbAccountsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbApplicationsMgr _dbApplicationsMgr = null;
        private DbUsersMgr _dbUsersMgr = null;
        private DbUserPermissionsMgr _dbUserPermissionsMgr = null;
        private DbGroupUsersMgr _dbGroupUsersMgr = null;
        private DbGroupPermissionsMgr _dbGroupPermissionsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbAccountsMgr(Jon.Functional.ASM.UserSession userSession)
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
        public jonStatus Login(LoginRequest loginRequest, Jon.Functional.ASM.User user, out Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            userSession = null;


            ////// User must be enabled and active.
            ////// (Intended to be data rule AND business rule).
            ////if (!user.IsEnabled)
            ////{
            ////    return (new jonStatus(Severity.Error, "User is not enabled"));
            ////}
            ////if (!user.IsActive)
            ////{
            ////    return (new jonStatus(Severity.Error, "User is not active"));
            ////}

            // Get user application
            Jon.Functional.ASM.Application application = null;
            status = GetUserApplication(user, out application);
            if (!jonStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }


            // Get user permissions
            UserId userId = new UserId(user.Id);
            List<Jon.Functional.ASM.Permission> userPermissionList = null;
            status = GetUserPermissions(userId, out userPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Build user session
            Jon.Functional.ASM.UserSession _userSession = new Jon.Functional.ASM.UserSession();
            _userSession.IPAddress = loginRequest.IPAddress;
            _userSession.UserAgent = loginRequest.UserAgent;
            _userSession.LastAction = DateTime.Now;
            _userSession.UserId = user.Id;
            _userSession.Application = application;
            _userSession.User = user;
            _userSession.Permissions = userPermissionList;
            _userSession.Created = DateTime.Now;


            // Write the user session
            Jon.Functional.ASM.UserSessionId _userSessionId = null;
            DbUserSessionsMgr dbUserSessionsMgr = new DbUserSessionsMgr(this.UserSession);
            status = dbUserSessionsMgr.Create(_userSession, out _userSessionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return the user session
            Jon.Functional.ASM.UserSession __userSession = null;
            status = dbUserSessionsMgr.Read(_userSessionId, out __userSession);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userSession = __userSession;
            userSession.User = user;
            userSession.Permissions = userPermissionList;

            return (new jonStatus(Severity.Success));
        }
        public jonStatus BuildUserSession(Jon.Functional.ASM.User user, string userData1, out Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            userSession = null;



            // User must be enabled
            // (Intended to be data rule AND business rule).
            if (!user.IsEnabled)
            {
                return (new jonStatus(Severity.Error, "User is not enabled"));
            }

            // Get user permissions
            UserId userId = new UserId(user.Id);
            List<Jon.Functional.ASM.Permission> userPermissionList = null;
            status = GetUserPermissions(userId, out userPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Build user session
            Jon.Functional.ASM.UserSession _userSession = new Jon.Functional.ASM.UserSession();
            _userSession.LastAction = DateTime.Now;
            _userSession.UserId = user.Id;
            _userSession.User = user;
            _userSession.Permissions = userPermissionList;
            _userSession.Created = DateTime.Now;
            _userSession.UserData1 = userData1;


            // Write the user session
            UserSessionId _userSessionId = null;
            DbUserSessionsMgr dbUserSessionsMgr = new DbUserSessionsMgr(this.UserSession);
            status = dbUserSessionsMgr.Create(_userSession, out _userSessionId);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return the user session
            Jon.Functional.ASM.UserSession __userSession = null;
            status = dbUserSessionsMgr.Read(_userSessionId, out __userSession);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userSession = __userSession;
            userSession.User = user;
            userSession.Permissions = userPermissionList;


            return (new jonStatus(Severity.Success));
        }
        public jonStatus ValidateUserSession(Jon.Functional.ASM.UserSessionId userSessionId, out Jon.Functional.ASM.UserSession userSession)
        {
            // Initialize
            jonStatus status = null;
            userSession = null;


            // See if user session exists
            DbUserSessionsMgr dbUserSessionsMgr = new DbUserSessionsMgr(this.UserSession);
            Jon.Functional.ASM.UserSession __userSession = null;
            status = dbUserSessionsMgr.Read(userSessionId, out __userSession);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user record
            UserId userId = new UserId(__userSession.UserId);
            Jon.Functional.ASM.User user = null;
            DbUsersMgr dbUsersMgr = new DbUsersMgr(this.UserSession);
            status = dbUsersMgr.Read(userId, out user);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user permissions
            List<Jon.Functional.ASM.Permission> userPermissionList = null;
            status = GetUserPermissions(userId, out userPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get application if user is assigned to one.
            Jon.Functional.ASM.Application application = null;
            if (user.ApplicationId.HasValue)
            {
                Jon.Functional.ASM.ApplicationId appilcationId = new Functional.ASM.ApplicationId(user.ApplicationId.Value);
                DbApplicationsMgr dbApplicationsMgr = new DbApplicationsMgr(this.UserSession);
                status = dbApplicationsMgr.Read(appilcationId, out application);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }


            // Return user session
            userSession = __userSession;
            userSession.User = user;
            userSession.Application = application;
            userSession.Permissions = userPermissionList;


            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetUserGroups(UserId userId, out List<Jon.Functional.ASM.Group> groupList)
        {
            // Initialize
            jonStatus status = null;
            groupList = null;


            // Get all user groups.
            UserGroupList userGroupList = null;
            status = _dbGroupUsersMgr.Read(userId, out userGroupList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return groups
            groupList = userGroupList.GroupList;

            return (new jonStatus(Severity.Success));
        }

        public jonStatus GetGroupUsers(GroupId groupId, out List<Jon.Functional.ASM.User> userList)
        {
            // Initialize
            jonStatus status = null;
            userList = null;


            // Get all user groups.
            GroupUserList groupUserList = null;
            status = _dbGroupUsersMgr.Read(groupId, out groupUserList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return groups
            userList = groupUserList.UserList;

            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetGroupPermissions(GroupId groupId, out List<Jon.Functional.ASM.Permission> permissionList)
        {
            // Initialize
            jonStatus status = null;
            permissionList = null;


            // Get all group permissions.
            GroupPermissionList groupPermissionList = null;
            status = _dbGroupPermissionsMgr.Read(groupId, out groupPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return permissions
            permissionList = groupPermissionList.PermissionList;

            return (new jonStatus(Severity.Success));
        }

        public jonStatus GetUserPermissions(Jon.Functional.ASM.UserId userId, out List<Jon.Functional.ASM.Permission> permissionList)
        {
            // Initialize
            jonStatus status = null;
            permissionList = null;


            // Get all user permissions.
            UserPermissionList userPermissionList = null;
            status = _dbUserPermissionsMgr.Read(userId, out userPermissionList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return permissions
            permissionList = userPermissionList.PermissionList;

            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetUserApplication(Jon.Functional.ASM.User user, out Jon.Functional.ASM.Application application)
        {
            // Initialize
            jonStatus status = null;
            application = null;

            
            // Ensure user is assigned to an application
            if (user.ApplicationId == null || user.ApplicationId < BaseId.VALID_ID)
            {
                return (new jonStatus(Severity.Warning, "User is not assigned to an application"));
            }

            // Get user application
            Jon.Functional.ASM.ApplicationId applicationId = new Functional.ASM.ApplicationId(user.ApplicationId.Value);
            status = _dbApplicationsMgr.Read(applicationId, out application);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus GetPermissionGroups(PermissionId permissionId, out List<Jon.Functional.ASM.Group> groupList)
        {
            // Initialize
            jonStatus status = null;
            groupList = null;


            // Get all user groups.
            PermissionGroupList permissionGroupList = null;
            status = _dbGroupPermissionsMgr.Read(permissionId, out permissionGroupList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return groups
            groupList = permissionGroupList.GroupList;

            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetPermissionUsers(PermissionId permissionId, out List<Jon.Functional.ASM.User> groupList)
        {
            // Initialize
            jonStatus status = null;
            groupList = null;


            // Get all user groups.
            PermissionUserList permissionUserList = null;
            status = _dbUserPermissionsMgr.Read(permissionId, out permissionUserList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return groups
            groupList = permissionUserList.UserList;

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
                _dbApplicationsMgr = new DbApplicationsMgr(this.UserSession);
                _dbUsersMgr = new DbUsersMgr(this.UserSession);
                _dbUserPermissionsMgr = new DbUserPermissionsMgr(this.UserSession);
                _dbGroupUsersMgr = new DbGroupUsersMgr(this.UserSession);
                _dbGroupPermissionsMgr = new DbGroupPermissionsMgr(this.UserSession);
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
