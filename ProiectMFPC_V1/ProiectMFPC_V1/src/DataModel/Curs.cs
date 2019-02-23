using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src.DataModel
{
    class Curs
    {
        public int id { get; set; }
        public string denumire { get; set; }
        public int nrStudenti { get; set; }
        public int idProfesor { get; set; }
        public string descriere { get; set; }

        public Curs() { }
        public Curs(int id, string denumire, int nrStudenti, int idProfesor, string descriere)
        {
            this.id = id;
            this.denumire = denumire;
            this.nrStudenti = nrStudenti;
            this.idProfesor = idProfesor;
            this.descriere = descriere;
        }
    }
}
