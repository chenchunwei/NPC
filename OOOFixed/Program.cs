using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Aspose.Cells;
using System.Data.SqlClient;


namespace OOOFixed
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = System.Configuration.ConfigurationManager.AppSettings["path"].ToString();

            var logger = new DefaultLoggerFactory().GetLogger("XXXFixed");
            Workbook workbook = new Workbook(file);
            var worksheet = workbook.Worksheets[0];
            var cells = worksheet.Cells;
            var dtImport = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            var rowsOfInsert = new List<int>();
            var rowsOfError = new List<int>();
            //遍历数据集合
            if (dtImport.Rows.Count > 0)
            {
                #region 新增语句
                var sql = @"INSERT INTO  [BerthRecord]
                                   ([Id]
                                   ,[No]
                                   ,[BerthId]
                                   ,[PlateNumber]
                                   ,[TimeOfBerthIn]
                                   ,[TimeOfBerthOut]
                                   ,[TotalFee]
                                   ,[TollCollectorIdOfBerthIn]
                                   ,[TollCollectorIdOfBerthOut]
                                   ,[BerthInRecordType]
                                   ,[BerthOutRecordType]
                                   ,[PaymentType]
                                   ,[BerthInDeviceSN]
                                   ,[BerthInDeviceNo]
                                   ,[BerthOutDeviceSN]
                                   ,[BerthOutDeviceNo]
                                   ,[CreateTime]
                                   ,[LastModifyDateTime]
                                   ,[RecordStatus])
                             VALUES
                                   (@Id
                                   ,@No
                                   ,@BerthId
                                   ,@PlateNumber
                                   ,@TimeOfBerthIn
                                   ,@TimeOfBerthOut 
                                   ,@TotalFee 
                                   ,@TollCollectorIdOfBerthIn 
                                   ,@TollCollectorIdOfBerthOut 
                                   ,@BerthInRecordType 
                                   ,@BerthOutRecordType 
                                   ,@PaymentType 
                                   ,@BerthInDeviceSN 
                                   ,@BerthInDeviceNo 
                                   ,@BerthOutDeviceSN 
                                   ,@BerthOutDeviceNo 
                                   ,@CreateTime 
                                   ,@LastModifyDateTime 
                                   ,@RecordStatus)";
                #endregion

                #region 更新语句
                var sqlOfUpdate = @"UPDATE  [BerthRecord]
                               SET [PlateNumber] = @PlateNumber 
                                  ,[TimeOfBerthIn] = @TimeOfBerthIn 
                                  ,[TimeOfBerthOut] = @TimeOfBerthOut 
                                  ,[TotalFee] = @TotalFee 
                                  ,[TollCollectorIdOfBerthIn] = @TollCollectorIdOfBerthIn 
                                  ,[TollCollectorIdOfBerthOut] = @TollCollectorIdOfBerthOut 
                                  ,[TotalTimeSpan] = @TotalTimeSpan 
                                  ,[BerthInRecordType] = @BerthInRecordType 
                                  ,[BerthOutRecordType] = @BerthOutRecordType 
                                  ,[PaymentType] = @PaymentType 
                                  ,[BerthInDeviceSN] = @BerthInDeviceSN 
                                  ,[BerthInDeviceNo] = @BerthInDeviceNo 
                                  ,[BerthOutDeviceSN] = @BerthOutDeviceSN 
                                  ,[BerthOutDeviceNo] = @BerthOutDeviceNo 
                                  ,[CreateTime] = @CreateTime 
                                  ,[LastModifyDateTime] = @LastModifyDateTime 
                                  ,[RecordStatus] = @RecordStatus 
                             WHERE [Id]=@Id";
                #endregion

                var mainSqlConnection = GetNewConnection();
                var rowNum = 1;
                var updateRows = 0;
                var insertRows = 0;
                var errorRows = 0;
                foreach (DataRow dr in dtImport.Rows)
                {
                    var timeOfBerthIn = string.Empty;
                    var timeOfBerthOut = string.Empty;
                    var status = string.Empty;
                    SqlDataReader sqlDataReader = null;
                    SqlConnection connOfInsert = null;
                    SqlConnection connOfUpdate = null;
                    try
                    {
                        #region 参数校验及赋值
                        if (dr["流水号"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行流水号为null", rowNum));
                        }
                        var recordNo = dr["流水号"].ToString().Trim();

                        if (dr["操作员"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行操作员为null", rowNum));
                        }
                        var collectorNo = dr["操作员"].ToString().Trim();

                        if (dr["泊位号"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行泊位号为null", rowNum));
                            continue;
                        }
                        var berthNo = dr["泊位号"].ToString().Trim();

                        if (dr["车牌"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行车牌为null", rowNum));
                        }
                        var plateNumber = dr["车牌"].ToString().Trim();
                        if (dr["进入时间"] == null && dr["离开时间"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行进入时间和离开时间均为null", rowNum));
                        }
                        if (dr["进入时间"] != null)
                        {
                            timeOfBerthIn = dr["进入时间"].ToString().Trim();
                        }
                        if (dr["离开时间"] != null)
                        {
                            timeOfBerthOut = dr["离开时间"].ToString().Trim();
                        }
                        if (dr["应收金额"] == null && dr["离开时间"] != null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行离开时间不为空应收金额为null", rowNum));
                        }
                        var money = dr["应收金额"].ToString().Trim();

                        if (dr["设备唯一码"] == null)
                        {
                            throw new ArgumentException(string.Format("第【{0}】行设备唯一码为null", rowNum));
                        }
                        var deviceSn = dr["设备唯一码"].ToString().Trim();

                        if (dr["上传标志"] != null)
                        {
                            status = dr["上传标志"].ToString().Trim();
                        }
                        #endregion

                        #region 判断记录是否存在
                        SqlCommand sqlCommandCount = new SqlCommand(string
                                .Format("Select count(*) from BerthRecord br Join Berth  b on b.Id=br.BerthId WHERE br.No='{0}' and b.No='{1}' and (br.BerthInDeviceSN='{2}' or br.BerthOutDeviceSN='{2}') and br.plateNumber='{3}'", recordNo, berthNo, deviceSn, plateNumber), mainSqlConnection);
                        var countObject = sqlCommandCount.ExecuteScalar();
                        var count = int.Parse(countObject.ToString());
                        sqlCommandCount.Dispose();
                        #endregion

                        if (count == 0)
                        {
                            #region 新增记录

                            connOfInsert = GetNewConnection();
                            var insertCommand = new SqlCommand(sql, connOfInsert);
                            insertCommand.Parameters.Add(CreateSqlParameter("Id", Guid.NewGuid()));

                            #region berthId
                            var connOfBerthId = GetNewConnection();
                            SqlCommand sqlCommandOfBerthId = new SqlCommand(string
                                .Format("Select Id from Berth where No='{0}'", berthNo), connOfBerthId);
                            object berthId = sqlCommandOfBerthId.ExecuteScalar();
                            sqlCommandOfBerthId.Dispose();
                            connOfBerthId.Close();
                            if (berthId == null)
                            {
                                throw new ArgumentException(string.Format("服务中不存在泊位号{0}！", berthNo));
                            }
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthId", berthId));

                            #endregion

                            insertCommand.Parameters.Add(CreateSqlParameter("PlateNumber", plateNumber));
                            Func<object> timeOfBerthInFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return DateTime.Parse(timeOfBerthIn);
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("TimeOfBerthIn", timeOfBerthInFunc()));
                            Func<object> timeOfBerthOutFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return DateTime.Parse(timeOfBerthOut);
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("TimeOfBerthOut", timeOfBerthOutFunc()));
                            Func<object> moneyFunc = () =>
                            {
                                if (string.IsNullOrEmpty(money))
                                {
                                    return 0;
                                }
                                return decimal.Parse(money);
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("TotalFee", moneyFunc()));

                            #region 泊入泊出收费员
                            var connOfCollection = GetNewConnection();
                            SqlCommand sqlCommandOfCollection = new SqlCommand(string
                                .Format("Select Id from TollCollector where No='{0}'", collectorNo), connOfCollection);
                            object collectorId = sqlCommandOfCollection.ExecuteScalar();
                            sqlCommandOfCollection.Dispose();
                            connOfCollection.Close();
                            if (collectorId == null)
                            {
                                throw new ArgumentNullException(string.Format("找不到对应的收费员{0}", collectorNo));
                            }
                            var collectorIdGuid = Guid.Parse(collectorId.ToString());
                            Func<object> tollCollectorIdOfBerthInFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return collectorIdGuid;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("TollCollectorIdOfBerthIn", tollCollectorIdOfBerthInFunc()));
                            Func<object> tollCollectorIdOfBerthOutFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return collectorIdGuid;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("TollCollectorIdOfBerthOut", tollCollectorIdOfBerthOutFunc()));

                            #endregion
                            Func<object> berthInRecordTypeFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return CovnertBerthInType(status);
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthInRecordType", berthInRecordTypeFunc()));
                            Func<object> berthOutRecordTypeFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return CovnertBerthInType(status);
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthOutRecordType", berthOutRecordTypeFunc()));
                            #region 收款类型
                            Func<object> paymentTypeFun = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                if (CovnertBerthOutType(status) == 2)
                                {
                                    return DBNull.Value;
                                }
                                return 1;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("PaymentType", paymentTypeFun()));
                            #endregion
                            Func<object> berthInDeviceSNFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return deviceSn;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthInDeviceSN", berthInDeviceSNFunc()));
                            Func<object> berthOutDeviceSNFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceSn;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthOutDeviceSN", berthOutDeviceSNFunc()));
                            #region 操作机器号
                            var connectOfDevice = GetNewConnection();
                            SqlCommand sqlCommandOfDevice = new SqlCommand(string.Format("Select No from Device where SN='{0}'", deviceSn), connectOfDevice);
                            object deviceNoObject = sqlCommandOfDevice.ExecuteScalar();
                            sqlCommandOfDevice.Dispose();
                            connectOfDevice.Close();
                            if (deviceNoObject == null)
                            {

                                throw new ArgumentException(string.Format("唯一码对应的设备未找到{0}", deviceSn));
                            }
                            Func<object> berthInDeviceNoFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceNoObject;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthInDeviceNo", berthInDeviceNoFunc()));
                            Func<object> berthOutDeviceNoFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceNoObject;
                            };
                            insertCommand.Parameters.Add(CreateSqlParameter("BerthOutDeviceNo", berthOutDeviceNoFunc()));
                            #endregion
                            var timeOfCreate = string.IsNullOrEmpty(timeOfBerthOut)
                                                   ? DateTime.Parse(timeOfBerthIn)
                                                   : DateTime.Parse(timeOfBerthOut);
                            insertCommand.Parameters.Add(CreateSqlParameter("CreateTime", timeOfCreate));
                            insertCommand.Parameters.Add(CreateSqlParameter("LastModifyDateTime", DateTime.Now));
                            insertCommand.Parameters.Add(CreateSqlParameter("No", recordNo));
                            insertCommand.Parameters.Add(CreateSqlParameter("RecordStatus", 0));
                            insertCommand.ExecuteNonQuery();
                            #endregion
                            connOfInsert.Close();
                            insertRows++;
                            rowsOfInsert.Add(rowNum);
                            logger.InfoFormat("第{0}行为遗漏新增，总新增{1}", rowNum, insertRows);
                        }
                        else if (count == 1)
                        {
                            #region 读出已存在的记录

                            SqlCommand recordsqlCommand = new SqlCommand(string
                                .Format("Select * from BerthRecord br Join Berth  b on b.Id=br.BerthId WHERE br.No='{0}' and b.No='{1}' and (br.BerthInDeviceSN='{2}' or br.BerthOutDeviceSN='{2}' and br.plateNumber='{3}')", recordNo, berthNo, deviceSn, plateNumber), mainSqlConnection);
                            sqlDataReader = recordsqlCommand.ExecuteReader();
                            #endregion

                            #region 更新记录
                            sqlDataReader.Read();
                            connOfUpdate = GetNewConnection();
                            var updateCommand = new SqlCommand(sqlOfUpdate, connOfUpdate);
                            updateCommand.Parameters.Add(CreateSqlParameter("PlateNumber", plateNumber));
                            Func<object> timeOfBerthInFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return DateTime.Parse(timeOfBerthIn);
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("TimeOfBerthIn", timeOfBerthInFunc()));
                            Func<object> timeOfBerthOutFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return DateTime.Parse(timeOfBerthOut);
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("TimeOfBerthOut", timeOfBerthOutFunc()));
                            Func<object> moneyFunc = () =>
                            {
                                if (string.IsNullOrEmpty(money))
                                {
                                    return 0;
                                }
                                return decimal.Parse(money);
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("TotalFee", moneyFunc()));

                            #region 收费员
                            var connOfCollection = GetNewConnection();
                            SqlCommand sqlCommandOfCollection = new SqlCommand(string
                                .Format("Select Id from TollCollector where No='{0}'", collectorNo), connOfCollection);
                            object collectorId = sqlCommandOfCollection.ExecuteScalar();
                            sqlCommandOfCollection.Dispose();
                            connOfCollection.Close();
                            if (collectorId == null)
                            {
                                throw new ArgumentNullException(string.Format("找不到对应的收费员{0}", collectorNo));
                            }
                            var collectorIdGuid = Guid.Parse(collectorId.ToString());
                            Func<object> tollCollectorIdOfBerthInFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return collectorIdGuid;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("TollCollectorIdOfBerthIn", tollCollectorIdOfBerthInFunc()));
                            Func<object> tollCollectorIdOfBerthOutFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return collectorIdGuid;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("TollCollectorIdOfBerthOut", tollCollectorIdOfBerthOutFunc()));

                            #endregion

                            updateCommand.Parameters.Add(CreateSqlParameter("TotalTimeSpan", DBNull.Value));
                            Func<object> berthInRecordTypeFunc = () =>
                            {
                                if (sqlDataReader["BerthInRecordType"] != null && sqlDataReader["BerthInRecordType"] != DBNull.Value)
                                {
                                    return int.Parse(sqlDataReader["BerthInRecordType"].ToString());
                                }
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return CovnertBerthInType(status);
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthInRecordType", berthInRecordTypeFunc()));
                            Func<object> berthOutRecordTypeFunc = () =>
                            {
                                if (sqlDataReader["BerthOutRecordType"] != null && sqlDataReader["BerthOutRecordType"] != DBNull.Value)
                                {
                                    return int.Parse(sqlDataReader["BerthOutRecordType"].ToString());
                                }
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return CovnertBerthInType(status);
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthOutRecordType", berthOutRecordTypeFunc()));
                            #region 收费类型
                            Func<object> paymentTypeFun = () =>
                            {
                                if ((sqlDataReader["BerthOutRecordType"] != null && sqlDataReader["BerthOutRecordType"] != DBNull.Value && sqlDataReader["BerthOutRecordType"].ToString() == "2"))
                                {
                                    return DBNull.Value;
                                }
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                if (sqlDataReader["PaymentType"] != null && sqlDataReader["PaymentType"] != DBNull.Value)
                                {
                                    return int.Parse(sqlDataReader["PaymentType"].ToString());
                                }
                                return 1;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("PaymentType", paymentTypeFun()));
                            #endregion
                            Func<object> berthInDeviceSNFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthIn))
                                {
                                    return DBNull.Value;
                                }
                                return deviceSn;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthInDeviceSN", berthInDeviceSNFunc()));
                            Func<object> berthOutDeviceSNFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceSn;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthOutDeviceSN", berthOutDeviceSNFunc()));
                            #region 设备号
                            var connectOfDevice = GetNewConnection();
                            SqlCommand sqlCommandOfDevice = new SqlCommand(string.Format("Select No from Device where SN='{0}'", deviceSn), connectOfDevice);
                            object deviceNoObject = sqlCommandOfDevice.ExecuteScalar();
                            sqlCommandOfDevice.Dispose();
                            connectOfDevice.Close();
                            if (deviceNoObject == null)
                            {

                                throw new ArgumentException(string.Format("唯一码对应的设备未找到{0}", deviceSn));
                            }
                            Func<object> berthInDeviceNoFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceNoObject;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthInDeviceNo", berthInDeviceNoFunc()));
                            Func<object> berthOutDeviceNoFunc = () =>
                            {
                                if (string.IsNullOrEmpty(timeOfBerthOut))
                                {
                                    return DBNull.Value;
                                }
                                return deviceNoObject;
                            };
                            updateCommand.Parameters.Add(CreateSqlParameter("BerthOutDeviceNo", berthOutDeviceNoFunc()));
                            #endregion

                            var timeOfCreate = string.IsNullOrEmpty(timeOfBerthOut)
                                                   ? DateTime.Parse(timeOfBerthIn)
                                                   : DateTime.Parse(timeOfBerthOut);
                            updateCommand.Parameters.Add(CreateSqlParameter("CreateTime", timeOfCreate));
                            updateCommand.Parameters.Add(CreateSqlParameter("LastModifyDateTime", DateTime.Now));
                            updateCommand.Parameters.Add(CreateSqlParameter("RecordStatus", 0));
                            updateCommand.Parameters.Add(CreateSqlParameter("Id", sqlDataReader["Id"]));

                            updateCommand.ExecuteNonQuery();
                            #endregion
                            connOfUpdate.Close();
                            updateRows++;
                            logger.InfoFormat("第{0}行记录被更新，总更新{1}", rowNum, updateRows);

                        }
                        else
                        {
                            throw new ArgumentException(string.Format(" No='{0}' and berthNo='{1}'的条件存在{2}条记录，放弃更新", recordNo, berthNo, count));
                        }
                    }
                    catch (Exception selectEx)
                    {
                        rowsOfError.Add(rowNum);
                        errorRows++;
                        logger.ErrorFormat("第【{0}】行处理时异常：{1}", rowNum, selectEx);
                    }
                    finally
                    {
                        if (sqlDataReader != null)
                            sqlDataReader.Close();
                        if (connOfInsert != null)
                            connOfInsert.Close();
                        if (connOfUpdate != null)
                            connOfUpdate.Close();
                    }
                    rowNum++;
                }
                mainSqlConnection.Close();
                logger.InfoFormat("总行数{2}，总共执行{3}行,总共更新{0},新增{1},出错：{4}", updateRows, insertRows, dtImport.Rows.Count, rowNum - 1, errorRows);
                logger.Info("新增行号为：" + string.Join(",", rowsOfInsert));
                logger.Info("出错行号为：" + string.Join(",", rowsOfError));
                logger.Info("文件：" + file + "同步完成");
                Console.ReadLine();
            }
        }

        private static SqlParameter CreateSqlParameter(string name, object value)
        {
            return new SqlParameter(name, value);
        }

        private static int CovnertBerthInType(string status)
        {
            if (string.IsNullOrEmpty(status))
                return 1;
            switch (status)
            {
                case "0":
                case "3":
                case "1":
                case "6":
                    return 1;
                case "4":
                case "7":
                case "8":
                    return 2;
            }
            return 1;
        }
        private static int CovnertBerthOutType(string status)
        {
            if (string.IsNullOrEmpty(status))
                return 1;
            switch (status)
            {
                case "0":
                case "3":
                case "1":
                case "6":
                    return 0;
                case "5":
                case "2":
                    return 2;
            }
            return 0;
        }

        private static SqlConnection GetNewConnection()
        {
            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            var conn = new SqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}
