using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src.DataModel
{
    class Student
    {
        public int id { get; set; }
        public string nume { get; set; }
        public int varsta { get; set; }
        public int nrMatricol { get; set; }
        public Student()
        { }
        public Student(int id, string nume, int varsta, int nrMatricol)
        {
            this.id = id;
            this.nume = nume;
            this.varsta = varsta;
            this.nrMatricol = nrMatricol;
        }
    }
}
