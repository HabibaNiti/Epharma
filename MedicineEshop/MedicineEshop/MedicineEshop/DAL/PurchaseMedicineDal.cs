using Oracle.ManagedDataAccess.Client;
using MedicineEshop.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace MedicineEshop.DAL
{
    public class PurchaseMedicineDal
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

        public async Task<List<PurchaseMedicineModel>> PurchaseMedicineList()
        {
            var sql = "SELECT  PURCHASE_INFO_ID,PURCHASE_NUMBER,PURCHASE_DATE,TOTAL_ITEM,TOTAL_AMOUNT,PURCHASE_BY FROM VEW_PURCHASE_MEDICINE";

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<PurchaseMedicineModel> objPurchaseMedicineModels = new List<PurchaseMedicineModel>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                PurchaseMedicineModel model = new PurchaseMedicineModel
                                {
                                    PurchaseInfoId = Convert.ToInt32(objDataReader["PURCHASE_INFO_ID"].ToString()),
                                    PurchaeNumber = objDataReader["PURCHASE_NUMBER"].ToString(),
                                    PurchaseDate = objDataReader["PURCHASE_DATE"].ToString(),
                                    TotalItem = Convert.ToInt32(objDataReader["TOTAL_ITEM"].ToString()),
                                    TotalPurchaseAmount = Convert.ToDouble(objDataReader["TOTAL_AMOUNT"].ToString()),
                                    PurchaedBy = objDataReader["PURCHASE_BY"].ToString()
                                };
                                objPurchaseMedicineModels.Add(model);
                            }
                            return objPurchaseMedicineModels;
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

        public async Task<List<PurchaseMedicineItem>> GetPurchaseMedicineItem(int purchaseInfoId)
        {
            var sql = " SELECT MEDICINE_NAME, COMPANY_NAME, MEDICINE_TYPE_NAME, PURCHASE_PRICE, QUANTITY, TOTAL FROM VEW_PURCHASE_LIST WHERE PURCHASE_INFO_ID = :PURCHASE_INFO_ID ";

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {
                    objCommand.Parameters.Add(":PURCHASE_INFO_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = purchaseInfoId;

                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        List<PurchaseMedicineItem> objPurchaseMedicineItems = new List<PurchaseMedicineItem>();
                        try
                        {
                            while (await objDataReader.ReadAsync())
                            {
                                PurchaseMedicineItem model = new PurchaseMedicineItem
                                {
                                    MedicineName = objDataReader["MEDICINE_NAME"].ToString(),
                                    CompanyName = objDataReader["COMPANY_NAME"].ToString(),
                                    MedicineTypeName = objDataReader["MEDICINE_TYPE_NAME"].ToString(),
                                    PurchasePrice = Convert.ToDouble(objDataReader["PURCHASE_PRICE"].ToString()),
                                    Quantity = Convert.ToInt32(objDataReader["QUANTITY"].ToString()),
                                    TotalAmount = Convert.ToDouble(objDataReader["TOTAL"].ToString())
                                };
                                objPurchaseMedicineItems.Add(model);
                            }
                            return objPurchaseMedicineItems;
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

        public async Task<int> GetTotalRowCount(DataTableAjaxPostModel model)
        {
            string sql = "SELECT COUNT(*) COUNT FROM VEW_MEDICINE_FOR_DATATABLE WHERE ACTIVEYN = 'Y' ";

            using (OracleConnection objConnection = GetConnection())
            {
                using (OracleCommand objCommand = new OracleCommand(sql, objConnection) { CommandType = CommandType.Text })
                {

                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        try
                        {
                            int rowCount = 0;
                            while (await objDataReader.ReadAsync())
                            {
                                rowCount = Convert.ToInt32(objDataReader["COUNT"].ToString());
                            }
                            return rowCount;
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

        public async Task<List<MedicineSetupModel>> GetAllMedicineForPurchase(DataTableAjaxPostModel model)
        {
            var query = "SELECT " +
                          "ROW_NUMBER () OVER (ORDER BY MEDICINEID) AS RN, " +
                          "MEDICINEID," +
                          "MEDICINENAME," +
                          "MEDICINEDESCRIPTION," +
                          "COMPANYID," +
                          "COMPANYNAME," +
                          "MEDICINETYPEID," +
                          "MEDICINETYPENAME," +
                          "PURCHASEPRICE," +
                          "SALEPRICE," +
                          "ACTIVEYN," +
                          "CREATEBY," +
                          "CREATEDATE," +
                          "UPDATEBY," +
                          "UPDATEDATE," +
                          "TOTALPURCHASEQTY," +
                          "TOTALSALEQTY," +
                          "STOCKQTY " +
                          "FROM VEW_MEDICINE_FOR_DATATABLE WHERE ACTIVEYN = 'Y' ";

            if (!string.IsNullOrWhiteSpace(model.search.value))
            {
                query += "and ( (lower(MEDICINENAME) like lower(:searchBy)  or upper(MEDICINENAME)like upper(:searchBy) )" +
                         "or (lower(COMPANYNAME) like lower(:searchBy)  or upper(COMPANYNAME)like upper(:searchBy) )" +
                         "or (lower(MEDICINETYPENAME) like lower(:searchBy)  or upper(MEDICINETYPENAME)like upper(:searchBy)) ) ";
            }
            

            if (model.order != null)
            {
                query += "ORDER BY " + model.columns[model.order[0].column].data + " " + model.order[0].dir.ToUpper();
            }

            query = "SELECT * FROM (" + query + ") WHERE RN BETWEEN  '" + model.start + "' AND '" + (model.start + model.length) + "' ";

            var totalRow = await GetTotalRowCount(model);

            using (OracleConnection objConnection = GetConnection())
            {

                using (OracleCommand objCommand = new OracleCommand(query, objConnection) { CommandType = CommandType.Text })
                {
                    if (!string.IsNullOrWhiteSpace(model.search.value))
                    {
                        objCommand.Parameters.Add(":SearchBy", OracleDbType.Varchar2, ParameterDirection.Input).Value = $"%{model.search.value}%";
                    }

                    await objConnection.OpenAsync();
                    using (OracleDataReader objDataReader = (OracleDataReader)await objCommand.ExecuteReaderAsync())
                    {
                        try
                        {
                            List<MedicineSetupModel> objMedicineSetupModel = new List<MedicineSetupModel>();
                            while (await objDataReader.ReadAsync())
                            {
                                MedicineSetupModel medicineItem = new MedicineSetupModel
                                {
                                    MedicineId = Convert.ToInt32(objDataReader["MEDICINEID"].ToString()),
                                    TotalItem = totalRow,
                                    MedicineName = objDataReader["MEDICINENAME"].ToString(),
                                    CompanyName = objDataReader["COMPANYNAME"].ToString(),
                                    MedicineTypeName = objDataReader["MEDICINETYPENAME"].ToString(),
                                    PurchasePrice = Convert.ToDouble(objDataReader["PURCHASEPRICE"].ToString()),
                                    TotalPurchaseQty = Convert.ToInt32(objDataReader["TOTALPURCHASEQTY"].ToString()),
                                    StockQty = Convert.ToInt32(objDataReader["STOCKQTY"].ToString())
                                };
                                objMedicineSetupModel.Add(medicineItem);
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

        public async Task<string> SavePurchaseInfo(PurchaseMedicineModel objPurchaseMedicineModel)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_PURCHASE_INFO_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_TOTAL_ITEM", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineModel.TotalItem;
            objOracleCommand.Parameters.Add("P_TOTAL_AMOUNT", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineModel.TotalPurchaseAmount;
            objOracleCommand.Parameters.Add("P_PURCHASE_BY", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objPurchaseMedicineModel.PurchaedBy) ? objPurchaseMedicineModel.PurchaedBy : null;

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

        public async Task<string> SavePurchaseItem(PurchaseMedicineItem objPurchaseMedicineItem)
        {
            string strMessage;

            OracleCommand objOracleCommand = new OracleCommand("PRO_PURCHASE_ITEM_SAVE")
            {
                CommandType = CommandType.StoredProcedure
            };

            objOracleCommand.Parameters.Add("P_PURCHASE_INFO_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineItem.PurchaseInfoId;
            objOracleCommand.Parameters.Add("P_MEDICINE_ID", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineItem.MedicineId;
            objOracleCommand.Parameters.Add("P_MEDICINE_NAME", OracleDbType.Varchar2, ParameterDirection.Input).Value = !string.IsNullOrWhiteSpace(objPurchaseMedicineItem.MedicineName) ? objPurchaseMedicineItem.MedicineName : null;
            objOracleCommand.Parameters.Add("P_PURCHASE_PRICE", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineItem.PurchasePrice;
            objOracleCommand.Parameters.Add("P_QUANTITY", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineItem.Quantity;
            objOracleCommand.Parameters.Add("P_TOTAL", OracleDbType.Varchar2, ParameterDirection.Input).Value = objPurchaseMedicineItem.TotalAmount;

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