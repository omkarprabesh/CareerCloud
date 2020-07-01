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
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (ApplicantWorkHistoryPoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Work_History]
           ([Id]
           ,[Applicant]
           ,[Company_Name]
           ,[Country_Code]
           ,[Location]
           ,[Job_Title]
           ,[Job_Description]
           ,[Start_Month]
           ,[Start_Year]
           ,[End_Month]
           ,[End_Year])
     VALUES
           (@Id
           ,@Applicant
           ,@Company_Name
           ,@Country_Code
           ,@Location
           ,@Job_Title
           ,@Job_Description
           ,@Start_Month
           ,@Start_Year
           ,@End_Month
           ,@End_Year)";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Applicant", poco.Applicant);
                cmd.Parameters.AddWithValue("Company_Name", poco.CompanyName);
                cmd.Parameters.AddWithValue("Country_Code", poco.CountryCode);
                cmd.Parameters.AddWithValue("Location", poco.Location);
                cmd.Parameters.AddWithValue("Job_Title", poco.JobTitle);
                cmd.Parameters.AddWithValue("Job_Description", poco.JobDescription);
                cmd.Parameters.AddWithValue("Start_Month", poco.StartMonth);
                cmd.Parameters.AddWithValue("Start_Year", poco.StartYear);
                cmd.Parameters.AddWithValue("End_Month", poco.EndMonth);
                cmd.Parameters.AddWithValue("End_Year", poco.EndYear);

                cmd.ExecuteNonQuery();

            }

            conn.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Applicant_Work_History";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            ApplicantWorkHistoryPoco[] pocos = new ApplicantWorkHistoryPoco[1000];


            int cnt = 0;
            while (rdr.Read())
            {
                ApplicantWorkHistoryPoco poco = new ApplicantWorkHistoryPoco();
                poco.Id = rdr.GetGuid(0);
                poco.Applicant = rdr.GetGuid(1);
                poco.CompanyName = rdr.GetString(2);
                poco.CountryCode = rdr.GetString(3);
                poco.Location = rdr.GetString(4);
                poco.JobTitle = rdr.GetString(5);
                poco.JobDescription = rdr.GetString(6);
                poco.StartMonth = (short)rdr.GetSqlInt16(7); ;
                poco.StartYear = (int)rdr.GetSqlInt32(8);
                poco.EndMonth = (short)rdr.GetSqlInt16(9);
                poco.EndYear = (int)rdr.GetSqlInt32(10);
                poco.TimeStamp= (byte[])rdr.GetSqlBinary(11);


                cnt++;
                pocos[cnt] = poco;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantWorkHistoryPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (ApplicantWorkHistoryPoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Applicant_Work_History]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (ApplicantWorkHistoryPoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Work_History]
                       SET [Applicant] = @Applicant
                          ,[Company_Name] = @Company_Name
                          ,[Country_Code] = @Country_Code
                          ,[Location] = @Location
                          ,[Job_Title] = @Job_Title
                          ,[Job_Description] = @Job_Description
                          ,[Start_Month] = @Start_month
                          ,[Start_Year]= @Start_Year
                           ,[End_Month]= @End_Month
                          ,[End_Year]= @End_Year                          
                     WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Applicant", poco.Applicant);
                cmd.Parameters.AddWithValue("Company_Name", poco.CompanyName);
                cmd.Parameters.AddWithValue("Country_Code", poco.CountryCode);
                cmd.Parameters.AddWithValue("Location", poco.Location);
                cmd.Parameters.AddWithValue("Job_Title", poco.JobTitle);
                cmd.Parameters.AddWithValue("Job_Description", poco.JobDescription);
                cmd.Parameters.AddWithValue("Start_Month", poco.StartMonth);
                cmd.Parameters.AddWithValue("Start_Year", poco.StartYear);
                cmd.Parameters.AddWithValue("End_Month", poco.EndMonth);
                cmd.Parameters.AddWithValue("End_Year", poco.EndYear);

                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
