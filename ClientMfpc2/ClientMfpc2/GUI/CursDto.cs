using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMfpc2.GUI
{
    internal class CursDto
    {
        public int id { get; set; }
        public string denumire { get; set; }
        public int nrStudenti { get; set; }
        public int idProfesor { get; set; }
        public string descriere { get; set; }

        public CursDto() { }
        public CursDto(int id, string denumire, int nrStudenti, int idProfesor, string descriere)
        {
            this.id = id;
            this.denumire = denumire;
            this.nrStudenti = nrStudenti;
            this.idProfesor = idProfesor;
            this.descriere = descriere;
        }
        public override string ToString()
        {
            return id + " " + denumire + " " + nrStudenti + " " + idProfesor + " " + descriere;
        }
    }
}
