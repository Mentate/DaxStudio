﻿using ADOTabular.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ADOTabular.AdomdClientWrappers
{
    public sealed class AdomdDataReader : System.Data.IDataReader
    {
        private readonly Microsoft.AnalysisServices.AdomdClient.AdomdDataReader _obj;

        public AdomdDataReader() { }
        public AdomdDataReader(Microsoft.AnalysisServices.AdomdClient.AdomdDataReader dataReader)
        {
            _obj = dataReader;         
        }

        public bool Read()
        {
            return _obj.Read();
        }

        public bool NextResult()
        {
            return _obj.NextResult();
        }



        public void Close()
        {
            _obj.Close();
        }

        public int Depth => _obj.Depth;
        

        public DataTable GetSchemaTable()
        {
            return _obj.GetSchemaTable();
        }

        public bool IsClosed => _obj.IsClosed;

        public int RecordsAffected => _obj.RecordsAffected;

        public void Dispose()
        {
            _obj.Dispose();
        }

        public int FieldCount => _obj.FieldCount;

        public ADOTabularConnection Connection { get; internal set; }
        public string CommandText { get; internal set; }

        public bool GetBoolean(int i) =>  _obj.GetBoolean(i);

        public byte GetByte(int i) => _obj.GetByte(i);

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)  => _obj.GetBytes(i,fieldOffset,buffer,bufferoffset,length);
          

        public char GetChar(int i) =>  _obj.GetChar(i);

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) =>  _obj.GetChars(i, fieldoffset,buffer,bufferoffset, length);
            
        public IDataReader GetData(int i) => _obj.GetData(i);
            
        public string GetDataTypeName(int i) => _obj.GetDataTypeName(i);
            
        public DateTime GetDateTime(int i) => _obj.GetDateTime(i);

        public decimal GetDecimal(int i) => _obj.GetDecimal(i);
            
        public double GetDouble(int i) => _obj.GetDouble(i);
            
        public Type GetFieldType(int i) => _obj.GetFieldType(i);
            
        public bool IsDataReader(int i) 
        {
            return GetFieldType(i).Name == "XmlaDataReader" ;
        }

        public string GetDataReaderValue(int i) => _obj.GetValue(i).ToString();
            

        public float GetFloat(int i) => _obj.GetFloat(i);

        public Guid GetGuid(int i) => _obj.GetGuid(i);
            
        public short GetInt16(int i) => _obj.GetInt16(i);
            
        public int GetInt32(int i) => _obj.GetInt32(i);
            
        public long GetInt64(int i) => _obj.GetInt64(i);

        public string GetName(int i) => _obj.GetName(i);
            
        public int GetOrdinal(string name) => _obj.GetOrdinal(name);
            
        public string GetString(int i) => _obj.GetString(i);
            
        public object GetValue(int i) => _obj.GetValue(i);
            
        public int GetValues(object[] values) => _obj.GetValues(values);
            
        public bool IsDBNull(int i) => _obj.IsDBNull(i);
            
        public object this[string name] => _obj[name];
            
        public object this[int i] => _obj[i];

        public DataTable ConvertToTable(Dictionary<string,string> formats)
        {
            return ConvertToTable(0, formats);
        }

        public DataTable ConvertToTable(long maxRows)
        {
            return ConvertToTable(maxRows, null);
        }

        public DataTable ConvertToTable()
        {
            return ConvertToTable(0, null);
        }

        public DataTable ConvertToTable(long maxRows, Dictionary<string,string> formats)
        {
            if( maxRows == 0)  maxRows = long.MaxValue;
            long rowCnt = 0;
            
            DataTable dtSchema = this.GetSchemaTable();
            DataTable dt = new DataTable();
            List<DataColumn> listCols = new List<DataColumn>();
            var invariantCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (dtSchema != null)
            {
                foreach (DataRow drow in dtSchema.Rows)
                {
                    string columnName = drow["ColumnName"].ToString();
                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    //column.Unique = (bool)drow["IsUnique"];
                    //column.AllowDBNull = (bool)drow["AllowDBNull"];
                    //column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                    if (formats?.ContainsKey(columnName)??false) column.ExtendedProperties.Add("FormatString", string.Format(invariantCulture, "{{0:{0}}}" + formats[columnName]));
                    listCols.Add(column);
                    dt.Columns.Add(column);
                }
            }

            // Read rows from DataReader and populate the DataTable
            while (this.Read())
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    if (listCols[i].ExtendedProperties.ContainsKey("FormatString"))
                        dataRow[((DataColumn)listCols[i])] = string.Format(invariantCulture, listCols[i].ExtendedProperties["FormatString"].ToString() , this[i]);
                    else
                        dataRow[((DataColumn)listCols[i])] = this[i];
                }
                dt.Rows.Add(dataRow);
                rowCnt++;
                if (rowCnt > maxRows) break;
            }
            return dt;  

        }

    }
}
