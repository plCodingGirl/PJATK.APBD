using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CW2.Models;

namespace CW2.DAL
{
    public class SqlDbService : IDbService
    {
        public IEnumerable<Student> GetStudents()
        {
            using (var client = new SqlConnection(
                "Data Source=db-mssql;Initial Catalog=s17428;Integrated Security=True"))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = client;
                cmd.CommandText = @"SELECT FirstName, LastName, BirthDate, e.Semester, s.Name
                FROM Student AS student
                JOIN Enrollment AS e ON student.IdEnrollment = e.IdEnrollment
                JOIN Studies AS s ON e.IdStudy = s.IdStudy";

                client.Open();
                var reader = cmd.ExecuteReader();
                var students = new List<Student>();

                while (reader.Read())
                {
                    var student = new Student();
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                    student.BirthDate = Convert.ToDateTime(reader["BirthDate"]);
                    student.StudyName = reader["Name"].ToString();
                    student.SemesterNumber = Convert.ToInt32(reader["Semester"]);
                    students.Add(student);
                }
                return students;
            }
        }

        public Student GetStudentById(string id)
        {
            using (var client = new SqlConnection(
                "Data Source=db-mssql;Initial Catalog=s17428;Integrated Security=True"))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = client;
                cmd.CommandText = @"SELECT FirstName, LastName, BirthDate, e.Semester, s.Name
                  FROM Student AS student
                  JOIN Enrollment AS e ON student.IdEnrollment = e.IdEnrollment
                  JOIN Studies AS s ON e.IdStudy = s.IdStudy
                  WHERE IndexNumber = @id";
                cmd.Parameters.AddWithValue("id", id);

                client.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var student = new Student();
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                    student.BirthDate = Convert.ToDateTime(reader["BirthDate"]);
                    student.StudyName = reader["Name"].ToString();
                    student.SemesterNumber = Convert.ToInt32(reader["Semester"]);
                    return student;
                }

                return null;
            }
        }
    }
}
