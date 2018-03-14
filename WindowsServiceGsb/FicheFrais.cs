using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceGsb
{
    class FicheFrais
    {
        public string iduser { get; set; }
        public string mois { get; set; }
        public string idetat { get; set; }

        public FicheFrais(string iduser, string mois, string idetat)
        {
            this.iduser = iduser;
            this.mois = mois;
            this.idetat = idetat;
        }

    }
}
