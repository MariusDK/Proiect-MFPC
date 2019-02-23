using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMfpc2.GUI
{
    internal class StudentDto
    {
        public int id { get; set; }
        public string nume { get; set; }
        public int varsta { get; set; }
        public int nrMatricol { get; set; }
        public StudentDto()
        { }
        public StudentDto(int id, string nume, int varsta, int nrMatricol)
        {
            this.id = id;
            this.nume = nume;
            this.varsta = varsta;
            this.nrMatricol = nrMatricol;
        }
        public override string ToString()
        {
            return id + " " + nume + " " + varsta + " " + nrMatricol;
        }
    }
}
