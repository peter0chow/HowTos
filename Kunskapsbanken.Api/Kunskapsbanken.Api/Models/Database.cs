using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Kunskapsbanken.Api.Models
{
    public class Database
    {
        private string connectionString = "user id=PeterChau;" +
                           "password=Tomat123;server=localhost\\SQLEXPRESS;" +
                           "Trusted_Connection=yes;" +
                           "database=Kunskapsbanken;" +
                           "connection timeout=30";

        public string GetFullName(string user)
        {
            string queryString = "SELECT CONCAT(FirstName, ' ', LastName) AS [FullName] FROM Users WHERE UserId LIKE '" + user+"'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    string fullName = "";
                    fullName = reader.GetString(reader.GetOrdinal("FullName"));                    
                    connection.Close();
                    return fullName;
                }
            }

            catch 
            {
                return user;
            }
            
        }


        public string GetDepartment(int id)
        {
            string queryString = "SELECT * FROM Departments WHERE DepartmentId LIKE '" + id + "'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    string fullName = "";
                    fullName = reader.GetString(reader.GetOrdinal("Name"));
                    connection.Close();
                    return fullName;
                }
            }

            catch (Exception e)
            {
                return e.Message;
            }

        }



        public List<HowTo> SqlGetAllHowTos()
        {
            //string queryString = "SELECT X.[HowToId], X.[Created], X.[Title], X.[Description], CONCAT(U.FirstName, ' ', U.LastName) AS [CreatedBy], X.[Name] AS [Department] FROM (SELECT * FROM AllHowTos A LEFT JOIN Departments B ON A.Department = B.DepartmentId) AS X LEFT JOIN Users AS U ON X.CreatedBy = U.UserId";
            string queryString = "SELECT * FROM AllHowTos";

            List<HowTo> howToList = new List<HowTo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    HowTo howTo = new HowTo();
                    howTo.Id = reader.GetInt32(reader.GetOrdinal("HowToId"));
                    howTo.Created = reader.GetDateTime(reader.GetOrdinal("Created"));
                    howTo.Title = reader.GetString(reader.GetOrdinal("Title"));
                    howTo.Description = reader.GetString(reader.GetOrdinal("Description"));
                    howTo.CreatedBy = GetFullName(reader.GetString(reader.GetOrdinal("CreatedBy")));
                    howTo.Department = GetDepartment(reader.GetInt32(reader.GetOrdinal("Department")));
                    howToList.Add(howTo);
                }
                connection.Close();

                return howToList;
            }

        }



        public HowTo SqlGetHowToById(int id)
        {
            return SqlGetAllHowTos().Where(x => x.Id == id).FirstOrDefault();
        }



        public void SqlAddHowTo(HowTo howTo)
        {
            string queryString = "INSERT INTO [dbo].[AllHowTos] ([Title], [Description], [CreatedBy], [Department]) VALUES (@title, @description, @createdBy, @department);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.CommandText = queryString;

                SqlParameter titleParam = new SqlParameter("@title", howTo.Title);
                SqlParameter descriptionParam = new SqlParameter("@description", howTo.Description);
                SqlParameter createdByParam = new SqlParameter("@createdBy", howTo.CreatedBy);
                SqlParameter departmentParam = new SqlParameter("@department", howTo.Department);

                command.Parameters.AddRange(new SqlParameter[] { titleParam, descriptionParam, createdByParam, departmentParam });
                command.ExecuteNonQuery();

                connection.Close();
            }

        }



        
        public void SqlUpdateHowTo(HowTo howTo)
        {
            string queryString = "UPDATE AllHowTos SET Title=@title, Description=@description, CreatedBy=@createdBy, Department=@department WHERE HowToId=" + howTo.Id;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.CommandText = queryString;

                SqlParameter titleParam = new SqlParameter("@title", howTo.Title);
                SqlParameter descriptionParam = new SqlParameter("@description", howTo.Description);
                SqlParameter createdByParam = new SqlParameter("@createdBy", howTo.CreatedBy);
                SqlParameter departmentParam = new SqlParameter("@department", howTo.Department);
                
                command.Parameters.AddRange(new SqlParameter[] { titleParam, descriptionParam, createdByParam, departmentParam });
                command.ExecuteNonQuery();

                connection.Close();
            }

        }




        public void SqlDeleteHowTo(int id)
        {

            string queryString = "DELETE FROM dbo.AllHowTos WHERE HowToId=@id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.CommandText = queryString;

                SqlParameter idParam = new SqlParameter("@id", id);

                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

                connection.Close();
            }

        }





        public List<HowTo> SqlSearchAllHowTos(string search)
        {
            string[] words = search.Split('_');

            string queryString = "SELECT * FROM dbo.AllHowTos WHERE Description LIKE '%" + words[0] + "%'";

            for (int i = 1; i < words.Count(); i++)
            {
                queryString = queryString + " AND Description LIKE '%" + words[i] + "%'";
            }

            List<HowTo> howToList = new List<HowTo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    HowTo howTo = new HowTo();
                    howTo.Id = reader.GetInt32(reader.GetOrdinal("HowToId"));
                    howTo.Created = reader.GetDateTime(reader.GetOrdinal("Created"));
                    howTo.Title = reader.GetString(reader.GetOrdinal("Title"));
                    howTo.Description = reader.GetString(reader.GetOrdinal("Description"));
                    howTo.CreatedBy = GetFullName(reader.GetString(reader.GetOrdinal("CreatedBy")));
                    howTo.Department = GetDepartment(reader.GetInt32(reader.GetOrdinal("Department")));

                    howToList.Add(howTo);
                }
                connection.Close();
                                
                return howToList;
            }

        }






    }
}