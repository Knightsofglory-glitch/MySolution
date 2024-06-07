using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jon.Functional.ASM;
using Jon.Functional.MySolution;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using Jon.Services.Data;
using Jon.Dio.MyDatabase;


/**********************************************************************************************************************************
 * 
 * Class: DbMyTablesManager
 * 
 * Template: CRUDL (Create, Read, Update, Delete and List)
 * 
 * NOTE: This is auto-generated once.  Thereafter, it is manually managed.
 *       You can add custom code into this class.  Do not modify DbMyTablesMgr. 
 *       
 **********************************************************************************************************************************/
namespace Jon.Services.Data.MyDatabase
{
    public class DbMyTablesManager : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private string _connectionString = null;
        private Jon.Functional.ASM.UserSession _useSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMyTablesManager(UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        public DbMyTablesManager(UserSession userSession, string connectionString)
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
        public jonStatus Create(RuleCalloutEvents ruleCalloutEvent, Jon.Functional.MySolution.MyTable myTable)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }


        public jonStatus Read(RuleCalloutEvents ruleCalloutEvent, Jon.Functional.MySolution.MyTableId myTableId, Jon.Functional.MySolution.MyTable myTable)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus Read(RuleCalloutEvents ruleCalloutEvent, string code, Jon.Functional.MySolution.MyTable myTable)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Update(RuleCalloutEvents ruleCalloutEvent, Jon.Functional.MySolution.MyTable myTable)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus Delete(RuleCalloutEvents ruleCalloutEvent, Jon.Functional.MySolution.MyTableId myTableId)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus List(RuleCalloutEvents ruleCalloutEvent, QueryOptions queryOptions, List<Jon.Functional.MySolution.vwMyTablesList> myTableList, QueryResponse queryResponse)
        {
            if (ruleCalloutEvent == RuleCalloutEvents.PRE_CALLOUT)
            {

            }
            else if (ruleCalloutEvent == RuleCalloutEvents.POST_CALLOUT)
            {

            }
            return (new jonStatus(Severity.Success));
        }
        #endregion


        #region Manual Routines 
        //----------------------------------------------------------------------------------------------------------------------------------
        // Manual Routines 
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
        #endregion
    }
}
