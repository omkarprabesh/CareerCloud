using System;
using System.Data.SqlClient;

namespace ArraySizeTester
{
     public class GetArraySize
        {
            public static int ListCount;
            public static void Main ()
            {
                SqlConnection conn1 = new SqlConnection(@"Data Source=OMKARKANDEL\HUMBERBRIDGING;Initial Catalog=JOB_PORTAL_DB;Integrated Security=True");
                SqlCommand cmd1 = new SqlCommand();


                cmd1.Connection = conn1;
                cmd1.CommandText = @"select count (*) from System_Language_Codes";
                conn1.Open();
                ListCount = (int)cmd1.ExecuteScalar();
            //Console.WriteLine(ListCount);
            conn1.Close();


            }
        }
}
