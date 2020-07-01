using ArraySizeTester;
using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
    public class SystemLanguageCodeRepository : IDataRepository<SystemLanguageCodePoco>
    {
      

    public void Add(params SystemLanguageCodePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (SystemLanguageCodePoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[System_Language_Codes]
           ([LanguageID]
           ,[Name]
           ,[Native_Name]
           )
     VALUES
           (@LanguageID
           ,@Name
           ,@Native_Name
           )";

                cmd.Parameters.AddWithValue("LanguageID", poco.LanguageID);
                cmd.Parameters.AddWithValue("Name", poco.Name);
                cmd.Parameters.AddWithValue("Native_Name", poco.NativeName);
                cmd.ExecuteNonQuery();

            }

            conn.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }





        public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)

        {
            SqlConnection conn1 = new SqlConnection(@"Data Source=OMKARKANDEL\HUMBERBRIDGING;Initial Catalog=JOB_PORTAL_DB;Integrated Security=True");
            SqlCommand cmd1 = new SqlCommand();


            cmd1.Connection = conn1;
            cmd1.CommandText = @"select count (*) from System_Language_Codes";
            conn1.Open();
            int ListCount = (int)cmd1.ExecuteScalar();
            //Console.WriteLine(ListCount);
            conn1.Close();

            using (SqlConnection conn = new SqlConnection(BaseAdo.connectionString)) 
            {
                
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select * from System_Language_Codes";
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    SystemLanguageCodePoco[] pocos = new SystemLanguageCodePoco[ListCount];

                //try { 
                    int cnt = 0;
                    while (rdr.Read())
                    {
                        SystemLanguageCodePoco poco = new SystemLanguageCodePoco();
                        poco.LanguageID = rdr.GetString(0);
                        poco.Name = rdr.GetString(1);
                        poco.NativeName = rdr.GetString(2);
                        pocos[cnt] = poco;
                        cnt++;
                    }
                //}

                //catch (Exception e)
                //{
                //    Console.WriteLine("Exception= {0}", e.StackTrace);
                //}
               // rdr.Close();
                //conn.Close();

                return pocos.Where(p => p != null).ToList();
            }
        }


        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemLanguageCodePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SystemLanguageCodePoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[System_Language_Codes]
                        WHERE LanguageID= @ID";

                cmd.Parameters.AddWithValue("ID", poco.LanguageID);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SystemLanguageCodePoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[System_Language_Codes]
                       SET [Name] = @Name
                          ,[Native_Name] = @NativeName
                                   
                     WHERE LanguageID= @LanguageID";

                cmd.Parameters.AddWithValue("LanguageID", poco.LanguageID);
                cmd.Parameters.AddWithValue("Name", poco.Name);
                cmd.Parameters.AddWithValue("NativeName", poco.NativeName);

                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
    
}
