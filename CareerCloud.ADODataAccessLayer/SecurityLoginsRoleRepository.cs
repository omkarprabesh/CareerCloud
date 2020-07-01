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
    public class SecurityLoginsRoleRepository : IDataRepository<SecurityLoginsRolePoco>
    {
        public void Add(params SecurityLoginsRolePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            conn.Open();
            foreach (SecurityLoginsRolePoco poco in items)
            {
                cmd.CommandText = @"INSERT INTO [dbo].[Security_Logins_Roles]
           ([Id]
           ,[Login]
           ,[Role]
           )
     VALUES
           (@Id
           ,@Login
           ,@Role
           )";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Login", poco.Login);
                cmd.Parameters.AddWithValue("Role", poco.Role);
                cmd.ExecuteNonQuery();

            }

            conn.Close();
        }
            public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsRolePoco> GetAll(params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"select * from Security_Logins_Roles";
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            SecurityLoginsRolePoco[] pocos = new SecurityLoginsRolePoco[1000];


            int cnt = 0;
            while (rdr.Read())
            {
                SecurityLoginsRolePoco poco = new SecurityLoginsRolePoco();
                poco.Id = rdr.GetGuid(0);
                poco.Login = rdr.GetGuid(1);
                poco.Role = rdr.GetGuid(2);
                poco.TimeStamp = (byte[])rdr.GetSqlBinary(3);
                pocos[cnt] = poco;
                cnt++;
            }
            rdr.Close();
            conn.Close();
            return pocos.Where(p => p != null).ToList();
        }

        public IList<SecurityLoginsRolePoco> GetList(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsRolePoco GetSingle(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsRolePoco> pocos = GetAll().AsQueryable();
            return pocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsRolePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SecurityLoginsRolePoco poco in items)
            {
                cmd.CommandText = @"DELETE FROM [dbo].[Security_Logins_Roles]
                        WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);

                cmd.ExecuteNonQuery();

            }
            conn.Close();
        }

        public void Update(params SecurityLoginsRolePoco[] items)
        {
            SqlConnection conn = new SqlConnection(BaseAdo.connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            foreach (SecurityLoginsRolePoco poco in items)
            {
                cmd.CommandText = @"UPDATE [dbo].[Security_Logins_Roles]
                       SET [Login] = @Login
                          ,[Role] = @Role
                                   
                     WHERE Id= @Id";

                cmd.Parameters.AddWithValue("Id", poco.Id);
                cmd.Parameters.AddWithValue("Login", poco.Login);
                cmd.Parameters.AddWithValue("Role", poco.Role);
                
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}
