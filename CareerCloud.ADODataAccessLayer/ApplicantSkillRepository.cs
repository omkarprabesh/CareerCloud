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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {
        public void Add(params ApplicantSkillPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (ApplicantSkillPoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[Applicant_Skills]
           ([Id]
           ,[Applicant]
           ,[Skill]
           ,[Skill_Level]
           ,[Start_Month]
           ,[Start_Year]
           ,[End_Month]
           ,[End_Year])
     VALUES
           (@Id
           ,@Applicant
           ,@Skill
           ,@Skill_Level
           ,@Start_Month
           ,@Start_Year
           ,@End_Month
           ,@End_Year)";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Applicant", poco.Applicant);
                cmd.Parameters.AddWithValue("Skill", poco.Skill);
                cmd.Parameters.AddWithValue("Skill_Level", poco.SkillLevel);
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

        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Applicant_Skills";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            ApplicantSkillPoco[] pocos = new ApplicantSkillPoco[1000];


            int cnt = 0;
            while (rdr.Read())
            {
                ApplicantSkillPoco poco = new ApplicantSkillPoco();
                poco.Id = rdr.GetGuid(0);
                poco.Applicant = rdr.GetGuid(1);
                poco.Skill = rdr.GetString(2);
                poco.SkillLevel = rdr.GetString(3); 
                poco.StartMonth = rdr.GetByte(4);
                poco.StartYear = (int) rdr.GetSqlInt32(5);
                poco.EndMonth = rdr.GetByte(6);
                poco.EndYear= (int)rdr.GetSqlInt32(7); ;
                poco.TimeStamp = (byte[])rdr.GetSqlBinary(8);

                cnt++;
                pocos[cnt] = poco;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantSkillPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantSkillPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (ApplicantSkillPoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Applicant_Skills]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params ApplicantSkillPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (ApplicantSkillPoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Applicant_Skills]
                       SET [Applicant] = @Applicant
                          ,[Skill] = @Skill
                          ,[Skill_Level] = @Skill_Level
                          ,[Start_Month] = @Start_Month
                          ,[Start_Year] = @Start_Year
                          ,[End_Month] = @End_Month
                          ,[End_Year] = @End_Year
                     WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Applicant", poco.Applicant);
                cmd.Parameters.AddWithValue("Skill", poco.Skill);
                cmd.Parameters.AddWithValue("Skill_Level", poco.SkillLevel);
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
   