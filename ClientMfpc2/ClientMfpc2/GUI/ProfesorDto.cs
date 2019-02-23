using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMfpc2.GUI
{
    internal class ProfesorDto
    {
        public int id { get; set; }
        public string nume { get; set; }
        public int varsta { get; set; }
        public string specializare { get; set; }

        public ProfesorDto() { }
        public ProfesorDto(int id, string nume, int varsta, string specializare)
        {
            this.id = id;
            this.nume = nume;
            this.varsta = varsta;
            this.specializare = specializare;
        }
        public override string ToString()
        {
            return id + " " + nume + " " + varsta + " " + specializare;
        }
    }
}
