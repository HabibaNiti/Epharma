using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using MedicineEshop.ViewModel;

namespace MedicineEshop.DAL
{
    public class DropdownDAL
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

        //For All Dropdown Start

        public async Task<DataTable> GetUserRoleListDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT DISTINCT " +
                      "EMPLOYEE_ROLE, " +
                      "EMPLOYEE_ROLE  " +
                      "FROM ADMIN_LOGIN ";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }

        public async Task<DataTable> GetUserMenuListDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT " +
                      "MENU_ID, " +
                      "MENU_NAME  " +
                      "FROM MENU_MAIN ";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }

        public async Task<DataTable> GetUserIdDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT " +
                      "EMPLOYEE_ID, " +
                      "EMPLOYEE_ID  " +
                      "FROM ADMIN_LOGIN ";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }

        public async Task<DataTable> GetLogedInEmployeeDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT EMPLOYEE_ID AS VALUE ,EMPLOYEE_ID ||'--'|| EMPLOYEE_NAME || ' (' || EMPLOYEE_ROLE || ')' AS TEXT  FROM ADMIN_LOGIN";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }

        public async Task<DataTable> GetCompanyListDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT " +
                      "COMPANY_ID, " +
                      "COMPANY_NAME  " +
                      "FROM L_COMPANY WHERE ACTIVE_YN = 'Y' ";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }

        public async Task<DataTable> GetMedicineTypeListDropdown()
        {
            DataTable dt = new DataTable();
            var sql = "SELECT " +
                      "MEDICINE_TYPE_ID, " +
                      "MEDICINE_TYPE_NAME  " +
                      "FROM L_MEDICINE_TYPE WHERE ACTIVE_YN = 'Y' ";

            OracleCommand objCommand = new OracleCommand(sql);
            OracleDataAdapter objDataAdapter = new OracleDataAdapter(objCommand);
            using (OracleConnection strConn = GetConnection())
            {
                try
                {
                    objCommand.Connection = strConn;
                    await strConn.OpenAsync();
                    objDataAdapter.Fill(dt);
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
            return dt;
        }
        //End

    }
}