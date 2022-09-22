using MedicineEshop.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MedicineEshop.DAL
{
    public class SaleMedicineDal
    {
        private OracleTransaction _trans;

        #region "Oracle Connection Check"
        private OracleConnection GetConnection()
        {
            var conString = ConfigurationManager.ConnectionStrings["OracleDbContext"];
            string strConnString = conString.ConnectionString;
            return new OracleConnection(strConnString);
        }
        #endregion

        public async Task<List<SaleMedicineModel>> SaleMedicineList(string customerId)
        {
            var sql = "SELECT  SALE_INFO_ID," +
                        "INVOICE_NUMBER," +
                        "INVOICE_DATE," +
                        "TOTAL_ITEM," +
                        "TOTAL_AMOUNT," +
                        "CUSTOMER_ID," +
                        "CUSTOMER_NAME," +
                        "PAYMENT_TYPE_ID," +
                        "PAYMENT_TYPE_NAME," +
                        "PAYMENT_REF_NUMBER," +
                        "PAYMENT_VERIFIED_YN," +
                        "DELIVERY_YN," +
                        "DELIVERY_DATE," +
                        "DELIVERY_BY," +
                        "DELIVERY_BY_NAME," +
                        "PRESCRIPTION_VERIFY_YN," +
                        "PRESCRIPTION_IMAGE FROM VEW_MEDICINE_SOLD ";
            if(customerId != "")
            {
                sql = sql + " WHERE CUSTOMER_ID = '" + customerId +"'";
            }

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<SaleMedicineModel> objSaleMedicineModels = new List<SaleMedicineModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                SaleMedicineModel model = new SaleMedicineModel
                                {
                                   SaleInfoId = Convert.ToInt32(objDataReader["SALE_INFO_ID"].ToString()),
                                    InvoiceNumber = objDataReader["INVOICE_NUMBER"].ToString(),
                                    InvoiceDate = objDataReader["INVOICE_DATE"].ToString(),
                                    TotalItem = Convert.ToInt32(objDataReader["TOTAL_ITEM"].ToString()),
                                    TotalSaleAmount = Convert.ToDouble(objDataReader["TOTAL_AMOUNT"].ToString()),
                                    CustomerName = objDataReader["CUSTOMER_NAME"].ToString(),
                                    PaymentType = objDataReader["PAYMENT_TYPE_NAME"].ToString(),
                                    PaymentRefNumber = objDataReader["PAYMENT_REF_NUMBER"].ToString(),
                                    PaymentVerifyYN = objDataReader["PAYMENT_VERIFIED_YN"].ToString(),
                                    DeliveryYN = objDataReader["DELIVERY_YN"].ToString(),
                                    DeliveryDate = objDataReader["DELIVERY_DATE"].ToString(),
                                    DeliveryBy = objDataReader["DELIVERY_BY_NAME"].ToString(),
                                    PrescriptionVerifyYN = objDataReader["PRESCRIPTION_VERIFY_YN"].ToString(),
                                    ImageString = Path.Combine("/Files/Prescriptions/", objDataReader["PRESCRIPTION_IMAGE"].ToString())
                                };
                                objSaleMedicineModels.Add(model);
                            }
                            return objSaleMedicineModels;
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

        public async Task<List<SaleMedicineModel>> GetOrderDataById(string orderId)
        {
            var sql = "SELECT  SALE_INFO_ID," +
                        "INVOICE_NUMBER," +
                        "INVOICE_DATE," +
                        "TOTAL_ITEM," +
                        "TOTAL_AMOUNT," +
                        "CUSTOMER_ID," +
                        "CUSTOMER_NAME," +
                        "PAYMENT_TYPE_ID," +
                        "PAYMENT_TYPE_NAME," +
                        "PAYMENT_REF_NUMBER," +
                        "PAYMENT_VERIFIED_YN," +
                        "DELIVERY_YN," +
                        "DELIVERY_DATE," +
                        "DELIVERY_BY," +
                        "DELIVERY_BY_NAME," +
                        "PRESCRIPTION_VERIFY_YN," +
                        "PRESCRIPTION_IMAGE FROM VEW_MEDICINE_SOLD WHERE SALE_INFO_ID = '" + orderId + "'";

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<SaleMedicineModel> objSaleMedicineModels = new List<SaleMedicineModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                SaleMedicineModel model = new SaleMedicineModel
                                {
                                    SaleInfoId = Convert.ToInt32(objDataReader["SALE_INFO_ID"].ToString()),
                                    InvoiceNumber = objDataReader["INVOICE_NUMBER"].ToString(),
                                    InvoiceDate = objDataReader["INVOICE_DATE"].ToString(),
                                    TotalItem = Convert.ToInt32(objDataReader["TOTAL_ITEM"].ToString()),
                                    TotalSaleAmount = Convert.ToDouble(objDataReader["TOTAL_AMOUNT"].ToString()),
                                    CustomerName = objDataReader["CUSTOMER_NAME"].ToString(),
                                    PaymentType = objDataReader["PAYMENT_TYPE_NAME"].ToString(),
                                    PaymentRefNumber = objDataReader["PAYMENT_REF_NUMBER"].ToString(),
                                    PaymentVerifyYN = objDataReader["PAYMENT_VERIFIED_YN"].ToString(),
                                    DeliveryYN = objDataReader["DELIVERY_YN"].ToString(),
                                    DeliveryDate = objDataReader["DELIVERY_DATE"].ToString(),
                                    DeliveryBy = objDataReader["DELIVERY_BY_NAME"].ToString(),
                                    PrescriptionVerifyYN = objDataReader["PRESCRIPTION_VERIFY_YN"].ToString(),
                                    ImageString = Path.Combine("/Files/Prescriptions/", objDataReader["PRESCRIPTION_IMAGE"].ToString())
                                };
                                objSaleMedicineModels.Add(model);
                            }
                            return objSaleMedicineModels;
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

        public async Task<List<SaleMedicineItem>> GetSaleMedicineList(int saleInfoId)
        {
            var sql = " SELECT MEDICINE_ID,MEDICINE_NAME,COMPANY_NAME,MEDICINE_TYPE_NAME,SALE_PRICE,QUANTITY,TOTAL FROM VEW_MEDICINE_SOLD_LIST WHERE SALE_INFO_ID = :SALE_INFO_ID ";

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":SALE_INFO_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = saleInfoId;

                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<SaleMedicineItem> objSaleMedicineItems = new List<SaleMedicineItem>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                SaleMedicineItem model = new SaleMedicineItem
                                {
                                    MedicineName = objDataReader["MEDICINE_NAME"].ToString(),
                                    CompanyName = objDataReader["COMPANY_NAME"].ToString(),
                                    MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString(),
                                    SalePrice = Convert.ToDouble(objDataReader["SALE_PRICE"].ToString()),
                                    Quantity = Convert.ToInt32(objDataReader["QUANTITY"].ToString()),
                                    TotalAmount = Convert.ToDouble(objDataReader["TOTAL"].ToString())
                                };
                                objSaleMedicineItems.Add(model);
                            }
                            return objSaleMedicineItems;
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

        public async Task<string> SaveSaleInfo(SaleMedicineModel objSaleMedicineModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_SALE_INFO_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_TOTAL_ITEM", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineModel.TotalItem;
            objOracleCommand.Parameters.Add("P_TOTAL_AMOUNT", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineModel.TotalSaleAmount;
            objOracleCommand.Parameters.Add("P_CUSTOMER_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.CustomerId) ? objSaleMedicineModel.CustomerId : null;
            objOracleCommand.Parameters.Add("P_PAYMENT_TYPE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineModel.PaymentTypeID;
            objOracleCommand.Parameters.Add("P_PAYMENT_REF_NUMBER", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.PaymentRefNumber) ? objSaleMedicineModel.PaymentRefNumber : null;
            objOracleCommand.Parameters.Add("P_PRESCRIPTION_IMAGE", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.ImageString) ? objSaleMedicineModel.ImageString : null;

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

        public async Task<string> UpdateSaleInfo(SaleMedicineModel objSaleMedicineModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_SALE_INFO_UPDATE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_SALE_INFO_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineModel.SaleInfoId;
            objOracleCommand.Parameters.Add("P_DELIVERY_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.DeliveryBy) ? objSaleMedicineModel.DeliveryBy : null;
            objOracleCommand.Parameters.Add("P_PAYMENT_VERIFIED_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.PaymentVerifyYN) ? objSaleMedicineModel.PaymentVerifyYN : null;
            objOracleCommand.Parameters.Add("P_DELIVERY_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.DeliveryYN) ? objSaleMedicineModel.DeliveryYN : null;
            objOracleCommand.Parameters.Add("P_PRESCRIPTION_VERIFY_YN", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineModel.PrescriptionVerifyYN) ? objSaleMedicineModel.PrescriptionVerifyYN : null;

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

        public async Task<string> SaveSaleItem(SaleMedicineItem objSaleMedicineItem)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_SALE_ITEM_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_SALE_INFO_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineItem.SaleInfoId;
            objOracleCommand.Parameters.Add("P_MEDICINE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineItem.MedicineId;
            objOracleCommand.Parameters.Add("P_MEDICINE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objSaleMedicineItem.MedicineName) ? objSaleMedicineItem.MedicineName : null;
            objOracleCommand.Parameters.Add("P_SALE_PRICE", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineItem.SalePrice;
            objOracleCommand.Parameters.Add("P_QUANTITY", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineItem.Quantity;
            objOracleCommand.Parameters.Add("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input).Value = objSaleMedicineItem.TotalAmount;

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

    }
}