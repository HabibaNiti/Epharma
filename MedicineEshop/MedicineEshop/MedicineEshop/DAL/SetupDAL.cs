using Oracle.ManagedDataAccess.Client;
using MedicineEshop.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MedicineEshop.DAL
{
    public class SetupDAL
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

        #region CompanaySetup
        public async Task<List<CompanySetupModel>> GetAllCompanyList()
        {
            var sql = "SELECT COMPANY_ID,COMPANY_NAME,ADDRESS,CONTACT_NO,EMAIL,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_COMPANY_LIST";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using(OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<CompanySetupModel> objCompanyModels = new List<CompanySetupModel>();
                        try
                        {
                            while(await objDataReader.ReadAsync())
                            {
                                CompanySetupModel model = new CompanySetupModel
                                {
                                    CompanyId = Convert.ToInt32(objDataReader["COMPANY_ID"].ToString()),
                                    ComapnayName = objDataReader["COMPANY_NAME"].ToString(),
                                    Address = objDataReader["ADDRESS"].ToString(),
                                    ContactNo = objDataReader["CONTACT_NO"].ToString(),
                                    ComapanyEmail = objDataReader["EMAIL"].ToString(),
                                    ActiveYN = objDataReader["ACTIVE_YN"].ToString(),
                                    CreateBy = objDataReader["CREATE_BY"].ToString(),
                                    CreateDate = objDataReader["CREATE_DATE"].ToString(),
                                    UpdateBy = objDataReader["UPDATE_BY"].ToString(),
                                    UpdateDate = objDataReader["UPDATE_DATE"].ToString()
                                };
                                objCompanyModels.Add(model);
                            }
                            return objCompanyModels;
                        }
                        catch(Exception ex)
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

        public async Task<CompanySetupModel> GetACompany(int companyId)
        {
            var sql = "SELECT COMPANY_ID,COMPANY_NAME,ADDRESS,CONTACT_NO,EMAIL,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_COMPANY_LIST " +
                " WHERE COMPANY_ID = :COMPANY_ID ";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":COMPANY_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = companyId;
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        CompanySetupModel objCompanyModel = new CompanySetupModel();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {

                                objCompanyModel.CompanyId = Convert.ToInt32(objDataReader["COMPANY_ID"].ToString());
                                objCompanyModel.ComapnayName = objDataReader["COMPANY_NAME"].ToString();
                                objCompanyModel.Address = objDataReader["ADDRESS"].ToString();
                                objCompanyModel.ContactNo = objDataReader["CONTACT_NO"].ToString();
                                objCompanyModel.ComapanyEmail = objDataReader["EMAIL"].ToString();
                                objCompanyModel.ActiveYN = objDataReader["ACTIVE_YN"].ToString();
                                objCompanyModel.ActiveStatus = objCompanyModel.ActiveYN == "Y" ? true : false;
                                objCompanyModel.CreateBy = objDataReader["CREATE_BY"].ToString();
                                objCompanyModel.CreateDate = objDataReader["CREATE_DATE"].ToString();
                                objCompanyModel.UpdateBy = objDataReader["UPDATE_BY"].ToString();
                                objCompanyModel.UpdateDate = objDataReader["UPDATE_DATE"].ToString();
                            }
                            return objCompanyModel;
                            
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

        public async Task<string> SaveOrUpdateCompany(CompanySetupModel objCompanySetupModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_COMPANY_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_COMPANY_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objCompanySetupModel.CompanyId;
            objOracleCommand.Parameters.Add("P_COMPANY_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.ComapnayName) ? objCompanySetupModel.ComapnayName.Trim() : null;
            objOracleCommand.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.Address) ? objCompanySetupModel.Address.Trim() : null;
            objOracleCommand.Parameters.Add("P_CONTACT_NO", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.ContactNo) ? objCompanySetupModel.ContactNo.Trim() : null;
            objOracleCommand.Parameters.Add("P_EMAIL", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.ComapanyEmail) ? objCompanySetupModel.ComapanyEmail.Trim() : null;
            objOracleCommand.Parameters.Add("P_ACTIVE_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.ActiveYN) ? objCompanySetupModel.ActiveYN.Trim() : null;
            objOracleCommand.Parameters.Add("P_CREATE_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objCompanySetupModel.CreateBy) ? objCompanySetupModel.CreateBy.Trim() : null;

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

        public async Task<string> DeleteCompany(int companyId)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_COMPANY_DELETE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_COMPANY_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = companyId;

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

        #region MedicineTypeSetup
        public async Task<List<MedicineTypeSetupModel>> GetAllMedicineTypeList()
        {
            var sql = "SELECT MEDICINE_TYPE_ID,MEDICINE_TYPE_NAME,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_MEDICINE_TYPE_LIST";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<MedicineTypeSetupModel> objMedicineTypeModels = new List<MedicineTypeSetupModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                MedicineTypeSetupModel model = new MedicineTypeSetupModel
                                {
                                    MedicineTypeId = Convert.ToInt32(objDataReader["MEDICINE_TYPE_ID"].ToString()),
                                    MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString(),
                                    ActiveYN = objDataReader["ACTIVE_YN"].ToString(),
                                    CreateBy = objDataReader["CREATE_BY"].ToString(),
                                    CreateDate = objDataReader["CREATE_DATE"].ToString(),
                                    UpdateBy = objDataReader["UPDATE_BY"].ToString(),
                                    UpdateDate = objDataReader["UPDATE_DATE"].ToString()
                                };
                                objMedicineTypeModels.Add(model);
                            }
                            return objMedicineTypeModels;
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

        public async Task<MedicineTypeSetupModel> GetAMedicineType(int medicineTypeId)
        {
            var sql = "SELECT MEDICINE_TYPE_ID,MEDICINE_TYPE_NAME,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_MEDICINE_TYPE_LIST " +
                " WHERE MEDICINE_TYPE_ID = :MEDICINE_TYPE_ID ";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":MEDICINE_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = medicineTypeId;
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        MedicineTypeSetupModel objMedicineTypeModel = new MedicineTypeSetupModel();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {

                                objMedicineTypeModel.MedicineTypeId = Convert.ToInt32(objDataReader["MEDICINE_TYPE_ID"].ToString());
                                objMedicineTypeModel.MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString();
                                objMedicineTypeModel.ActiveYN = objDataReader["ACTIVE_YN"].ToString();
                                objMedicineTypeModel.ActiveStatus = objMedicineTypeModel.ActiveYN == "Y" ? true : false;
                                objMedicineTypeModel.CreateBy = objDataReader["CREATE_BY"].ToString();
                                objMedicineTypeModel.CreateDate = objDataReader["CREATE_DATE"].ToString();
                                objMedicineTypeModel.UpdateBy = objDataReader["UPDATE_BY"].ToString();
                                objMedicineTypeModel.UpdateDate = objDataReader["UPDATE_DATE"].ToString();
                            }
                            return objMedicineTypeModel;

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

        public async Task<string> SaveOrUpdateMedicineType(MedicineTypeSetupModel objMedicineTypeSetupModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_MEDICINE_TYPE_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_MEDICINE_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineTypeSetupModel.MedicineTypeId;
            objOracleCommand.Parameters.Add("P_MEDICINE_TYPE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineTypeSetupModel.MedicineTypeName) ? objMedicineTypeSetupModel.MedicineTypeName.Trim() : null;
            objOracleCommand.Parameters.Add("P_ACTIVE_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineTypeSetupModel.ActiveYN) ? objMedicineTypeSetupModel.ActiveYN.Trim() : null;
            objOracleCommand.Parameters.Add("P_CREATE_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineTypeSetupModel.CreateBy) ? objMedicineTypeSetupModel.CreateBy.Trim() : null;

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

        public async Task<string> DeleteMedicineType(int medicineTypeId)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_MEDICINE_TYPE_DELETE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_MEDICINE_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = medicineTypeId;

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

        #region PaymentTypeSetup
        public async Task<List<PaymentTypeSetupModel>> GetAllPaymentTypeList()
        {
            var sql = "SELECT PAYMENT_TYPE_ID,PAYMENT_TYPE_NAME,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_PAYMENT_TYPE_LIST";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<PaymentTypeSetupModel> objPaymentTypeModels = new List<PaymentTypeSetupModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                PaymentTypeSetupModel model = new PaymentTypeSetupModel
                                {
                                    PaymentTypeId = Convert.ToInt32(objDataReader["PAYMENT_TYPE_ID"].ToString()),
                                    PaymentTypeName = objDataReader["PAYMENT_TYPE_NAME"].ToString(),
                                    ActiveYN = objDataReader["ACTIVE_YN"].ToString(),
                                    CreateBy = objDataReader["CREATE_BY"].ToString(),
                                    CreateDate = objDataReader["CREATE_DATE"].ToString(),
                                    UpdateBy = objDataReader["UPDATE_BY"].ToString(),
                                    UpdateDate = objDataReader["UPDATE_DATE"].ToString()
                                };
                                objPaymentTypeModels.Add(model);
                            }
                            return objPaymentTypeModels;
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

        public async Task<PaymentTypeSetupModel> GetAPaymentType(int paymentTypeId)
        {
            var sql = "SELECT PAYMENT_TYPE_ID,PAYMENT_TYPE_NAME,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_PAYMENT_TYPE_LIST " +
                " WHERE PAYMENT_TYPE_ID = :PAYMENT_TYPE_ID ";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":PAYMENT_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = paymentTypeId;
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        PaymentTypeSetupModel objPaymentTypeModel = new PaymentTypeSetupModel();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {

                                objPaymentTypeModel.PaymentTypeId = Convert.ToInt32(objDataReader["PAYMENT_TYPE_ID"].ToString());
                                objPaymentTypeModel.PaymentTypeName = objDataReader["PAYMENT_TYPE_NAME"].ToString();
                                objPaymentTypeModel.ActiveYN = objDataReader["ACTIVE_YN"].ToString();
                                objPaymentTypeModel.ActiveStatus = objPaymentTypeModel.ActiveYN == "Y" ? true : false;
                                objPaymentTypeModel.CreateBy = objDataReader["CREATE_BY"].ToString();
                                objPaymentTypeModel.CreateDate = objDataReader["CREATE_DATE"].ToString();
                                objPaymentTypeModel.UpdateBy = objDataReader["UPDATE_BY"].ToString();
                                objPaymentTypeModel.UpdateDate = objDataReader["UPDATE_DATE"].ToString();
                            }
                            return objPaymentTypeModel;

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

        public async Task<string> SaveOrUpdatePaymentType(PaymentTypeSetupModel objPaymentTypeSetupModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_PAYMENT_TYPE_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_PAYMENT_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPaymentTypeSetupModel.PaymentTypeId;
            objOracleCommand.Parameters.Add("P_PAYMENT_TYPE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objPaymentTypeSetupModel.PaymentTypeName) ? objPaymentTypeSetupModel.PaymentTypeName.Trim() : null;
            objOracleCommand.Parameters.Add("P_ACTIVE_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objPaymentTypeSetupModel.ActiveYN) ? objPaymentTypeSetupModel.ActiveYN.Trim() : null;
            objOracleCommand.Parameters.Add("P_CREATE_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objPaymentTypeSetupModel.CreateBy) ? objPaymentTypeSetupModel.CreateBy.Trim() : null;

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

        public async Task<string> DeletePaymentType(int paymentTypeId)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_PAYMENT_TYPE_DELETE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_PAYMENT_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = paymentTypeId;

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

        #region MedicineSetup
        public async Task<List<MedicineSetupModel>> GetAllMedicineList()
        {
            var sql = "SELECT MEDICINE_ID,MEDICINE_NAME,MEDICINE_DESCRIPTION,COMPANY_ID,COMPANY_NAME,MEDICINE_TYPE_ID,MEDICINE_TYPE_NAME,PURCHASE_PRICE," +
                "SALE_PRICE,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE,TOTAL_PURCHASE_QTY,TOTAL_SALE_QTY,STOCK_QTY FROM VEW_MEDICINE_LIST";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<MedicineSetupModel> objMedicineSetupModels = new List<MedicineSetupModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                MedicineSetupModel model = new MedicineSetupModel
                                {
                                    MedicineId = Convert.ToInt32(objDataReader["MEDICINE_ID"].ToString()),
                                    MedicineName = objDataReader["MEDICINE_NAME"].ToString(),
                                    MedicineDescription = objDataReader["MEDICINE_DESCRIPTION"].ToString(),
                                    CompanyId = Convert.ToInt32(objDataReader["COMPANY_ID"].ToString()),
                                    CompanyName = objDataReader["COMPANY_NAME"].ToString(),
                                    MedicineTypeId = Convert.ToInt32(objDataReader["MEDICINE_TYPE_ID"].ToString()),
                                    MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString(),
                                    PurchasePrice = Convert.ToDouble(objDataReader["PURCHASE_PRICE"].ToString()),
                                    SalePrice = Convert.ToDouble(objDataReader["SALE_PRICE"].ToString()),
                                    ActiveYN = objDataReader["ACTIVE_YN"].ToString(),
                                    CreateBy = objDataReader["CREATE_BY"].ToString(),
                                    CreateDate = objDataReader["CREATE_DATE"].ToString(),
                                    UpdateBy = objDataReader["UPDATE_BY"].ToString(),
                                    UpdateDate = objDataReader["UPDATE_DATE"].ToString(),
                                    TotalPurchaseQty = Convert.ToInt32(objDataReader["TOTAL_PURCHASE_QTY"].ToString()),
                                    TotalSaleQty = Convert.ToInt32(objDataReader["TOTAL_SALE_QTY"].ToString()),
                                    StockQty = Convert.ToInt32(objDataReader["STOCK_QTY"].ToString())
                                };
                                objMedicineSetupModels.Add(model);
                            }
                            return objMedicineSetupModels;
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

        public async Task<MedicineSetupModel> GetAMedicine(int medicineId)
        {
            var sql = "SELECT MEDICINE_ID,MEDICINE_NAME,MEDICINE_DESCRIPTION,COMPANY_ID,COMPANY_NAME,MEDICINE_TYPE_ID,MEDICINE_TYPE_NAME,PURCHASE_PRICE," +
                    "SALE_PRICE,ACTIVE_YN,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE FROM VEW_MEDICINE_LIST " +
                    " WHERE MEDICINE_ID = :MEDICINE_ID ";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":MEDICINE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = medicineId;
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        MedicineSetupModel objMedicineSetupModel = new MedicineSetupModel();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                objMedicineSetupModel.MedicineId = Convert.ToInt32(objDataReader["MEDICINE_ID"].ToString());
                                objMedicineSetupModel.MedicineName = objDataReader["MEDICINE_NAME"].ToString();
                                objMedicineSetupModel.MedicineDescription = objDataReader["MEDICINE_DESCRIPTION"].ToString();
                                objMedicineSetupModel.CompanyId = Convert.ToInt32(objDataReader["COMPANY_ID"].ToString());
                                objMedicineSetupModel.CompanyName = objDataReader["COMPANY_NAME"].ToString();
                                objMedicineSetupModel.MedicineTypeId = Convert.ToInt32(objDataReader["MEDICINE_TYPE_ID"].ToString());
                                objMedicineSetupModel.MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString();
                                objMedicineSetupModel.PurchasePrice = Convert.ToDouble(objDataReader["PURCHASE_PRICE"].ToString());
                                objMedicineSetupModel.SalePrice = Convert.ToDouble(objDataReader["SALE_PRICE"].ToString());
                                objMedicineSetupModel.ActiveYN = objDataReader["ACTIVE_YN"].ToString();
                                objMedicineSetupModel.ActiveStatus = objMedicineSetupModel.ActiveYN == "Y" ? true : false;
                                objMedicineSetupModel.CreateBy = objDataReader["CREATE_BY"].ToString();
                                objMedicineSetupModel.CreateDate = objDataReader["CREATE_DATE"].ToString();
                                objMedicineSetupModel.UpdateBy = objDataReader["UPDATE_BY"].ToString();
                                objMedicineSetupModel.UpdateDate = objDataReader["UPDATE_DATE"].ToString();
                            }
                            return objMedicineSetupModel;

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

        public async Task<string> SaveOrUpdateMedicine(MedicineSetupModel objMedicineSetupModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_MEDICINE_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_MEDICINE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineSetupModel.MedicineId;
            objOracleCommand.Parameters.Add("P_MEDICINE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineSetupModel.MedicineName) ? objMedicineSetupModel.MedicineName.Trim() : null;
            objOracleCommand.Parameters.Add("P_MEDICINE_DESCRIPTION", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineSetupModel.MedicineDescription) ? objMedicineSetupModel.MedicineDescription.Trim() : null;
            objOracleCommand.Parameters.Add("P_COMPANY_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineSetupModel.CompanyId;
            objOracleCommand.Parameters.Add("P_MEDICINE_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineSetupModel.MedicineTypeId;
            objOracleCommand.Parameters.Add("P_PURCHASE_PRICE", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineSetupModel.PurchasePrice;
            objOracleCommand.Parameters.Add("P_SALE_PRICE", OracleDbType.Varchar2, ParameterDirection.Input).Value = objMedicineSetupModel.SalePrice;
            objOracleCommand.Parameters.Add("P_ACTIVE_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineSetupModel.ActiveYN) ? objMedicineSetupModel.ActiveYN.Trim() : null;
            objOracleCommand.Parameters.Add("P_CREATE_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objMedicineSetupModel.CreateBy) ? objMedicineSetupModel.CreateBy.Trim() : null;

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

        public async Task<string> DeleteMedicine(int medicineId)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_MEDICINE_DELETE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_MEDICINE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = medicineId;

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
    }
}