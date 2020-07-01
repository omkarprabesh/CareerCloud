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
    public class CompanyJobDescriptionRepository : IDataRepository<CompanyJobDescriptionPoco>
    {
        public void Add(params CompanyJobDescriptionPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (CompanyJobDescriptionPoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[Company_Jobs_Descriptions]
           ([Id]
           ,[Job]
           ,[Job_Name]
           ,[Job_Descriptions] 
           )
     VALUES
           (@Id
           ,@Job
           ,@Job_Name
           ,@Job_Descriptions
           )";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Job", poco.Job);
                cmd.Parameters.AddWithValue("Job_Name", poco.JobName);
                cmd.Parameters.AddWithValue("Job_Descriptions", poco.JobDescriptions);

                cmd.ExecuteNonQuery();

            }

            conn.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyJobDescriptionPoco> GetAll(params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Company_Jobs_Descriptions";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            CompanyJobDescriptionPoco[] pocos = new CompanyJobDescriptionPoco[2000];


            int cnt = 0;
            while (rdr.Read())
            {
                CompanyJobDescriptionPoco poco = new CompanyJobDescriptionPoco();
                poco.Id = rdr.GetGuid(0);
                poco.Job = rdr.GetGuid(1);
                poco.JobName = rdr.IsDBNull(2)? null: rdr.GetString(2); 
                poco.JobDescriptions= rdr.IsDBNull(3) ? null : rdr.GetString(3);
                poco.TimeStamp = (byte[])rdr.GetSqlBinary(4);

                
                pocos[cnt] = poco;
                cnt++;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<CompanyJobDescriptionPoco> GetList(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobDescriptionPoco GetSingle(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobDescriptionPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobDescriptionPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (CompanyJobDescriptionPoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Company_Jobs_Descriptions]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params CompanyJobDescriptionPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (CompanyJobDescriptionPoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Company_Jobs_Descriptions]
                       SET [Job] = @Job
                          ,[Job_Name] = @Job_Name
                          ,[Job_Descriptions] = @Job_Descriptions
                          
                                                  
                     WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Job", poco.Job);
                cmd.Parameters.AddWithValue("Job_Name", poco.JobName);
                cmd.Parameters.AddWithValue("Job_Descriptions", poco.JobDescriptions);
                
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
