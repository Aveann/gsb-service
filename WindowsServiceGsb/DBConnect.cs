using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace WindowsServiceGsb
{
    class DBConnect
    {
        // propriétés
        private readonly string _server = "localhost";
        private readonly string _database = "gsb_frais";
        private readonly string _uid = "admin";
        private readonly string _password = "admin";
        
        private MySqlConnection connection; // connexion MySql


        // constructeur
        public DBConnect()
        {
            this.Init();
        }

        private void Init()
        {
            string connectionString;
            connectionString = String.Format("server={0};database={1};uid={2};password={3}",
                _server, _database, _uid, _password);
            this.connection = new MySqlConnection(connectionString);
        }

        /**
         * Retourne la liste de toutes les fiches à l'état du mois passé en 
         * paramètre et de l'état passé en paramètre
         * 
         * @param String mois   Le mois au format "yyyyMM" dont on veut récupérer
         * les fiches.
         * @param String idetat L'état des fiches qui doivent être retournées
         * 
         * @return List< FicheFrais >   Une liste de FicheFrais avec iduser, mois
         * et idetat.
         */
        public List<FicheFrais> SelectFicheForMonthAndForIdEtat(string mois, string idetat)
        {
            string query = 
                "SELECT iduser, mois, idetat FROM fichefrais " +
                "WHERE idetat = @idetat AND mois = @mois";

            //Create a list to store the result
            List<FicheFrais> ficheFraisList = new List<FicheFrais>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Prepare();
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@idetat", idetat);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Propriétés des fiche frais qui seront contenues dans la liste
                String iduser; //l'id de l'utilisateur
                String moisFiche;  //le mois
                String idetatFiche; //l'état de la fiche ('VA', 'CR', 'CL', 'PM', 'RB')

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    iduser = dataReader["iduser"].ToString();
                    moisFiche = dataReader["mois"].ToString();
                    idetatFiche = dataReader["idetat"].ToString();
                    FicheFrais ficheFrais = new FicheFrais(iduser, mois, idetatFiche);

                    ficheFraisList.Add(ficheFrais);
                }

                dataReader.Close();

                this.CloseConnection();

                //retourne la liste
                return ficheFraisList;
            }
            else
            {
                //retournera du vide
                return ficheFraisList;
            }
        }

        /**
         * Change l'état de la fiche frais identifiée par l'iduser et le mois donnés
         * par l'idetat donné.
         * 
         * @param FicheFrais ficheFrais     La fiche frais à mettre à jour.
         * @param String idetat             Le nouvel état à assigné à cette fiche
         * 
         * @return Boolean          vrai si l'opération s'est bien déroulée, faux sinon.
         */
        public bool majEtatFicheFrais(FicheFrais ficheFrais, string idetat)
        {
            String iduser = ficheFrais.iduser;
            String mois = ficheFrais.mois;
            
            //Open connection
            if (this.OpenConnection() == true)
            {
                string query =
                "UPDATE fichefrais SET idetat = @idetat " +
                "WHERE iduser = @iduser AND mois = @mois";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@idetat", idetat);
                cmd.Parameters.AddWithValue("@iduser", iduser);
                cmd.Parameters.AddWithValue("@mois", mois);

                cmd.ExecuteNonQuery();

                this.CloseConnection();

                return true;
            }

            return false;
        }

        /**
         * Ferme la connection précédemment ouverte.
         * Retourne vrai si la fermeture s'est déroulée avec succès.
         * Si un problème se produit durant la fermeture, un message d'erreur est 
         * imprimé dans la console et on retourne faux.
         * 
         * @return Boolean vrai si aucun problème, faux sinon.
         * */
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                
                return false;
            }
        }

        // Ouverture de la connexion
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.Write("Erreur lors de la connection à la base de données. Code erreur: " + ex.Number + " : " + ex.Message);
                Console.WriteLine();
                return false;
            }
        }
    }
}
