# ADO.NET-Data-Access

## Classes in ADO.NET

##### Connected Classes:

Connection > Creates a connection to specified database

Transaction > Executes commands within a transaction

Command > SQL command to send to Database

DataAdapter > Fill a dataset/datatable with data

DataReader > A fast , forward-only cursor to read data

##### Disconnected Classes:

DataSet > Collection of one or many Datatables

DataTable > A Single Table of data that includes rows and columns

DataView > Allows to get a view into the Datatable and filter or sort the data

DataRow > A Single row of Data in DataTable

DataColumn > A Single column of Data in a DataRow

##### Builder Classes:

ConnectionStringBuilder > Create or break apart a connection string

CommandBuilder > Create an insert,update or delete command

##### Providers

SQLServer > System.Data.SqlClient ( SqlConnection,SqlCommand,SqlDataAdapter,.. )

OLE DB > System.Data.OleDb ( OleDbConnection,OleDbCommand,OleDbDataAdapter,.. )

ODBC > System.Data.Odbc( OdbcConnection,OdbcCommand,OdbcDataAdapter,.. )

Oracle(recommanded to use the one provided by oracle) > System.Data.OracleClient( OracleConnection,OracleCommand,OracleDataAdapter,.. )

# Wrapper Classes

https://bit.ly/2MrlBRl or https://www.pdsa.com/blog?id=115 or https://www.pdsa.com/Resources-BlogPosts/15-DataReaderWrapper.pdf

https://bit.ly/2MTC7sh or https://www.pdsa.com/blog?id=116 or https://www.pdsa.com/Resources-BlogPosts/16-DataWrapperSample.pdf
