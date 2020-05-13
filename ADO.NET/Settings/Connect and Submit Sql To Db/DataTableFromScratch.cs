using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public class DataTableFromScratch
    {
        public DataTable BuildDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            //*********************
            //*** Creating Columns
            //*********************
            // Method 1: Use Add() method to build column
            dt.Columns.Add("ProductId", typeof(int));

            // Method 2: Create a DataColumn object for more control
            dc = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ProductName",
                Caption = "Product Name",
                ReadOnly = false
            };
            dt.Columns.Add(dc);

            // Method 3: Create the DataColumn object in Add() method
            dt.Columns.Add(new DataColumn
            {
                DataType = typeof(decimal),
                ColumnName = "Price",
                Caption = "Price",
                ReadOnly = false
            });

            //*********************
            //*** Adding Rows
            //*********************
            // Method 1: Pass in variable amount of arguments to Add() method
            // Pass data in the same order as the columns you created
            dt.Rows.Add(1, "VB.NET Fundamentals", 9.99D);

            // Method 2: Create a new DataRow
            DataRow dr = dt.NewRow();
            // Set each column by name
            dr["ProductId"] = 2;
            dr["ProductName"] = "XML Fundamentals in C#";
            dr["Price"] = 19D;
            // Add DataRow to Rows collection
            dt.Rows.Add(dr);

            // After you are done adding all rows, call AcceptChanges
            dt.AcceptChanges();

            return dt;
        }

        public DataTable CloneDataTable()
        {
            DataTable dt = BuildDataTable();

            // Clone just the structure of the DataTable, no data
            DataTable dtNew = dt.Clone();

            return dtNew;
        }

        public DataTable CopyDataTable()
        {
            DataTable dt = BuildDataTable();

            // Clone the structure and copy all data
            DataTable dtNew = dt.Copy();

            return dtNew;
        }

        public DataTable SelectCopyRowByRow()
        {
            DataTable dt;
            DataTable dtNew;
            DataTables dts = new DataTables();

            // Get Products from Table
            dt = dts.GetCountriesAsDataTable();

            // Clone structure into new DataTable
            dtNew = dt.Clone();

            // Select Rows from DataTableObject
            DataRow[] rows = dt.Select("CountryId < 20");

            // Loop through array of rows
            foreach (DataRow row in rows)
            {
                // NOTE: The following causes an error 
                //       A single row cannot belong to more than one data table
                // dtNew.Rows.Add(row); 

                // Method 1: Use ItemArray to avoid the error
                //dtNew.Rows.Add(row.ItemArray);

                // Method 2: Use ImportRow to avoid error
                dtNew.ImportRow(row);
            }
            dtNew.AcceptChanges();

            return dtNew;
        }

        //instead of SelectCopyRowByRow()
        public DataTable SelectUsingCopyToDataTable()
        {
            DataTable dt;
            DataTable dtNew;
            DataTables dts = new DataTables();

            // Get Products from Table
            dt = dts.GetCountriesAsDataTable();

            // Select Rows from DataTableObject and Copy to New DataTable
            dtNew = dt.Select("CountryId < 20").CopyToDataTable();

            return dtNew;
        }

        public void PrintDataTable(DataTable dt)
        {
            int index = 0;
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine("** Row: " + index.ToString() + " **");
                foreach (DataColumn col in dt.Columns)
                {
                    Console.WriteLine(col.ColumnName + ": " + row[col.ColumnName].ToString());
                    index++;
                }
                Console.WriteLine();
            }
        }

    }
}
