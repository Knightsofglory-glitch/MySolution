using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jon.Functional.ASM;
using Jon.Functional.Util;
using Jon.Util.Status;
using Jon.Util.Buffer;
using Jon.Util.Data;
using System.Linq.Expressions;
using LinqKit;


using System.Data.SqlClient;

namespace Jon.Services.Data
{
    public class DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private string _connectionString = "";

        public const string WHERE_1_EQUALS_1 = " WHERE 1 = 1 ";

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbMgr()
        {
            initialize();
        }
        public DbMgr(UserSession userSession)
        {
            initialize();
        }
        public DbMgr(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                this._connectionString = connectionString;
            }
            else
            {
                this._connectionString = ConnectionMgr.DataManagerConnectionString;
            }
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public string ConnectionString
        {
            get
            {
                return (this._connectionString);
            }
            set
            {
                this._connectionString = value;
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region Transactions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Transactions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public jonStatus GetUniqueTransactionName(string prefix, out string transactionName)
        {
            // Initialize
            transactionName = null;
            transactionName = (prefix == null ? "" : prefix) + Guid.NewGuid().ToString();
            return (new jonStatus(Severity.Success));
        }
        public jonStatus BeginTransaction(string transactionName, out DbMgrTransaction trans)
        {
            trans = new DbMgrTransaction(transactionName);
            return (trans.BeginTransaction());
        }
        public jonStatus BeginTransaction(string database, string transactionName, out DbMgrTransaction trans)
        {
            trans = new DbMgrTransaction(database, transactionName);
            return (trans.BeginTransaction());
        }
        public jonStatus RollbackTransaction(DbMgrTransaction trans)
        {
            return (trans.RollbackTransaction());
        }
        public jonStatus CommitTransaction(DbMgrTransaction trans)
        {
            return (trans.CommitTransaction());
        }
        #endregion


        #region Databases
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Databases
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public string BracketIdentifier(string identifier)
        {
            if (identifier.StartsWith("[") && identifier.EndsWith("]"))
            {
                return (identifier);
            }
            string _identifier = "[" + identifier + "]";
            return (_identifier);
        }
        public string StripBrackets(string identifier)
        {
            string _identifer = identifier.Replace("[", "");
            identifier = _identifer.Replace("]", "");
            return (identifier);
        }
        #endregion


        #region I/O Support Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * I/O Support Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public string BuildSortString(SortColumnList sortColumnList)
        {
            string sOrderBy = " ORDER BY ";
            if (sortColumnList.Columns.Count == 0)
            {
                return (sOrderBy + " 1 ASC");
            }
            StringBuilder sbSort = new StringBuilder();
            for (int i = 0; i < sortColumnList.Columns.Count; i++)
            {
                SortColumn sortColumn = sortColumnList.Columns[i];
                sbSort.Append(sortColumn.Name + " ");
                if (sortColumn.Direction == SortDirection.DESC)
                {
                    sbSort.Append("DESC");
                }
                if (i + 1 < sortColumnList.Columns.Count)
                {
                    sbSort.Append(", ");
                }
            }
            return (sOrderBy + sbSort.ToString());
        }
        public QueryResponse BuildQueryResponse(QueryOptions queryOptions, int count)
        {
            QueryResponse queryResponse = new QueryResponse();
            queryResponse.TotalRecords = count;
            queryResponse.PageNumber = queryOptions.Paging.PageNumber;
            queryResponse.PageSize = queryOptions.Paging.PageSize;
            queryResponse.TotalPages = (int)Math.Ceiling((double)queryResponse.TotalRecords / queryOptions.Paging.PageSize);

            return (queryResponse);
        }
        public string BuildSearchStringWhere(QueryOptions queryOptions, PropertyInfo[] dbProperties)
        {
            // NOTE: THIS ROUTINE IS NOT DONE, IT'S PASSING BACK AN EMPTY WHERE CLAUSE FOR NOW.
            // NOTE: THIS IS EVOLVING.  THE SEARCH STRING NEEDS TO WORK WITH ANY COLUMN DATATYPE TO WHICH ITS VALUE IS APPLICABLE/DOABLE/FEASIBLE.
            string searchString = queryOptions.SearchOptions.SearchString;

            // Determine what column datatypes the search string can be applied to.
            // NOTE: STRING IS ALWAYS COMPATIBLE WITH ANY SEARCH STRING VALUE.
            int _integerSearchString = -1;
            bool bIntegerCompatible = int.TryParse(searchString, out _integerSearchString);

            decimal _decimalSearchString = -1.0m;
            bool bDecimalCompatible = decimal.TryParse(searchString, out _decimalSearchString);

            DateTime _dateTimeSearchString = new DateTime();
            bool bDateTimeCompatible = DateTime.TryParse(searchString, out _dateTimeSearchString);


            // Build WHERE clause for each column that can have the search string value applied to it.
            string sqlTerm = "'%" + searchString + "%'";
            StringBuilder sbWhere = new StringBuilder("");
            for (int idx = 0; idx < dbProperties.Length; idx++)
            {
                PropertyInfo propertyInfo = dbProperties[idx];
                if (propertyInfo.PropertyType == typeof(System.String))
                {
                    if (sbWhere.Length > 0)
                    {
                        sbWhere.Append(" OR ");
                    }
                    sbWhere.Append(" [" + propertyInfo.Name + "] LIKE " + sqlTerm);
                }
                else if (propertyInfo.PropertyType == typeof(System.Int32))
                {
                    if (propertyInfo.Name == "Id") { continue; }
                }
                else if (propertyInfo.PropertyType == typeof(System.Int16))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Int64))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Decimal))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.Double))
                {

                }
                else if (propertyInfo.PropertyType == typeof(System.DateTime))
                {

                }
            }
            return (" WHERE " + sbWhere.ToString());
        }
        public jonStatus BuildQueryResponse(int count, QueryOptions queryOptions, out QueryResponse queryResponse)
        {
            queryResponse = new QueryResponse();
            try
            {
                queryResponse.TotalRecords = count;
                queryResponse.PageNumber = queryOptions.Paging.PageNumber;
                queryResponse.PageSize = queryOptions.Paging.PageSize;              
                if (queryOptions.Paging.PageSize == 0)
                {
                    queryOptions.Paging.PageSize = count > 0 ? count : 1;
                }

                int _totalPages = 1;
                if (count > 0)
                {
                    _totalPages = count / queryOptions.Paging.PageSize;
                    if (count % queryOptions.Paging.PageSize > 0)
                    {
                        _totalPages++;
                    }
                }
                queryResponse.TotalPages = _totalPages;
            }
            catch (System.Exception ex)
            {
                return (new jonStatus(Severity.Fatal, String.Format("EXCEPTION: DbMgr.BuildQueryResponse: {0}", ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new jonStatus(Severity.Success));
        }

        public jonStatus BuildSelectQueryAdo(QueryOptions queryOptions, string sTableName, Type type, out string sSelectQuery)
        {
            jonStatus status = null;
            sSelectQuery = "SELECT * FROM " + sTableName;

            try
            {
                string sSortOrder = string.Empty;
                string sPaging = string.Empty;
                string sWhereClause = BuildWhereClauseForAdo(queryOptions, type);

                //Build the sort order string
                if (queryOptions != null && queryOptions.SortColumns != null)
                {
                    sSortOrder = BuildSortOrder(queryOptions.SortColumns);
                }
                else
                {
                    sSortOrder = " ORDER BY 1 ";
                }

                //build  the paging query piece
                if (queryOptions != null && queryOptions.Paging != null)
                {
                    int nPageSize = queryOptions.Paging.PageSize;
                    int nPageNumber = queryOptions.Paging.PageNumber;

                    sPaging = $" OFFSET {nPageSize}*({nPageNumber} - 1) ROWS FETCH NEXT {nPageSize} ROWS ONLY ";
                }

                sSelectQuery = sSelectQuery + sWhereClause + sSortOrder + sPaging;
            }
            catch (Exception exp)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, exp.Message));
                throw new System.Exception(status.Message, exp);
            }
            return(new jonStatus(Severity.Success));
        }
        public jonStatus BuildSelectQueryAdo(QueryOptions queryOptions, string sTableName, Type type, string whereSupplement, out string sSelectQuery)
        {
            jonStatus status = null;
            sSelectQuery = "SELECT * FROM " + sTableName;

            try
            {
                string sSortOrder = string.Empty;
                string sPaging = string.Empty;
                string sWhereClause = BuildWhereClauseForAdo(queryOptions, type, whereSupplement);

                //Build the sort order string
                if (queryOptions != null && queryOptions.SortColumns != null)
                {
                    sSortOrder = BuildSortOrder(queryOptions.SortColumns);
                }
                else
                {
                    sSortOrder = " ORDER BY 1 ";
                }

                //build  the paging query piece
                if (queryOptions != null && queryOptions.Paging != null)
                {
                    int nPageSize = queryOptions.Paging.PageSize;
                    int nPageNumber = queryOptions.Paging.PageNumber;

                    sPaging = $" OFFSET {nPageSize}*({nPageNumber} - 1) ROWS FETCH NEXT {nPageSize} ROWS ONLY ";
                }

                sSelectQuery = sSelectQuery + sWhereClause + sSortOrder + sPaging;
            }
            catch (Exception exp)
            {
                status = new jonStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, exp.Message));
                throw new System.Exception(status.Message, exp);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetTotalCountWHEREClause(string sSelectQuery, out string totalWHEREClause)
        {
            string sWhereClause = sSelectQuery.Substring(sSelectQuery.IndexOf(" WHERE"));
            totalWHEREClause = sWhereClause.Substring(0, sWhereClause.IndexOf("ORDER"));

            return (new jonStatus(Severity.Success));
        }

        public string BuildWhereClauseForAdo(QueryOptions queryOptions, Type type)
        {
            PropertyInfo[] dbProperties = type.GetProperties();

            if (queryOptions.SearchOptions.SearchFieldList.Count == 0)
            {
                if (string.IsNullOrEmpty(queryOptions.SearchOptions.SearchString))
                {
                    return (DbMgr.WHERE_1_EQUALS_1);
                }
                else
                {
                    return (DbMgr.WHERE_1_EQUALS_1);
                    //return (BuildSearchStringWhere(queryOptions, dbProperties));
                }
            }

            StringBuilder sbWhere = new StringBuilder();
            for (int idx = 0; idx < queryOptions.SearchOptions.SearchFieldList.Count; idx++)
            {
                SearchField searchField = queryOptions.SearchOptions.SearchFieldList[idx];

                // Get Property Type
                PropertyInfo propertyInfo = dbProperties.Where(i => i.Name == searchField.Name).FirstOrDefault();
                if (propertyInfo == null) { continue;  }


                // OR with previous field
                // TODO: AND
                if (sbWhere.Length > 0)
                {
                    if (searchField.Conjunction == Conjunction.AND)
                    {
                        sbWhere.Append(" AND ");
                    }
                    else
                    {
                        sbWhere.Append(" OR ");
                    }
                }


                String value = null;
                if (propertyInfo.PropertyType == typeof(System.String))
                {
                    value = searchField.Value;
                }
                else if (propertyInfo.PropertyType == typeof(System.DateTime))
                {
                    DateTime dateTime = DateTime.MinValue;
                    if (!DateTime.TryParse(searchField.Value, out dateTime))
                    {
                        throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
                    }
                    if (searchField.SearchOperation == SearchOperation.DateOnly)
                    {
                        value = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = dateTime.ToString();
                    }
                }
                else if (propertyInfo.PropertyType == typeof(System.Boolean))
                {
                    int oneOrZero = -1;
                    if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
                    {
                        if (oneOrZero == 0)
                        {
                            value = "0";
                        }
                        else
                        {
                            value = "1";
                        }
                    }
                    else  // Value must be 'true' or 'false' after converting to lowercase
                    {
                        if (searchField.Value.ToLower() == "false")
                        {
                            value = "0";
                        }
                        else
                        {
                            value = "1";
                        }
                    }
                }
                else
                {
                    value = searchField.Value == null ? null : searchField.Value.ToString();
                }

                // Column name starts out the WHERE phrase unless it is a DateTime
                if (propertyInfo.PropertyType != typeof(System.DateTime))
                {
                    sbWhere.Append(String.Format("{0}", searchField.Name));
                }

                // Build based on operation
                switch (searchField.SearchOperation)
                {
                    case SearchOperation.Equal:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else if (value == null)
                        {
                            sbWhere.Append(" = NULL ");
                        }
                        else if(propertyInfo.PropertyType == typeof(System.String))
                        {
                            sbWhere.Append(String.Format(" = '{0}'", value));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" = {0}", value));
                        }
                        break;
                    case SearchOperation.NotEquals:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else if (propertyInfo.PropertyType == typeof(System.String))
                        {
                            sbWhere.Append(String.Format(" = '{0}'", value));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" != {0}", value));
                        }
                        break;
                    case SearchOperation.LessThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" < {0}", value));
                        }
                        break;
                    case SearchOperation.LessThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" <= {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" > {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" >= {0}", value));
                        }
                        break;
                    case SearchOperation.Contains:
                        sbWhere.Append(String.Format(" LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotContain:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.BeginsWith:
                        sbWhere.Append(String.Format(" LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotBeginWith:
                        sbWhere.Append(String.Format(" NOT LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.EndsWith:
                        sbWhere.Append(String.Format(" LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.DoesNotEndWith:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.IsNull:
                        sbWhere.Append(String.Format(" = NULL "));
                        break;
                    case SearchOperation.IsNotNull:
                        sbWhere.Append(String.Format(" != NULL "));
                        break;
                    default:
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}",
                                Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
                }
            }

            if(sbWhere.Length > 0)
            {
                sbWhere.Insert(0, " WHERE ");
            }

            return (sbWhere.ToString());
        }
        public string BuildWhereClauseForAdo(QueryOptions queryOptions, Type type, string whereSupplement)
        {
            PropertyInfo[] dbProperties = type.GetProperties();

            if (queryOptions.SearchOptions.SearchFieldList.Count == 0)
            {
                if (string.IsNullOrEmpty(queryOptions.SearchOptions.SearchString))
                {
                    if (string.IsNullOrEmpty(whereSupplement))
                    {
                        return (DbMgr.WHERE_1_EQUALS_1);
                    }
                    else
                    {
                        return (DbMgr.WHERE_1_EQUALS_1 + whereSupplement);
                    }
                }
                else
                {
                    string _whereClause = BuildSearchStringWhere(queryOptions, dbProperties);
                    if (! string.IsNullOrEmpty(whereSupplement))
                    {
                        _whereClause += " " + whereSupplement;
                    }
                    return (_whereClause);
                }
            }

            StringBuilder sbWhere = new StringBuilder();
            for (int idx = 0; idx < queryOptions.SearchOptions.SearchFieldList.Count; idx++)
            {
                SearchField searchField = queryOptions.SearchOptions.SearchFieldList[idx];

                // Get Property Type
                PropertyInfo propertyInfo = dbProperties.Where(i => i.Name == searchField.Name).FirstOrDefault();
                if (propertyInfo == null) { continue; }


                String value = null;
                if (propertyInfo.PropertyType == typeof(System.String))
                {
                    value = searchField.Value;
                }
                else if (propertyInfo.PropertyType == typeof(System.DateTime))
                {
                    DateTime dateTime = DateTime.MinValue;
                    if (!DateTime.TryParse(searchField.Value, out dateTime))
                    {
                        throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
                    }
                    if (searchField.SearchOperation == SearchOperation.DateOnly)
                    {
                        value = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = dateTime.ToString();
                    }
                }
                else if (propertyInfo.PropertyType == typeof(System.Boolean))
                {
                    int oneOrZero = -1;
                    if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
                    {
                        if (oneOrZero == 0)
                        {
                            value = "0";
                        }
                        else
                        {
                            value = "1";
                        }
                    }
                    else  // Value must be 'true' or 'false' after converting to lowercase
                    {
                        if (searchField.Value.ToLower() == "false")
                        {
                            value = "0";
                        }
                        else if (searchField.Value.ToLower() == "true")
                        {
                            value = "1";
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    value = searchField.Value == null ? null : searchField.Value.ToString();
                }


                // OR with previous field
                // TODO: AND
                if (sbWhere.Length > 0)
                {
                    if (searchField.Conjunction == Conjunction.AND)
                    {
                        sbWhere.Append(" AND ");
                    }
                    else
                    {
                        sbWhere.Append(" OR ");
                    }
                }


                // Column name starts out the WHERE phrase unless it is a DateTime
                if (propertyInfo.PropertyType != typeof(System.DateTime))
                {
                    sbWhere.Append(String.Format("{0}", searchField.Name));
                }

                // Build based on operation
                switch (searchField.SearchOperation)
                {
                    case SearchOperation.Equal:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else if (value == null)
                        {
                            sbWhere.Append(" = NULL ");
                        }
                        else if (propertyInfo.PropertyType == typeof(System.String))
                        {
                            sbWhere.Append(String.Format(" = '{0}'", value));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" = {0}", value));
                        }
                        break;
                    case SearchOperation.NotEquals:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else if (propertyInfo.PropertyType == typeof(System.String))
                        {
                            sbWhere.Append(String.Format(" = '{0}'", value));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" != {0}", value));
                        }
                        break;
                    case SearchOperation.LessThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" < {0}", value));
                        }
                        break;
                    case SearchOperation.LessThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" <= {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThan:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" > {0}", value));
                        }
                        break;
                    case SearchOperation.GreaterThanOrEqual:
                        if (propertyInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(String.Format(" >= {0}", value));
                        }
                        break;
                    case SearchOperation.Contains:
                        sbWhere.Append(String.Format(" LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotContain:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}%' ", value));
                        break;
                    case SearchOperation.BeginsWith:
                        sbWhere.Append(String.Format(" LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.DoesNotBeginWith:
                        sbWhere.Append(String.Format(" NOT LIKE '{0}%' ", value));
                        break;
                    case SearchOperation.EndsWith:
                        sbWhere.Append(String.Format(" LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.DoesNotEndWith:
                        sbWhere.Append(String.Format(" NOT LIKE '%{0}' ", value));
                        break;
                    case SearchOperation.IsNull:
                        sbWhere.Append(String.Format(" = NULL "));
                        break;
                    case SearchOperation.IsNotNull:
                        sbWhere.Append(String.Format(" != NULL "));
                        break;
                    default:
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}",
                                Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
                }
            }
            if (!string.IsNullOrEmpty(whereSupplement))
            {
                sbWhere.Append(whereSupplement);
            }
            if (sbWhere.Length > 0)
            {
                sbWhere.Insert(0, " WHERE ");
            }
            return (sbWhere.ToString());
        }
        #endregion


        #region Build Generic Where predicate
        //private Expression<Func<T, bool>> BuildWhereClausePredicate<T>(T inputObj, SearchOptions searchOptions)
        public Expression<Func<T, bool>> BuildWherePredicate<T>(QueryOptions queryOptions, PropertyInfo[] dbProperties)
        {
            Expression<Func<T, bool>> whereClause = PredicateBuilder.New<T>(false);

            try
            {
                if(queryOptions == null || queryOptions.SearchOptions == null)
                {
                    return whereClause;
                }

                SearchOptions searchOptions = queryOptions.SearchOptions;

                whereClause = PredicateBuilder.New<T>(true);

                foreach (SearchField item in searchOptions.SearchFieldList)
                {
                    PropertyInfo pInfo = dbProperties.Single(y => y.Name == item.Name);
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var member = Expression.Property(parameter, item.Name); //x.Id
                    ConstantExpression constant = null;
                    BinaryExpression body;
                    constant = Expression.Constant(item.Value);
                    MemberExpression m = Expression.MakeMemberAccess(parameter, pInfo);
                    MethodInfo mi;
                    MethodCallExpression methodExp;

                    if (pInfo.PropertyType == typeof(System.String))
                    {
                        switch (item.SearchOperation)
                        {
                            case SearchOperation.Equal:
                                body = Expression.Equal(member, constant);
                                if (whereClause != null)
                                {
                                    whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                }
                                else
                                {
                                    whereClause = Expression.Lambda<Func<T, bool>>(body, new[] { parameter });
                                }
                                break;
                            case SearchOperation.BeginsWith:
                                mi = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
                                methodExp = Expression.Call(m, mi, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(methodExp, new[] { parameter }));
                                break;
                            case SearchOperation.Contains:
                                mi = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                                methodExp = Expression.Call(m, mi, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(methodExp, new[] { parameter }));
                                break;
                            case SearchOperation.NotEquals:
                                body = Expression.NotEqual(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            case SearchOperation.EndsWith:
                                mi = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
                                methodExp = Expression.Call(m, mi, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(methodExp, new[] { parameter }));
                                break;
                            default:
                                body = Expression.Equal(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                        }
                    }
                    else if (pInfo.PropertyType == (typeof(Int32)))
                    {
                        constant = Expression.Constant(Convert.ToInt32(item.Value));

                        switch (item.SearchOperation)
                        {
                            case SearchOperation.Equal:
                                body = Expression.Equal(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            case SearchOperation.GreaterThan:
                                body = Expression.GreaterThan(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            case SearchOperation.GreaterThanOrEqual:
                                body = Expression.GreaterThanOrEqual(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            case SearchOperation.LessThan:
                                body = Expression.LessThan(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            case SearchOperation.LessThanOrEqual:
                                body = Expression.LessThanOrEqual(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                            default:
                                body = Expression.Equal(member, constant);
                                whereClause = whereClause.And(Expression.Lambda<Func<T, bool>>(body, new[] { parameter }));
                                break;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            Console.WriteLine("Expression Tree: {0}", whereClause);
            Console.WriteLine("Expression Tree Body: {0}", whereClause.Body);
            Console.WriteLine("Expression Tree Body: {0}", whereClause.Parameters.Count);


            return whereClause;
        }
        #endregion


        #region Build Orderby predicate
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Build Orderby predicate
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public IQueryable<T> BuildOrderByPredicate<T>(IQueryable<T> source, SortColumnList sortColumnList)
        {
            if (sortColumnList == null || sortColumnList.Columns == null)
            {
                return source;
            }
            else
            {
                var type = typeof(T);
                var parameter = Expression.Parameter(type, "p");

                foreach (SortColumn column in sortColumnList.Columns)
                {
                    string command = "OrderBy";
                    var property = type.GetProperty(column.Name);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                    if (column.Direction == SortDirection.DESC)
                    {
                        command = "OrderByDescending";
                    }
                    var resultExpression = Expression.Call(typeof(Queryable), command, 
                        new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

                    source = source.Provider.CreateQuery<T>(resultExpression);
                }

                return source;
            }
        }

        #endregion


        #region Build String query using query options
        public string BuildWhereClause(QueryOptions queryOptions, PropertyInfo[] dbProperties, out Dictionary<string, object> dictParamValues)
        {
            dictParamValues = new Dictionary<string, object>();

            // NOTE: THIS IS A TEMPORARY, LIMITED SUPPORT UNTIL FULL-BLOWN DYNAMIC LINQ IS BUILT.
            if (queryOptions.SearchOptions.SearchFieldList.Count == 0)
            {
                if (string.IsNullOrEmpty(queryOptions.SearchOptions.SearchString))
                {
                    return ("1=1");
                }
                else
                {
                    return (BuildSearchStringWhere(queryOptions, dbProperties));
                }
            }

            StringBuilder sbWhere = new StringBuilder();
            int index = 0;


            foreach (SearchField searchField in queryOptions.SearchOptions.SearchFieldList)
            {
                // Get Property Type 
                PropertyInfo pInfo = dbProperties.Where(x => x.Name == searchField.Name).FirstOrDefault();

                if (sbWhere.Length > 0)
                {
                    sbWhere.Append(" AND ");
                }

                //Get the value from the input search field. 
                String value = null;
                if (pInfo.PropertyType == typeof(System.String))
                {
                    value = String.Format("\"{0}\"", searchField.Value);
                }
                else if (pInfo.PropertyType == typeof(System.DateTime))
                {
                    DateTime dateTime = DateTime.MinValue;
                    if (!DateTime.TryParse(searchField.Value, out dateTime))
                    {
                        throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
                    }
                    if (searchField.SearchOperation == SearchOperation.DateOnly)
                    {
                        value = dateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        value = dateTime.ToString();
                    }
                }
                else if (pInfo.PropertyType == typeof(System.Boolean))
                {
                    int oneOrZero = -1;
                    if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
                    {
                        if (oneOrZero == 0)
                        {
                            value = "FALSE";
                        }
                        else
                        {
                            value = "TRUE";
                        }
                    }
                    else  // Value must be 'true' or 'false' after converting to lowercase
                    {
                        if (searchField.Value.ToLower() == "false")
                        {
                            value = "FALSE";
                        }
                        else
                        {
                            value = "TRUE";
                        }
                    }
                }
                else
                {
                    value = searchField.Value == null ? null : searchField.Value.ToString();
                }

                // Column name starts out the WHERE phrase unless it is a DateTime
                if (pInfo.PropertyType != typeof(System.DateTime))
                {
                    sbWhere.Append(String.Format("{0}", searchField.Name));
                }

                bool bIsNullcheck = false;

                // Build based on operation
                switch (searchField.SearchOperation)
                {
                    case SearchOperation.Equal:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else if (value == null)
                        {
                            sbWhere.Append(" IS NULL ");
                            bIsNullcheck = true;
                        }
                        else
                        {
                            sbWhere.Append(" = @P" + index);
                        }
                        break;
                    case SearchOperation.NotEquals:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(" != @P" + index);
                        }
                        break;
                    case SearchOperation.LessThan:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(" < @P" + index);
                        }
                        break;
                    case SearchOperation.LessThanOrEqual:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(" <= @P" + index);
                        }
                        break;
                    case SearchOperation.GreaterThan:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(" > @P" + index);
                        }
                        break;
                    case SearchOperation.GreaterThanOrEqual:
                        if (pInfo.PropertyType == typeof(System.DateTime))
                        {
                            sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                        }
                        else
                        {
                            sbWhere.Append(" >= @P" + index);
                        }
                        break;
                    case SearchOperation.Contains:
                        sbWhere.Append(String.Format(" LIKE '%@P{0}%' ", index));
                        break;
                    case SearchOperation.DoesNotContain:
                        sbWhere.Append(String.Format(" NOT LIKE '%@P{0}%' ", index));
                        break;
                    case SearchOperation.BeginsWith:
                        sbWhere.Append(String.Format(" LIKE '@P{0}%' ", index));
                        break;
                    case SearchOperation.DoesNotBeginWith:
                        sbWhere.Append(String.Format(" NOT LIKE '@P{0}%' ", index));
                        break;
                    case SearchOperation.EndsWith:
                        sbWhere.Append(String.Format(" LIKE '%@P{0}' ", index));
                        break;
                    case SearchOperation.DoesNotEndWith:
                        sbWhere.Append(String.Format(" NOT LIKE '%@P{0}' ", index));
                        break;
                    case SearchOperation.IsNull:
                        sbWhere.Append(String.Format(" IS NULL "));
                        bIsNullcheck = true;
                        break;
                    case SearchOperation.IsNotNull:
                        sbWhere.Append(String.Format(" IS NOT NULL "));
                        bIsNullcheck = true;
                        break;
                    default:
                        bIsNullcheck = true;
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}",
                                Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
                }

                if (!bIsNullcheck)
                {
                    dictParamValues.Add("@P" + index, value);
                }
            }


            #region commented code
            //for (int idx = 0; idx < queryOptions.SearchOptions.SearchFieldList.Count; idx++)
            ////foreach (var item in queryOptions)
            //{
            //    SearchField searchField = queryOptions.SearchOptions.SearchFieldList[idx];


            //    // Get Property Type
            //    PropertyInfo propertyInfo = dbProperties.Where(i => i.Name == searchField.Name).FirstOrDefault();
            //    String value = null;
            //    if (propertyInfo.PropertyType == typeof(System.String))
            //    {
            //        value = String.Format("\"{0}\"", searchField.Value);
            //    }
            //    else if (propertyInfo.PropertyType == typeof(System.DateTime))
            //    {
            //        DateTime dateTime = DateTime.MinValue;
            //        if (!DateTime.TryParse(searchField.Value, out dateTime))
            //        {
            //            throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
            //        }
            //        if (searchField.SearchOperation == SearchOperation.DateOnly)
            //        {
            //            value = dateTime.ToString("yyyy-MM-dd");
            //        }
            //        else
            //        {
            //            value = dateTime.ToString();
            //        }
            //    }
            //    else if (propertyInfo.PropertyType == typeof(System.Boolean))
            //    {
            //        int oneOrZero = -1;
            //        if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
            //        {
            //            if (oneOrZero == 0)
            //            {
            //                value = "FALSE";
            //            }
            //            else
            //            {
            //                value = "TRUE";
            //            }
            //        }
            //        else  // Value must be 'true' or 'false' after converting to lowercase
            //        {
            //            if (searchField.Value.ToLower() == "false")
            //            {
            //                value = "FALSE";
            //            }
            //            else
            //            {
            //                value = "TRUE";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        value = searchField.Value == null ? null : searchField.Value.ToString();
            //    }

            //    // Column name starts out the WHERE phrase unless it is a DateTime
            //    if (propertyInfo.PropertyType != typeof(System.DateTime))
            //    {
            //        sbWhere.Append(String.Format("{0}", searchField.Name));
            //    }

            //    // Build based on operation
            //    switch (searchField.SearchOperation)
            //    {
            //        case SearchOperation.Equal:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else if (value == null)
            //            {
            //                sbWhere.Append(" = NULL ");
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" = {0}", value));
            //            }
            //            break;
            //        case SearchOperation.NotEquals:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" != {0}", value));
            //            }
            //            break;
            //        case SearchOperation.LessThan:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" < {0}", value));
            //            }
            //            break;
            //        case SearchOperation.LessThanOrEqual:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" <= {0}", value));
            //            }
            //            break;
            //        case SearchOperation.GreaterThan:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" > {0}", value));
            //            }
            //            break;
            //        case SearchOperation.GreaterThanOrEqual:
            //            if (propertyInfo.PropertyType == typeof(System.DateTime))
            //            {
            //                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
            //            }
            //            else
            //            {
            //                sbWhere.Append(String.Format(" >= {0}", value));
            //            }
            //            break;
            //        case SearchOperation.Contains:
            //            sbWhere.Append(String.Format(" LIKE '%{0}%' ", value));
            //            break;
            //        case SearchOperation.DoesNotContain:
            //            sbWhere.Append(String.Format(" NOT LIKE '%{0}%' ", value));
            //            break;
            //        case SearchOperation.BeginsWith:
            //            sbWhere.Append(String.Format(" LIKE '{0}%' ", value));
            //            break;
            //        case SearchOperation.DoesNotBeginWith:
            //            sbWhere.Append(String.Format(" NOT LIKE '{0}%' ", value));
            //            break;
            //        case SearchOperation.EndsWith:
            //            sbWhere.Append(String.Format(" LIKE '%{0}' ", value));
            //            break;
            //        case SearchOperation.DoesNotEndWith:
            //            sbWhere.Append(String.Format(" NOT LIKE '%{0}' ", value));
            //            break;
            //        case SearchOperation.IsNull:
            //            sbWhere.Append(String.Format(" = NULL "));
            //            break;
            //        case SearchOperation.IsNotNull:
            //            sbWhere.Append(String.Format(" != NULL "));
            //            break;
            //        default:
            //            throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}",
            //                    Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
            //    }
            //    if (idx + 1 < queryOptions.SearchOptions.SearchFieldList.Count)
            //    {
            //        sbWhere.Append(" AND ");
            //    }
            //}

            #endregion
            return (sbWhere.ToString());
        }

        #endregion


        #region Build the select query with Input object and queryoptions

        public string BuildSelectQuery<T>(T inputObj, QueryOptions queryOptions, out List<string> queryParams)
        {
            StringBuilder sbQuery = new StringBuilder();
            string sWhereClause = string.Empty;
            string sSortOrder = string.Empty;
            string sPaging = string.Empty;
            queryParams = new List<string>();

            try
            {
                sbQuery.AppendFormat("SELECT * FROM {0} ", inputObj.GetType().Name);
                PropertyInfo[] properties = inputObj.GetType().GetProperties();

                //If no input query options then return the whole data.
                if (queryOptions != null && queryOptions.SearchOptions != null &&
                    queryOptions.SearchOptions.SearchFieldList != null &&
                    queryOptions.SearchOptions.SearchFieldList.Count > 0)
                {
                    sWhereClause = BuildWhereClause(queryOptions, properties, out queryParams);
                }

                //Get the sort order for the query
                if (queryOptions != null && queryOptions.SortColumns != null &&
                    queryOptions.SortColumns.Columns != null && queryOptions.SortColumns.Columns.Any())
                {
                    sSortOrder = BuildSortOrder(queryOptions.SortColumns);
                }
                else
                {
                    sSortOrder = " ORDER BY 1 ";
                }

                //Get the paging query piece
                if (queryOptions != null && queryOptions.Paging != null)
                {
                    int nPageSize = queryOptions.Paging.PageSize;
                    int nPageNumber = queryOptions.Paging.PageNumber;

                    sPaging = $" OFFSET {nPageSize}*({nPageNumber} - 1) ROWS FETCH NEXT {nPageSize} ROWS ONLY ";
                }

                //Put where, sort and paging together.
                sbQuery.Append(sWhereClause);
                sbQuery.Append(sSortOrder);
                sbQuery.Append(sPaging);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                throw exp;
            }

            return sbQuery.ToString();
        }
        public string BuildSortOrder(SortColumnList sortColumns)
        {
            StringBuilder sbSortOrder = new StringBuilder();

            try
            {
                if (sortColumns == null || sortColumns.Columns == null || !sortColumns.Columns.Any())
                {
                    return " ORDER BY 1 ";
                }

                foreach (SortColumn sortField in sortColumns.Columns)
                {
                    sbSortOrder.AppendFormat(" {2}{0} {1}", sortField.Name, sortField.Direction,
                        (sbSortOrder.Length > 0) ? "," : string.Empty);
                }

                sbSortOrder.Insert(0, " ORDER BY ");
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return sbSortOrder.ToString();
        }
        public string BuildWhereClause(QueryOptions queryOptions, PropertyInfo[] dbProperties, out List<string> lstParameters)
        {
            StringBuilder sbWhere = new StringBuilder();
            lstParameters = new List<string>();
            int index = 0;

            try
            {
                if (queryOptions == null || queryOptions.SearchOptions == null ||
                    queryOptions.SearchOptions.SearchFieldList == null || !queryOptions.SearchOptions.SearchFieldList.Any())
                {
                    return sbWhere.ToString();
                }

                foreach (SearchField searchField in queryOptions.SearchOptions.SearchFieldList)
                {
                    // Get Property Type 
                    PropertyInfo pInfo = dbProperties.Where(x => x.Name == searchField.Name).FirstOrDefault();

                    if (sbWhere.Length > 0)
                    {
                        sbWhere.Append(" AND ");
                    }

                    //Get the value from the input search field. 
                    String value = null;
                    if (pInfo.PropertyType == typeof(System.String))
                    {
                        value = String.Format("{0}", searchField.Value);
                    }
                    else if (pInfo.PropertyType == typeof(System.DateTime))
                    {
                        DateTime dateTime = DateTime.MinValue;
                        if (!DateTime.TryParse(searchField.Value, out dateTime))
                        {
                            throw new Exception(String.Format("Invalid DateTime value: {0}", searchField.Value));
                        }
                        if (searchField.SearchOperation == SearchOperation.DateOnly)
                        {
                            value = dateTime.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            value = dateTime.ToString();
                        }
                    }
                    else if (pInfo.PropertyType == typeof(System.Boolean))
                    {
                        int oneOrZero = -1;
                        if (int.TryParse(searchField.Value, out oneOrZero))  // Assuming value is 1 or 0
                        {
                            if (oneOrZero == 0)
                            {
                                value = "FALSE";
                            }
                            else
                            {
                                value = "TRUE";
                            }
                        }
                        else  // Value must be 'true' or 'false' after converting to lowercase
                        {
                            if (searchField.Value.ToLower() == "false")
                            {
                                value = "FALSE";
                            }
                            else
                            {
                                value = "TRUE";
                            }
                        }
                    }
                    else
                    {
                        value = searchField.Value == null ? null : searchField.Value.ToString();
                    }

                    // Column name starts out the WHERE phrase unless it is a DateTime
                    if (pInfo.PropertyType != typeof(System.DateTime))
                    {
                        sbWhere.Append(String.Format("{0}", searchField.Name));
                    }

                    bool bIsNullcheck = false;

                    // Build based on operation
                    switch (searchField.SearchOperation)
                    {
                        case SearchOperation.Equal:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else if (value == null)
                            {
                                sbWhere.Append(" IS NULL ");
                                bIsNullcheck = true;
                            }
                            else
                            {
                                sbWhere.Append(" = {" + index + "}");
                            }
                            break;
                        case SearchOperation.NotEquals:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else
                            {
                                sbWhere.Append(" != {" + index + "}");
                            }
                            break;
                        case SearchOperation.LessThan:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else
                            {
                                sbWhere.Append(" < {" + index + "}");
                            }
                            break;
                        case SearchOperation.LessThanOrEqual:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else
                            {
                                sbWhere.Append(" <= {" + index + "}");
                            }
                            break;
                        case SearchOperation.GreaterThan:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else
                            {
                                sbWhere.Append(" > {" + index + "}");
                            }
                            break;
                        case SearchOperation.GreaterThanOrEqual:
                            if (pInfo.PropertyType == typeof(System.DateTime))
                            {
                                sbWhere.Append(dateOperatorSubClause(searchField.Name, value, searchField.SearchOperation));
                            }
                            else
                            {
                                sbWhere.Append(" >= {" + index + "}");
                            }
                            break;
                        case SearchOperation.Contains:
                            value = "%" + value + "%";
                            sbWhere.Append(" LIKE {" + index + "}");
                            break;
                        case SearchOperation.DoesNotContain:
                            value = "%" + value + "%";
                            sbWhere.Append(" NOT LIKE {" + index + "}");
                            break;
                        case SearchOperation.BeginsWith:
                            value = "%" + value;
                            sbWhere.Append(" LIKE {" + index + "}");
                            break;
                        case SearchOperation.DoesNotBeginWith:
                            value = "%" + value;
                            sbWhere.Append(" NOT LIKE {" + index + "}");
                            break;
                        case SearchOperation.EndsWith:
                            value = value + "%";
                            sbWhere.Append(" LIKE {" + index + "}");
                            break;
                        case SearchOperation.DoesNotEndWith:
                            value = value + "%";
                            sbWhere.Append(" NOT LIKE {" + index + "}");
                            break;
                        case SearchOperation.IsNull:
                            sbWhere.Append(String.Format(" IS NULL "));
                            bIsNullcheck = true;
                            break;
                        case SearchOperation.IsNotNull:
                            sbWhere.Append(String.Format(" IS NOT NULL "));
                            bIsNullcheck = true;
                            break;
                        default:
                            bIsNullcheck = true;
                            throw (new System.Exception(String.Format("EXCEPTION: DbMgr.BuildWhereClause: Invalid Search Operator: {0}",
                                    Enum.GetName(typeof(SearchOperation), searchField.SearchOperation))));
                    }

                    if (!bIsNullcheck)
                    {
                        lstParameters.Add(value);
                    }

                    index++;
                }

                if (sbWhere.Length > 0)
                {
                    sbWhere.Insert(0, " WHERE ");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                throw exp;
            }

            return (sbWhere.ToString());
        }
        #endregion


        #region SqlParameters
        /*----------------------------------------------------------------------------------------------------------------------------------
         * SqlParameters
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public jonStatus BuildIntegerArrayTableType(string parameterName, List<int> intList, out SqlParameter sqlParameter)
        {
            // Initialize
            sqlParameter = null;


            // Build type
            DataTable dtWellIds = new DataTable();
            dtWellIds.Columns.Add("n", typeof(int));
            foreach (int Id in intList)
            {
                dtWellIds.Rows.Add(Id);
            }

            // Build parameter
            sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured);
            sqlParameter.TypeName = "dbo.IntegerArrayTableType";
            sqlParameter.Value = dtWellIds;

            return (new jonStatus(Severity.Success));
        }
        #endregion


        #region SqlDataReader
        /*----------------------------------------------------------------------------------------------------------------------------------
         * SqlDataReader
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public jonStatus TransferSQLReader(SqlDataReader sqlDataReader, object destination, bool bIgnoreExceptions = false)
        {
            PropertyInfo[] piDestination = destination.GetType().GetProperties();
            foreach (PropertyInfo piD in piDestination)
            {
                // If Data Reader doesn't have the colunn, ignore it and continue
                try
                {
                    int idx = sqlDataReader.GetOrdinal(piD.Name);
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }

                // If null value, skip it.
                if (sqlDataReader[piD.Name] == DBNull.Value)
                {
                    continue;
                }

                    // Get column 
                if (piD.PropertyType == typeof(System.String))
                {
                    string _stringValue = sqlDataReader[piD.Name] == DBNull.Value ? null : sqlDataReader[piD.Name].ToString();
                    piD.SetValue(destination, _stringValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int16))
                {
                    short _shortValue = Convert.ToInt16(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _shortValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int32))
                {
                    int _intValue = Convert.ToInt32(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _intValue, null);
                }
                else if (piD.PropertyType == typeof(System.Int64))
                {
                    long _longValue = Convert.ToInt64(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _longValue, null);
                }
                else if (piD.PropertyType == typeof(System.DateTime))
                {
                    DateTime _dateTimeValue = Convert.ToDateTime(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _dateTimeValue, null);
                }
                else if (piD.PropertyType == typeof(System.Decimal))
                {
                    Decimal _decimalValue = Convert.ToDecimal(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _decimalValue, null);
                }
                else if (piD.PropertyType == typeof(System.Boolean))
                {
                    bool _boolValue = Convert.ToBoolean(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _boolValue, null);
                }
                else if (piD.PropertyType == typeof(System.Char))
                {
                    char _charValue = Convert.ToChar(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _charValue, null);
                }
                else if (piD.PropertyType == typeof(System.Byte))
                {
                    byte _byteValue = Convert.ToByte(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _byteValue, null);
                }
                else if (piD.PropertyType == typeof(System.Double))
                {
                    double _doubleValue = Convert.ToDouble(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _doubleValue, null);
                }
                else if (piD.PropertyType == typeof(System.Guid))
                {
                    Guid _guidValue = new Guid(sqlDataReader[piD.Name].ToString());
                    piD.SetValue(destination, _guidValue, null);
                }
                else if (piD.PropertyType == typeof(System.SByte))
                {
                    SByte _sbyteValue = Convert.ToSByte(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _sbyteValue, null);
                }
                else if (piD.PropertyType == typeof(System.Single))
                {
                    Single _singleValue = Convert.ToSingle(sqlDataReader[piD.Name]);
                    piD.SetValue(destination, _singleValue, null);
                }
                else if (piD.PropertyType.Name == "Nullable`1")
                {
                    if (piD.PropertyType == typeof(System.Int16))
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            short _shortValue = Convert.ToInt16(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _shortValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Int32")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            int _intValue = Convert.ToInt32(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _intValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Int64")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            long _longValue = Convert.ToInt64(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _longValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.DateTime")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            DateTime _dateTimeValue = Convert.ToDateTime(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _dateTimeValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Decimal")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Decimal _decimalValue = Convert.ToDecimal(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _decimalValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Boolean")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            bool _boolValue = Convert.ToBoolean(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _boolValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Char")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            char _charValue = Convert.ToChar(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _charValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Byte")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            byte _byteValue = Convert.ToByte(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _byteValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Double")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            double _doubleValue = Convert.ToDouble(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _doubleValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Guid")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Guid _guidValue = new Guid(sqlDataReader[piD.Name].ToString());
                            piD.SetValue(destination, _guidValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.SByte")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            SByte _sbyteValue = Convert.ToSByte(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _sbyteValue, null);
                        }
                    }
                    else if (piD.PropertyType.GenericTypeArguments[0].FullName == "System.Single")
                    {
                        if (sqlDataReader[piD.Name] != DBNull.Value)
                        {
                            Single _singleValue = Convert.ToSingle(sqlDataReader[piD.Name]);
                            piD.SetValue(destination, _singleValue, null);
                        }
                    }
                }
                else
                {
                    if (!bIgnoreExceptions)
                    {
                        throw new System.Exception(String.Format("EXCEPTION: {0}.{1} Unexpected property type: ({2})",
                                 this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name,
                                 piD.PropertyType.Name));
                    }
                }
            }
            return (new jonStatus(Severity.Success));
        }
        #endregion


        #region ErrorMessages
        /*----------------------------------------------------------------------------------------------------------------------------------
         * ErrorMessages
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public string GetErrorMessage(Exception ex)
        {
            string errorMessage = getInnerMostErrorMessage(ex);
            return(errorMessage);
        }
        #endregion


        #region ValidateRequiredFields
        /*----------------------------------------------------------------------------------------------------------------------------------
         * ValidateRequiredFields
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public jonStatus TrimAndValidate(Object source, List<string> requiredFieldList)
        {
            // Initialize.
            jonStatus status = null;


            // Trim fields.
            try
            {
                BufferMgr.TrimRequiredFields(source);
            }
            catch (Exception ex)
            {
                status = new jonStatus(Severity.Error, ex.Message);
                return (status);
            }


            // Validate required fields.
            status = this.ValidateRequiredFields(source, requiredFieldList);
            if (!jonStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus ValidateRequiredFields(Object source, List<string> requiredFieldList)
        {
            // initialize
            jonStatus status;
            List<string> missingRequiredFieldList = new List<string>();
            string className = source.GetType().Name;

            foreach (string fieldName in requiredFieldList)
            {
                PropertyInfo piS = source.GetType().GetProperty(fieldName);
                if (piS == null)
                {
                    // Can't find the property - developer oversight. Changed a property lately?
                    string errorMessage = String.Format("InvalidRequiredDataMember: {0}.{1}", className, fieldName);
                    status = new jonStatus(Severity.Error, errorMessage);
                    return status;
                }
                else
                {
                    string fieldType = piS.PropertyType.FullName;
                    switch (fieldType)
                    {
                        case "System.String":
                            string sValue = (string)piS.GetValue(source, null);
                            if (sValue == null)
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            else if (string.IsNullOrEmpty(sValue.Trim()))
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            break;
                        case "System.Int32":
                            int iValue = (int)piS.GetValue(source, null);
                            if (iValue < BaseId.VALID_ID)
                            {
                                missingRequiredFieldList.Add(fieldName);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (missingRequiredFieldList.Count > 0)
            {
                string errorMessage = String.Format("MissingRequiredDataMember: {0}|{1}", className, String.Join(",", missingRequiredFieldList.ToArray<string>()));
                status = new jonStatus(Severity.Error, errorMessage);
                return status;
            }
            return (new jonStatus(Severity.Success));
        }
        #endregion


        #region Column Metadata
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Column Metadata
         *---------------------------------------------------------------------------------------------------------------------------------*/
        #endregion


        #region Utility Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Utility Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public jonStatus GetRow(SqlDataReader rdr, out dynamic resultRow)
        {
            // Initialize
            jonStatus status = null;
            resultRow = new ExpandoObject();


            // Get row data.
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(i)] = rdr[rdr.GetName(i)].ToString();

                status = GetColumnData(rdr, i, resultRow);
                if (!jonStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetColumnData(SqlDataReader rdr, int idx, dynamic resultRow)
        {
            if (rdr[rdr.GetName(idx)] == DBNull.Value)
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = null;
                return (new jonStatus(Severity.Success));
            }

            Type _type = rdr.GetFieldType(idx);
            if (_type == typeof(int))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt32(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(string))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = rdr[rdr.GetName(idx)].ToString();
            }
            else if (_type == typeof(bool))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToBoolean(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(DateTime))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDateTime(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(byte))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToByte(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(sbyte))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToByte(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(char))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToChar(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(decimal))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDecimal(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(double))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToDouble(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(float))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToSingle(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(short))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt16(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(long))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Convert.ToInt64(rdr[rdr.GetName(idx)]);
            }
            else if (_type == typeof(Guid))
            {
                ((IDictionary<String, Object>)resultRow)[rdr.GetName(idx)] = Guid.Parse(rdr[rdr.GetName(idx)].ToString());
            }
            else
            {
                return (new jonStatus(Severity.Error, String.Format("ERROR: Unknown column type: {0}", _type.ToString())));
            }
            return (new jonStatus(Severity.Success));
        }
        public jonStatus GetCaller(Type askingType, out jonStatus Status)
        {
            // Initialize
            Status = null;
            string module = null;
            string method = null;
            string message = null;
            StackTrace stackTrace = new StackTrace();
            int frameIndex = 0;
            const int MAX_FRAMES = 20;


            // Get caller of given askingType
            for (frameIndex = 0; frameIndex < MAX_FRAMES; frameIndex++)
            {
                MethodBase methodBase = stackTrace.GetFrame(frameIndex).GetMethod();
                if (methodBase.DeclaringType == null) { continue; }
                if (askingType.FullName == methodBase.DeclaringType.FullName)
                {
                    module = methodBase.DeclaringType.FullName;
                    method = MethodSignature.GetMethodSignature(methodBase);
                    break;
                }
            }
            if (module == null)
            {
                return (new jonStatus(Severity.Error, String.Format("ERROR: DbMgr.GetCaller: Calling type not found above askingType: {0}", askingType.ToString())));
            }
            Status = new jonStatus(Severity.Fatal, module, method, message);
            return (new jonStatus(Severity.Success));
        }
        public string GetFullStackTrace(Type askingType, string StopAtNamespace)
        {
            // Initialize
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrameArray = stackTrace.GetFrames();


            // Locate asking frame on stack, then start printing stack frame
            bool bAskingFrameLocated = false;
            StringBuilder sbFullStackTrace = new StringBuilder();
            foreach (StackFrame stackFrame in stackFrameArray)
            {
                MethodBase methodBase = stackFrame.GetMethod();
                if (methodBase.DeclaringType == null) { continue; }
                if (!bAskingFrameLocated)
                {
                    bAskingFrameLocated = askingType.FullName == methodBase.DeclaringType.FullName;
                    if (!bAskingFrameLocated)
                    {
                        continue;
                    }
                }
                if (StopAtNamespace != null)
                {
                    if (StopAtNamespace == methodBase.DeclaringType.Namespace)
                    {
                        break;
                    }
                }
                string module = methodBase.DeclaringType.FullName;
                string fileName = stackFrame.GetFileName();
                string method = MethodSignature.GetMethodSignature(stackFrame.GetMethod());
                int lineNumber = stackFrame.GetFileLineNumber();

                sbFullStackTrace.Append(" at " + module + "." + method);
                if (!string.IsNullOrEmpty(fileName))
                {
                    sbFullStackTrace.Append(" in " + fileName);
                }
                if (lineNumber > 0)
                {
                    sbFullStackTrace.Append(" line " + lineNumber.ToString());
                }
                sbFullStackTrace.AppendLine();
            }
            return (sbFullStackTrace.ToString());
        }
        #endregion


        #region Files
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Files
         *---------------------------------------------------------------------------------------------------------------------------------*/
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
                if(string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = this._connectionString = ConnectionMgr.DataManagerConnectionString;
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
        private string dateOperatorSubClause(string columnName, string Date, SearchOperation searchOperation)
        {
            // Initialize
            string subClause = null;


            // Validate date
            DateTime _date = DateTime.MinValue;
            if (!DateTime.TryParse(Date, out _date))
            {
                throw (new System.Exception(String.Format("EXCEPTION: DbMgr.exactDateSubClause: Invalid Date: {0}", Date)));
            }

            // Determine operator
            string comparisonOperator = null;
            DateTime _endDate = DateTime.MinValue;
            switch (searchOperation)
            {
                case SearchOperation.Equal:
                    DateTime _startDate = DateTime.MinValue;
                    if (!DateTime.TryParse(Date, out _startDate))
                    {
                        throw (new System.Exception(String.Format("EXCEPTION: DbMgr.exactDateSubClause: Invalid Date: {0}", Date)));
                    }
                    _endDate = _startDate.AddDays(1);

                    subClause = String.Format("( {0} >= DateTime({1}, {2}, {3}) AND {0} < DateTime({4}, {5}, {6}) )", columnName,
                            _startDate.Year, _startDate.Month, _startDate.Day, _endDate.Year, _endDate.Month, _endDate.Day);
                    break;
                case SearchOperation.NotEquals:
                    _endDate = _date.AddDays(1);
                    subClause = String.Format("( {0} < DateTime({1}, {2}, {3}) OR {0} >= DateTime({4}, {5}, {6}) )", columnName,
                            _date.Year, _date.Month, _date.Day, _endDate.Year, _endDate.Month, _endDate.Day);
                    break;
                case SearchOperation.LessThan:
                    comparisonOperator = " < ";
                    break;
                case SearchOperation.LessThanOrEqual:
                    comparisonOperator = " < ";  // Less than the next day.
                    _date = _date.AddDays(1);
                    break;
                case SearchOperation.GreaterThan:
                    comparisonOperator = " > ";
                    _date = _date.AddDays(1);
                    break;
                case SearchOperation.GreaterThanOrEqual:
                    comparisonOperator = " >= ";
                    break;
                default:
                    throw (new System.Exception(String.Format("EXCEPTION: DbMgr.dateOperatorSubClause2: Invalid operator: {0}", Enum.GetName(typeof(SearchOperation), searchOperation))));
            }

            // Build subclause
            if (subClause == null)
            {
                subClause = String.Format("( {0} {1} DateTime({2}, {3}, {4}) )", columnName, comparisonOperator,
                        _date.Year, _date.Month, _date.Day);
            }
            return (subClause);
        }
        private string getInnerMostErrorMessage(Exception ex)
        {
            Exception realerror = ex;
            while (realerror.InnerException != null)
            {
                realerror = realerror.InnerException;
            }
            return (realerror.Message.ToString());
        }
        private jonStatus parseEntryMask(string TSQL, out Dictionary<string, bool> optionalParameters)
        {
            // Initialize
            optionalParameters = new Dictionary<string, bool>();

            // Going to need an ANTLR grammar to do this right.
            // See this for HOW-TO and issues involved: https://github.com/antlr/grammars-v4/tree/master/tsql


            return (new jonStatus(Severity.Success));
        }
        #endregion
    }
}
