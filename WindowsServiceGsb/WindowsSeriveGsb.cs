using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsServiceGsb
{
    public partial class WindowsServiceGsb : ServiceBase
    {
        private DBConnect connection;
        private static System.Timers.Timer appTimer;
        public WindowsServiceGsb()
        {
            InitializeComponent();

            //Initialisation de la connection MySql
            connection = new DBConnect();

            //Pour pouvoir logguer des messages dans le journal "MySource"
            eventLog1 = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            String log = "Entrée dans le OnStart()";
            eventLog1.WriteEntry(log);
            Console.Write(log);
            Console.WriteLine();

            StartTimer(50000); // toutes les 5 minutes
        }


        private void OnTimer(Object source, ElapsedEventArgs e)
        {
            String log = "Opération timer: ";
            Console.WriteLine(log);
            Console.WriteLine();
            eventLog1.WriteEntry(log);

            if (DateGsb.isPeriodeValidation())
            {
                String previousMonth = DateGsb.getPreviousMonth();
                List < FicheFrais > ficheFraisList = new List<FicheFrais>();
                //sélection des fiches du mois précédent à l'état 'Saisi en cours' (CR)
                ficheFraisList = connection.SelectFicheForMonthAndForIdEtat(previousMonth, "CR");

                //Mettre chacune de ces fiches frais à l'état CL
                foreach (var ficheFrais in ficheFraisList)
                {
                    connection.majEtatFicheFrais(ficheFrais, "CL");
                }
                Console.WriteLine("Mise à jour des fiches du mois : " + previousMonth +
                    " à VA (période de validation).");
                Console.WriteLine();
            }
            else if (DateGsb.isPeriodeRemboursement())
            {
                String previousMonth = DateGsb.getPreviousMonth();
                List<FicheFrais> ficheFraisListVA = new List<FicheFrais>();
                List<FicheFrais> ficheFraisListPM = new List<FicheFrais>();
                //sélection des fiches du mois précédent à l'état 'Saisi en cours' (CR)
                ficheFraisListVA = connection.SelectFicheForMonthAndForIdEtat(previousMonth, "VA");
                ficheFraisListPM = connection.SelectFicheForMonthAndForIdEtat(previousMonth, "PM");

                /*
                 * Les fiches à l'état 'Validée' (VA) et 'Mise en paiment' (PM) sont 
                 * mises en "Remboursées" (RB) à partir du 20 du mois.
                 */

                //Mettre chacune de ces fiche frais à l'état RB
                foreach (var ficheFraisVA in ficheFraisListVA)
                {
                    connection.majEtatFicheFrais(ficheFraisVA, "RB");
                }
                //Mettre chacune de ces fiche frais à l'état RB
                foreach (var ficheFraisPM in ficheFraisListPM)
                {
                    connection.majEtatFicheFrais(ficheFraisPM, "RB");
                }
                Console.WriteLine("Mise à jour des fiches du mois : " + previousMonth + 
                    " à RB (période de remboursement).");
                Console.WriteLine();
            }
        }
        

        protected override void OnStop()
        {
            String log = "Entrée dans le OnStop()";

            eventLog1.WriteEntry(log);
            Console.Write(log);
            Console.WriteLine();

            this.StopTimer();
        }


        protected override void OnContinue()
        {
            String log = "Entrée dans le OnContinue()";

            eventLog1.WriteEntry(log);

            Console.Write(log);
            Console.WriteLine();
        }


        /**
         * Crée un timer qui executera du code toutes les x milliseconde(s) entrée(s)
         * en paramètre.
         * 
         * @param Integer milliseconds  Le nombre de millisecondes d'intervale
         */
        private void StartTimer(int milliseconds)
        {
            appTimer = new System.Timers.Timer(milliseconds);
            //fonction executée à chaque intervalle
            appTimer.Elapsed += new ElapsedEventHandler(OnTimer);
            //appTimer.AutoReset = true;
            appTimer.Enabled = true;
        }

        /**
         * Stoppe le timer
         */
        private void StopTimer()
        {
            //Arrêt du timer
            appTimer.Stop();

            string log = "Arrêt du timer";
            Console.Write(log);
            Console.WriteLine();
            eventLog1.WriteEntry(log);

            appTimer.Dispose();
        }


        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {


        }
    }
}
