using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src.DataModel
{
    class Profesor
    {
        public int id { get; set; }
        public string nume { get; set; }
        public int varsta { get; set; }
        public string specializare { get; set; }

        public Profesor() { }
        public Profesor(int id,string nume, int varsta, string specializare) {
            this.id = id;
            this.nume = nume;
            this.varsta = varsta;
            this.specializare = specializare;
        }

    }
}
