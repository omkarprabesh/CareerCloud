using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
    public class CompanyProfileRepository : IDataRepository<CompanyProfilePoco>
    {
        public void Add(params CompanyProfilePoco[] items)
        {
          
            
                SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                conn.Open();
                foreach (CompanyProfilePoco poco in items)
                {
                    cmd.CommandText = @"INSERT INTO [dbo].[Company_Profiles]
           ([Id]
           ,[Registration_Date]
           ,[Company_Website]
           ,[Contact_Phone]
            ,[Contact_Name]
            ,[Company_Logo]
            
            )
     VALUES
           (@Id
           ,@RD
           ,@CW
           ,@CP
            ,@CN
            ,@CL
             
           )";

                    cmd.Parameters.AddWithValue("Id", poco.Id);
                    cmd.Parameters.AddWithValue("RD", poco.RegistrationDate);
                    cmd.Parameters.AddWithValue("CW", poco.CompanyWebsite);
                    cmd.Parameters.AddWithValue("CP", poco.ContactPhone);
                    cmd.Parameters.AddWithValue("CN", poco.ContactName);
                    cmd.Parameters.AddWithValue("CL", poco.CompanyLogo);
                    
                    cmd.ExecuteNonQuery();

                }

                conn.Close();
            }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Company_Profiles";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            CompanyProfilePoco[] pocos = new CompanyProfilePoco[1000];


            int cnt = 0;
            while (rdr.Read())
            {
                CompanyProfilePoco poco = new CompanyProfilePoco();
                poco.Id = rdr.GetGuid(0);
                poco.RegistrationDate = rdr.GetDateTime(1);
                poco.CompanyWebsite = rdr.IsDBNull(2)?null:rdr.GetString(2);
                poco.ContactPhone = rdr.GetString(3);
                poco.ContactName = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                poco.CompanyLogo = rdr.IsDBNull(5)? null: (byte[])rdr.GetSqlBinary(5);
                poco.TimeStamp = (byte[])rdr.GetSqlBinary(6);
                pocos[cnt] = poco;
                cnt++;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<CompanyProfilePoco> GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyProfilePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (CompanyProfilePoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Company_Profiles]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params CompanyProfilePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (CompanyProfilePoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Company_Profiles]
                       SET [Registration_Date] = @RD
                          ,[Company_Website] = @CW
                          ,[Contact_Phone] = @CP
                          ,[Contact_Name]= @CN
                           ,[Company_Logo]= @CL
                            
                            
                     WHERE Id= @Id";
                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("RD", poco.RegistrationDate);
                cmd.Parameters.AddWithValue("CW", poco.CompanyWebsite);
                cmd.Parameters.AddWithValue("CP", poco.ContactPhone);
                cmd.Parameters.AddWithValue("CN", poco.ContactName);
                cmd.Parameters.AddWithValue("CL", poco.CompanyLogo);
                
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
