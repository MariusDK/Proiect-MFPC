using MySql.Data.MySqlClient;
using ProiectMFPC_V1.src.DataModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace ProiectMFPC_V1.src
{
    class Operation
    {
        private DBConnection dbCon;
        public List<Student> students { get; set; }
        public Student student { get; set; }
        public Profesor profesor { get; set; }
        public Curs curs { get; set; }
        public Operation()
        {
            dbCon = new DBConnection("facultate");
        }
        public void ExecuteMethodGet(string methodName, int cod)
        {
            if (methodName.Equals("GetAllStudents"))
            {
                GetAllStudents();
            }
            if (methodName.Equals("GetStudent"))
            {
                this.student = GetStudent(cod);
            }
            if (methodName.Equals("GetAllProfesors"))
            {
                GetAllProfesors();
            }
            if (methodName.Equals("GetProfesor"))
            {
                profesor = GetProfesor(cod);
            }
            if (methodName.Equals("GetAllCurs"))
            {
                GetAllCurs();
            }
            if (methodName.Equals("GetCurs"))
            {
                curs = GetCursById(cod);
            }
        }
        public void ExecuteMethodInsert(string methodName,Student student,Profesor profesor,Curs curs)
        {
            if (methodName.Equals("InsertStudent"))
            {
                InsertStudent(student);
            }
            if (methodName.Equals("InsertProfesor"))
            {
                InsertProfesor(profesor);
            }
            if (methodName.Equals("InsertCurs"))
            {
                InsertCurs(curs);
            }
        }
        public void ExecuteMethodUpdate(string methodName, int cod, Student student, Profesor profesor, Curs curs)
        {
            if (methodName.Equals("UpdateStudent"))
            {
                UpdateStudent(cod, student);
            }
            if (methodName.Equals("UpdateCurs"))
            {
                UpdateCurs(cod, curs);
            }
            if (methodName.Equals("UpdateProfesor"))
            {
                UpdateProfesor(cod, profesor);
            }
        }
        public void ExecuteMethodDelete(string methodName, int cod)
        {
            if (methodName.Equals("DeleteStudent"))
            {
                DeleteStudent(cod);
            }
            if (methodName.Equals("DeleteProfesor"))
            {
                DeleteProfesor(cod);
            }
            if (methodName.Equals("DeleteCurs"))
            {
                DeleteCurs(cod);
            }
        }
            public List<Student> GetAllStudents()
        {
            string query = "SELECT * FROM student";
            List<Student> students = new List<Student>();
            if (dbCon.OpenConnection() == true)
            {
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string nume = reader["Nume"].ToString();
                        string varstaString = reader["Varsta"].ToString();
                        string nrMatricolString = reader["NrMatricol"].ToString();
                        int id = Convert.ToInt32(idString);
                        int varsta = Convert.ToInt32(varstaString);
                        int nrMatricol = Convert.ToInt32(nrMatricolString);
                        Student student = new Student(id, nume, varsta, nrMatricol);
                        students.Add(student);
                    }
                }
                dbCon.CloseConnection();
            }
            return students;
        }
        public Student GetStudent(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM student WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string nume = reader["Nume"].ToString();
                        string varstaString = reader["Varsta"].ToString();
                        string nrMatricolString = reader["NrMatricol"].ToString();
                        int idStudent = Convert.ToInt32(idString);
                        int varsta = Convert.ToInt32(varstaString);
                        int nrMatricol = Convert.ToInt32(nrMatricolString);
                        student = new Student(idStudent, nume, varsta, nrMatricol);
                        dbCon.CloseConnection();
                        return student;
                    }
                }
                dbCon.CloseConnection();
            }
            return null;
        }
        public void InsertStudent(Student student)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO student (id, Nume, Varsta, NrMatricol) VALUES (@id, @nume, @varsta, @nrMatricol)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", student.id);
                cmd.Parameters.AddWithValue("@nume", student.nume);
                cmd.Parameters.AddWithValue("@varsta", student.varsta);
                cmd.Parameters.AddWithValue("@nrMatricol", student.nrMatricol);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
        public void UpdateStudent(int id, Student student)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE student SET Nume=@nume, Varsta=@varsta, NrMatricol=@nrMatricol WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@nume", student.nume);
                cmd.Parameters.AddWithValue("@varsta", student.varsta);
                cmd.Parameters.AddWithValue("@nrMatricol", student.nrMatricol);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public void DeleteStudent(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM student WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error deleting data from Database!");
            }
        }
        public bool isEmptyStudent()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT count(*) FROM student";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                //var reader = cmd.ExecuteReader();
                int count = (int)(long)cmd.ExecuteScalar();
                if (count == 0)
                {
                    dbCon.CloseConnection();
                    return true;
                }
                else
                {
                    dbCon.CloseConnection();
                    return false;
                }
            }
            dbCon.CloseConnection();
            return false;
        }
        public int GetNextIdStudent()
        {
            if (isEmptyStudent())
            {
                return 1;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT id FROM student ORDER BY ID DESC LIMIT 1";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string idString = reader["id"].ToString();
                        int id = Convert.ToInt32(idString);
                        id++;
                        dbCon.CloseConnection();
                        return id;
                    }
                }
                return 0;
            }
        }

        public List<Profesor> GetAllProfesors()
        {
            List<Profesor> profesors = new List<Profesor>();
            if (dbCon.OpenConnection() == true)
            {
                string query = "SELECT * FROM profesor";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string nume = reader["Nume"].ToString();
                        string varstaString = reader["Varsta"].ToString();
                        string specializare = reader["Specializare"].ToString();
                        int id = Convert.ToInt32(idString);
                        int varsta = Convert.ToInt32(varstaString);
                        Profesor profesor = new Profesor(id, nume, varsta, specializare);
                        profesors.Add(profesor);
                    }
                }
                dbCon.CloseConnection();
            }
            return profesors;
        }

        public Profesor GetProfesor(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM profesor WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string nume = reader["Nume"].ToString();
                        string varstaString = reader["Varsta"].ToString();
                        string specializare = reader["Specializare"].ToString();
                        int idProf = Convert.ToInt32(idString);
                        int varsta = Convert.ToInt32(varstaString);
                        profesor = new Profesor(idProf, nume, varsta, specializare);
                        dbCon.CloseConnection();
                        return profesor;
                    }
                }
                dbCon.CloseConnection();
            }
            return null;
        }
        public void InsertProfesor(Profesor profesor)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO profesor (id, Nume, Varsta, Specializare) VALUES (@id, @nume, @varsta, @specializare)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", profesor.id);
                cmd.Parameters.AddWithValue("@nume", profesor.nume);
                cmd.Parameters.AddWithValue("@varsta", profesor.varsta);
                cmd.Parameters.AddWithValue("@specializare", profesor.specializare);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
        public void UpdateProfesor(int id, Profesor profesor)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE profesor SET Nume=@nume, Varsta=@varsta, Specializare=@specializare WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@nume", profesor.nume);
                cmd.Parameters.AddWithValue("@varsta", profesor.varsta);
                cmd.Parameters.AddWithValue("@specializare", profesor.specializare);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public void DeleteProfesor(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM profesor WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error deleting data from Database!");
            }
        }
        public bool isEmptyProfesor()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT count(*) FROM profesor";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                //var reader = cmd.ExecuteReader();
                int count = (int)(long)cmd.ExecuteScalar();
                if (count == 0)
                {
                    dbCon.CloseConnection();
                    return true;
                }
                else
                {
                    dbCon.CloseConnection();
                    return false;
                }
            }
            dbCon.CloseConnection();
            return false;
        }
        public int GetNextIdProfesor()
        {
            if (isEmptyProfesor())
            {
                return 1;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT id FROM profesor ORDER BY ID DESC LIMIT 1";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string idString = reader["id"].ToString();
                        int id = Convert.ToInt32(idString);
                        id++;
                        dbCon.CloseConnection();
                        return id;
                    }
                }
                return 0;
            }
        }
        public List<Curs> GetAllCurs()
        {
            string query = "SELECT * FROM curs";
            List<Curs> cursuri = new List<Curs>();
            if (dbCon.OpenConnection() == true)
            {
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string denumire = reader["Denumire"].ToString();
                        string nrStudentiString = reader["NrStudenti"].ToString();
                        string idProfesorString = reader["idProfesor"].ToString();
                        string descriere = reader["Descriere"].ToString();
                        int id = Convert.ToInt32(idString);
                        int nrStudenti = Convert.ToInt32(nrStudentiString);
                        int idProfesor = Convert.ToInt32(idProfesorString);
                        Curs curs = new Curs(id, denumire, nrStudenti, idProfesor, descriere);
                        cursuri.Add(curs);
                    }
                    dbCon.CloseConnection();
                }
                dbCon.CloseConnection();
            }
            return cursuri;
        }
        public Curs GetCurs(string denumire)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM curs WHERE Denumire=@denumire";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@denumire", denumire);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string denumireCurs = reader["Denumire"].ToString();
                        string nrStudentiString = reader["NrStudenti"].ToString();
                        string idProfesorString = reader["idProfesor"].ToString();
                        string descriere = reader["Descriere"].ToString();
                        int id = Convert.ToInt32(idString);
                        int nrStudenti = Convert.ToInt32(nrStudentiString);
                        int idProfesor = Convert.ToInt32(idProfesorString);
                        curs = new Curs(id, denumireCurs, nrStudenti, idProfesor, descriere);
                        dbCon.CloseConnection();
                        return curs;
                    }
                    
                }
                dbCon.CloseConnection();
            }
            return null;
        }
        public Curs GetCursById(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM curs WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string denumireCurs = reader["Denumire"].ToString();
                        string nrStudentiString = reader["NrStudenti"].ToString();
                        string idProfesorString = reader["idProfesor"].ToString();
                        string descriere = reader["Descriere"].ToString();
                        int idCurs = Convert.ToInt32(idString);
                        int nrStudenti = Convert.ToInt32(nrStudentiString);
                        int idProfesor = Convert.ToInt32(idProfesorString);
                        Curs curs = new Curs(idCurs, denumireCurs, nrStudenti, idProfesor, descriere);
                        dbCon.CloseConnection();
                        return curs;
                    }
                }
                dbCon.CloseConnection();
            }
            return null;
        }
        public void InsertCurs(Curs curs)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO curs (id, Denumire, NrStudenti, idProfesor) VALUES (@id, @denumire, @nrStudenti, @idProfesor)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", curs.id);
                cmd.Parameters.AddWithValue("@denumire", curs.denumire);
                cmd.Parameters.AddWithValue("@nrStudenti", curs.nrStudenti);
                cmd.Parameters.AddWithValue("@idProfesor", curs.idProfesor);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
        public void UpdateCurs(int id, Curs curs)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "UPDATE curs SET Denumire=@denumire, NrStudenti=@nrStudenti, idProfesor=@idProfesor WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@denumire", curs.denumire);
                cmd.Parameters.AddWithValue("@nrStudenti", curs.nrStudenti);
                cmd.Parameters.AddWithValue("@idProfesor", curs.idProfesor);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error updating data into Database!");
            }
        }
        public void DeleteCurs(int id)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "DELETE FROM curs WHERE id=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                if (result < 0)
                    Console.WriteLine("Error deleting data from Database!");
                dbCon.CloseConnection();
            }
        }
        public bool isEmptyCurs()
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT count(*) FROM curs";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                //var reader = cmd.ExecuteReader();
                int count = (int)(long)cmd.ExecuteScalar();
                if (count == 0)
                {
                    dbCon.CloseConnection();
                    return true;
                }
                else
                {
                    dbCon.CloseConnection();
                    return false;
                }
            }
            dbCon.CloseConnection();
            return false;
        }
        public int GetNextIdCurs()
        {
            if (isEmptyCurs())
            {
                return 1;
            }
            else
            {
                if (dbCon.OpenConnection() == true)
                {
                    String query = "SELECT id FROM curs ORDER BY ID DESC LIMIT 1";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string idString = reader["id"].ToString();
                        int id = Convert.ToInt32(idString);
                        id++;
                        dbCon.CloseConnection();
                        return id;
                    }
                    dbCon.CloseConnection();
                }
                return 0;
            }
        }
        public void AddStudentToCurs(int idCurs, int idStudent)
        {
            if (dbCon.OpenConnection() == true)
            {
                String query = "INSERT INTO student_curs (idStudent, idCurs) VALUES (@idStudent, @idCurs)";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@idStudent", idStudent);
                cmd.Parameters.AddWithValue("@idCurs", idCurs);
                int result = cmd.ExecuteNonQuery();
                dbCon.CloseConnection();
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
        public List<Student> GetAllStudentsOfCurs(int idCurs)
        {
            List<Student> students = new List<Student>();
            if (dbCon.OpenConnection() == true)
            {
                String query = "SELECT * FROM student s INNER JOIN student_curs sc ON s.id=sc.idStudent INNER JOIN curs c ON sc.idCurs = c.id WHERE c.id=@idCurs";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@idCurs", idCurs);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string nume = reader["Nume"].ToString();
                        string varstaString = reader["Varsta"].ToString();
                        string nrMatricolString = reader["NrMatricol"].ToString();
                        int id = Convert.ToInt32(idString);
                        int varsta = Convert.ToInt32(varstaString);
                        int nrMatricol = Convert.ToInt32(nrMatricolString);
                        Student student = new Student(id, nume, varsta, nrMatricol);
                        students.Add(student);
                    }
                }
                dbCon.CloseConnection();
            }
            return students;
        }
        public List<Curs> GetAllCursOfStudent(int idStudent)
        {
            List<Curs> curses = new List<Curs>();
            if (dbCon.OpenConnection() == true)
            {
                string query = "SELECT * FROM curs c INNER JOIN student_curs sc ON c.id = sc.idCurs INNER JOIN student s ON sc.idStudent = s.id WHERE s.id = @idStudent";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.Parameters.AddWithValue("@idStudent", idStudent);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string idString = reader["id"].ToString();
                        string denumire = reader["Denumire"].ToString();
                        string nrStudentiString = reader["NrStudenti"].ToString();
                        string idProfesorString = reader["idProfesor"].ToString();
                        string descriere = reader["Descriere"].ToString();
                        int id = Convert.ToInt32(idString);
                        int nrStudenti = Convert.ToInt32(nrStudentiString);
                        int idProfesor = Convert.ToInt32(idProfesorString);
                        Curs curs = new Curs(id, denumire, nrStudenti, idProfesor, descriere);
                        curses.Add(curs);
                    }
                }
                dbCon.CloseConnection();
            }
            return curses;
        }
    }
}