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
    public class SecurityLoginRepository : IDataRepository<SecurityLoginPoco>
    {
        public void Add(params SecurityLoginPoco[] items)
        {


            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (SecurityLoginPoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Logins]
           ([Id]
           ,[Login]
           ,[Password]
           ,[Created_Date]
            ,[Password_Update_Date]
            ,[Agreement_Accepted_Date]
            ,[Is_Locked]
            ,[Is_Inactive]
            ,[Email_Address]
            ,[Phone_Number]
            ,[Full_Name]
            ,[Force_Change_Password]
            ,[Prefferred_Language]
            )
     VALUES
           (@Id
           ,@Login
           ,@Password
           ,@CD
            ,@PUD
            ,@AAD
             ,@IL
             ,@IS
              ,@EA
            ,@PN
            ,@FN
            ,@FCP
            ,@PL)";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Login", poco.Login);
                cmd.Parameters.AddWithValue("Password", poco.Password);
                cmd.Parameters.AddWithValue("CD", poco.Created);
                cmd.Parameters.AddWithValue("PUD", poco.PasswordUpdate);
                cmd.Parameters.AddWithValue("AAD", poco.AgreementAccepted);
                cmd.Parameters.AddWithValue("IL", poco.IsLocked);
                cmd.Parameters.AddWithValue("IS", poco.IsInactive);
                cmd.Parameters.AddWithValue("EA", poco.EmailAddress);
                cmd.Parameters.AddWithValue("PN", poco.PhoneNumber);
                cmd.Parameters.AddWithValue("FN", poco.FullName);
                cmd.Parameters.AddWithValue("FCP", poco.ForceChangePassword);
                cmd.Parameters.AddWithValue("PL", poco.PrefferredLanguage);
                cmd.ExecuteNonQuery();

            }

            conn.Close();
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Security_Logins";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            SecurityLoginPoco[] pocos = new SecurityLoginPoco[2000];


            int cnt = 0;
            while (rdr.Read())
            {
                SecurityLoginPoco poco = new SecurityLoginPoco();
                poco.Id = rdr.GetGuid(0);
                poco.Login = rdr.GetString(1);
                poco.Password = rdr.GetString(2);
                poco.Created = rdr.GetDateTime(3);
                poco.PasswordUpdate = rdr.IsDBNull(4) ? null : (DateTime?)rdr.GetDateTime(4);
                poco.AgreementAccepted = rdr.IsDBNull(5) ? null : (DateTime?)rdr.GetDateTime(5);
                poco.IsLocked = rdr.GetBoolean(6);
                poco.IsInactive = rdr.GetBoolean(7);
                poco.EmailAddress = rdr.GetString(8);
                poco.PhoneNumber = rdr.IsDBNull(9)? null:rdr.GetString(9);
                poco.FullName = rdr.IsDBNull(10) ? null : rdr.GetString(10);
                poco.ForceChangePassword = rdr.GetBoolean(11);
                poco.PrefferredLanguage = rdr.IsDBNull(12) ? null : rdr.GetString(12);
                poco.TimeStamp = (byte[])rdr.GetSqlBinary(13);
                pocos[cnt] = poco;
                cnt++;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<SecurityLoginPoco> GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SecurityLoginPoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Security_Logins]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SecurityLoginPoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Security_Logins]
                       SET [Login] = @Login
                          ,[Password] = @Password
                          ,[Created_Date] = @CD
                          ,[Password_Update_Date]= @PUD
                           ,[Agreement_Accepted_Date]= @AAD
                            ,[Is_Locked]= @IL
                            ,[Is_Inactive]= @IS
                        ,[Email_Address]= @EA
                    ,[Phone_Number]= @PN
                        ,[Full_Name]= @FN
                        ,[Force_Change_Password]= @FCP
                        ,[Prefferred_Language]= @PL
                            
                            
                     WHERE Id= @Id";
                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Login", poco.Login);
                cmd.Parameters.AddWithValue("Password", poco.Password);
                cmd.Parameters.AddWithValue("CD", poco.Created);
                cmd.Parameters.AddWithValue("PUD", poco.PasswordUpdate);
                cmd.Parameters.AddWithValue("AAD", poco.AgreementAccepted);
                cmd.Parameters.AddWithValue("IL", poco.IsLocked);
                cmd.Parameters.AddWithValue("IS", poco.IsInactive);
                cmd.Parameters.AddWithValue("EA", poco.EmailAddress);
                cmd.Parameters.AddWithValue("PN", poco.PhoneNumber);
                cmd.Parameters.AddWithValue("FN", poco.FullName);
                cmd.Parameters.AddWithValue("FCP", poco.ForceChangePassword);
                cmd.Parameters.AddWithValue("PL", poco.PrefferredLanguage);
                

                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
