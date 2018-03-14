using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceGsb
{
    /**
     * Classe abstraite qui s'occupe des fonctions propres à la date concernant
     * l'application Gsb frais.
     * 
     * @author Naëva Arnould
     */
    static class DateGsb
    {
        /**
        * Permet d'obtenir le mois suivant au moins entré en paramètre au format "yyyyMM".
        * Si aucun paramètre n'est donné, on retourne le mois suivant au mois courant.
        * 
        * @param String month  (Obtionnel) Le mois dont on veut obtenir le mois suivant.
        * 
        * @return String       Le mois suivant au format "yyyyMM"
        */
        public static string getNextMonth(string month = null)
        {
            if (month == null)
            {
                string nextMonth = DateTime.Now.AddMonths(1).ToString("yyyyMM");
                return nextMonth;
            }
            else
            {
                string yearNum = month.Substring(0, 4);
                string monthNum = month.Substring(4);
                Console.WriteLine(yearNum + monthNum);
                return yearNum + monthNum;
            }
        }

        /**
         * Permet d'obtenir le mois précédent au moins entré en paramètre au format "yyyyMM".
         * Si aucun paramètre n'est donné, on retourne le mois précédent au mois courant.
         * 
         * @param String month  (Obtionnel) Le mois dont on veut obtenir le mois précédent.
         * 
         * @return String       Le mois précédent au format "yyyyMM"
         */ 
        public static string getPreviousMonth(string month = null)
        {
            if (month == null)
            {
                string previousMonth = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
                return previousMonth;
            }
            else
            {
                string yearNum = month.Substring(0, 4);
                string monthNum = month.Substring(4);
                Console.WriteLine(yearNum + monthNum);
                return yearNum + monthNum;
            }
        }

        /**
         * Teste si l'on se trouve dans une période de validation, c'est-à-dire
         * entre le 1er et 10 du mois courant.
         * 
         * @return Boolean  Vrai si l'on est entre le 1 et 10 du mois (période Validation), 
         * faux sinon
         */
         public static Boolean isPeriodeValidation()
        {
            if (DateTime.Now.Day <= 10)
            {
                return true;
            }
            return false;
        }

        /**
         * Teste si l'on se trouve dans une période de remboursement, c'est-à-dire
         * entre le 20 et la fin du mois courant.
         * 
         * @return Boolean  Vrai si l'on est entre le 20 et la fin du mois (période Remboursement), 
         * faux sinon
         */
        public static Boolean isPeriodeRemboursement()
        {
            if(DateTime.Now.Day >= 20)
            {
                return true;
            }
            return false;
        }
    }
}
