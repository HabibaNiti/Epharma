using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MedicineEshop.ViewModel;

namespace MedicineEshop.DAL
{
    public class AuthenticationDAL
    {
        OracleTransaction _trans;

        #region "Oracle Connection Check"

        private OracleConnection GetConnection()
        {
            var conString = ConfigurationManager.ConnectionStrings["OracleDbContext"];
            string strConnString = conString.ConnectionString;
            return new OracleConnection(strConnString);
        }

        #endregion

        public async Task<AuthModel> Login(string employeeId, string employeePassword)
        {
            AuthModel model = new AuthModel();
            OracleCommand objOracleCommand = new OracleCommand("pro_check_valid_user")
            {
                CommandType = CommandType.StoredProcedure
            };


            objOracleCommand.Parameters.Add("p_employee_id", OracleDbType.Varchar2, ParameterDirection.InputOutput)
                .Value = !string.IsNullOrWhiteSpace(employeeId) ? employeeId : null;
            objOracleCommand.Parameters.Add("p_employee_password", OracleDbType.Varchar2, ParameterDirection.Input)
                .Value = !string.IsNullOrWhiteSpace(employeePassword) ? employeePassword : null;

            objOracleCommand.Parameters.Add("p_employee_name", OracleDbType.Varchar2, 2000).Direction =
                ParameterDirection.Output;
            objOracleCommand.Parameters.Add("p_employee_email", OracleDbType.Varchar2, 2000).Direction =
                ParameterDirection.Output;
            objOracleCommand.Parameters.Add("p_employee_role", OracleDbType.Varchar2, 2000).Direction =
                ParameterDirection.Output;

            objOracleCommand.Parameters.Add("p_message", OracleDbType.Varchar2, 500).Direction =
                ParameterDirection.Output;

            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objOracleCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    _trans = strConn.BeginTransaction();
                    await objOracleCommand.ExecuteNonQueryAsync();
                    _trans.Commit();
                    strConn.Close();

                    model.EmployeeId = objOracleCommand.Parameters["p_employee_id"].Value.ToString();
                    model.EmployeeName = objOracleCommand.Parameters["p_employee_name"].Value.ToString();
                    model.EmployeeEmail = objOracleCommand.Parameters["p_employee_email"].Value.ToString();
                    model.EmployeeRole = objOracleCommand.Parameters["p_employee_role"].Value.ToString();

                    var strMsg = objOracleCommand.Parameters["p_message"].Value.ToString();

                    model.Message = strMsg == "TRUE";
                }

                catch (Exception ex)
                {
                    throw new Exception("Error : " + ex.Message);
                }

                finally
                {

                    strConn.Close();
                }

            }

            return model;
        }
        

        public async Task<string> LogAction(LogActionModel objLogActionModel, AuthModel objAuthModel)
        {
            string strMsg;
            OracleCommand objOracleCommand = new OracleCommand("PRO_USER_ACTION_HIST_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };
            objOracleCommand.Parameters.Add("p_user_id", OracleDbType.Varchar2, ParameterDirection.InputOutput).Value =
                !string.IsNullOrWhiteSpace(objAuthModel.EmployeeId) ? objAuthModel.EmployeeId : null;
            objOracleCommand.Parameters.Add("p_user_role", OracleDbType.Varchar2, ParameterDirection.InputOutput).Value
                = !string.IsNullOrWhiteSpace(objAuthModel.EmployeeRole) ? objAuthModel.EmployeeRole : null;
            objOracleCommand.Parameters.Add("p_user_Name", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                !string.IsNullOrWhiteSpace(objAuthModel.EmployeeName) ? objAuthModel.EmployeeName : null;
            objOracleCommand.Parameters.Add("p_controller_name", OracleDbType.Varchar2, ParameterDirection.Input).Value
                = !string.IsNullOrWhiteSpace(objLogActionModel.ControllerName)
                    ? objLogActionModel.ControllerName
                    : "unknown";
            objOracleCommand.Parameters.Add("p_action_name", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                !string.IsNullOrWhiteSpace(objLogActionModel.ActionName) ? objLogActionModel.ActionName : "unknown";
            objOracleCommand.Parameters.Add("p_menu_url", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                !string.IsNullOrWhiteSpace(objLogActionModel.MenuUrl) ? objLogActionModel.MenuUrl : "";
            objOracleCommand.Parameters.Add("p_ip_address", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                !string.IsNullOrWhiteSpace(objLogActionModel.IpAddress) ? objLogActionModel.IpAddress : "0";
            objOracleCommand.Parameters.Add("P_MESSAGE", OracleDbType.Varchar2, 500).Direction =
                ParameterDirection.Output;

            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objOracleCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    _trans = strConn.BeginTransaction();
                    await objOracleCommand.ExecuteNonQueryAsync();
                    _trans.Commit();
                    strConn.Close();

                    strMsg = objOracleCommand.Parameters["P_MESSAGE"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error : " + ex.Message);
                }
                finally
                {
                    strConn.Close();
                }
            }

            return strMsg;
        }

        public async Task<string> RoleWiseActionPermision(string controllerAction,string userRole)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_MENU_PERMISION_CHECK")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("p_Menu_URL", OracleDbType.Varchar2, ParameterDirection.Input).Value =controllerAction;
            objOracleCommand.Parameters.Add("p_User_Role", OracleDbType.Varchar2, ParameterDirection.Input).Value =userRole;
            objOracleCommand.Parameters.Add("P_MESSAGE", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;

            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objOracleCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    _trans = strConn.BeginTransaction();
                    await objOracleCommand.ExecuteNonQueryAsync();
                    _trans.Commit();
                    strConn.Close();

                    strMessage = objOracleCommand.Parameters["P_MESSAGE"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error : " + ex.Message);
                }
                finally
                {
                    strConn.Close();
                    strConn.Dispose();
                    objOracleCommand.Dispose();
                }
            }
            return strMessage;
        }

        #region UserSelfRegistration
        public  string GetSystemGeneratedUserId()
        {
            var sql = "SELECT " +
                      "LPAD (NVL (MAX (SUBSTR ( (EMPLOYEE_ID), 0, 5)), 0) + 1,5,0) EMPLOYEE_ID  " +
                         "FROM ADMIN_LOGIN ";
            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection))
                {

                    objConnection.Open();
                    using (OracleDataReader objDataReader = (OracleDataReader) objCommand.ExecuteReader())
                    {
                        string userId = "";
                        try
                        {
                            while (objDataReader.Read())
                            {
                                userId = objDataReader["EMPLOYEE_ID"].ToString();
                            }

                            return userId;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error : " + ex.Message);
                        }
                        finally
                        {
                            objDataReader.Dispose();
                            objCommand.Dispose();
                            objConnection.Dispose();
                        }
                    }
                }
            }
        }

        public async Task<string> UserRegistrationSave(AuthModel authModel)
        {
            string strMessage;
            OracleCommand objOracleCommand = new OracleCommand("PRO_USER_SELF_REGISTRATION")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_EMPLOYEE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.EmployeeId) ? authModel.EmployeeId : null;
            objOracleCommand.Parameters.Add("P_EMPLOYEE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.EmployeeName) ? authModel.EmployeeName.Trim() : null;
            objOracleCommand.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.NewPassword) ? authModel.NewPassword : null;
            objOracleCommand.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.EmployeeEmail) ? authModel.EmployeeEmail : null;
            objOracleCommand.Parameters.Add("P_CONTACT_NO", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.EmployeePhone) ? authModel.EmployeePhone : null;
            objOracleCommand.Parameters.Add("P_EMPLOYEE_ADDRESS", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(authModel.EmployeeArea) ? authModel.EmployeeArea : null;

            objOracleCommand.Parameters.Add("P_MESSAGE", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;

            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objOracleCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    _trans = strConn.BeginTransaction();
                    await objOracleCommand.ExecuteNonQueryAsync();
                    _trans.Commit();
                    strConn.Close();

                    strMessage = objOracleCommand.Parameters["P_MESSAGE"].Value.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error : " + ex.Message);
                }
                finally
                {
                    strConn.Close();
                    strConn.Dispose();
                    objOracleCommand.Dispose();
                }
            }
            return strMessage;
        }
        #endregion

        #region UserList
        public async Task<List<EmployeeDistributionModel>> GetUserList()
        {


            var sql = "SELECT " +
                      "EMPLOYEE_ID," +
                      "EMPLOYEE_NAME," +
                      "EMPLOYEE_EMAIL," +
                      "EMPLOYEE_ROLE," +
                      "ACTIVE_YN," +
                      "CREATED_BY," +
                      "CREATED_DATE," +
                      "UPDATE_BY," +
                      "UPDATE_DATE " +
                      "FROM ADMIN_LOGIN ORDER BY UPDATE_DATE DESC";



            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<EmployeeDistributionModel> objUserListModels = new List<EmployeeDistributionModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                EmployeeDistributionModel model = new EmployeeDistributionModel
                                {
                                    EmployeeId = objDataReader["EMPLOYEE_ID"].ToString(),
                                    EmployeeName = objDataReader["EMPLOYEE_NAME"].ToString(),
                                    Email = objDataReader["EMPLOYEE_EMAIL"].ToString(),
                                    EmployeeRole = objDataReader["EMPLOYEE_ROLE"].ToString(),
                                    ActiveYn = objDataReader["ACTIVE_YN"].ToString(),
                                    UpdateBy = objDataReader["UPDATE_BY"].ToString(),
                                    UpdateDate = objDataReader["UPDATE_DATE"].ToString(),
                                    CreateBy = objDataReader["CREATED_BY"].ToString(),
                                    CreateDate = objDataReader["CREATED_DATE"].ToString()
                                };
                                objUserListModels.Add(model);
                            }
                            return objUserListModels;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error : " + ex.Message);
                        }
                        finally
                        {
                            objDataReader.Dispose();
                            objCommand.Dispose();
                            objConnection.Dispose();
                        }
                    }


                }
            }
        }
        #endregion
      
        

    }
}