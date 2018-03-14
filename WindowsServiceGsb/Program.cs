using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceGsb
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WindowsServiceGsb()
            };

            ///---DEBUGGAGE---///
            // On est en mode intéractif et débogage ?
            if (Environment.UserInteractive && System.Diagnostics.Debugger.IsAttached)
            {
                // Simule l'exécution des services
                RunInteractiveServices(ServicesToRun);
            }
            else
            {
                // Exécute les services normalement
                
                ServiceBase.Run(ServicesToRun);
            }
            //-----------FIN DEBUGGAGE-------///
            //ServiceBase.Run(ServicesToRun);

        }

        //-------DEBUGGAGE------------///
        /// <summary>
        /// Exécute les services en mode interactif
        /// </summary>
        static void RunInteractiveServices(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Démarrage des services en mode intéractif.");
            Console.WriteLine();

            Console.WriteLine("Appuyer sur une ENTREE pour arrêter les services et terminer le processus...");
            Console.WriteLine();

            // Récupération de la méthode a exécuter sur chaque service pour le démarrer
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);

            // Boucle de démarrage des services
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Démarrage de {0} : \n", service.ServiceName);
                Console.WriteLine();
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            // Attente de l'appui sur une touche pour arrêter
            Console.ReadLine();
            Console.WriteLine();

            // Récupération de la méthode à exécuter sur chaque service pour l'arrêter
            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);

            // Boucle d'arrêt
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Arrêt de {0} ... ", service.ServiceName);
                Console.WriteLine();
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Arrêté");
            }

            Console.WriteLine();
            Console.WriteLine("Tous les services sont arrêtés.");

            // Attend l'appui d'une touche pour ne pas retourner directement à VS
            Console.WriteLine();
            Console.Write("=== Appuyer sur une touche pour quitter ===");
            Console.Read();
        }
        //-----------FIN DEBUGGAGE-------///
    }
}
