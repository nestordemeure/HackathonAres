using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influence
{
    class Exemple
    {
        static void Main(string[] args)
        {
			// Initialisation du random
            Random r = new Random();
			
			// On récupère l'instance du client (cela le crée en fait)
            InfluenceClient client = InfluenceClient.GetInstance();

			// On se connecte au serveur : le premier paramètre est son adresse IP, le deuxième est le nom de votre équipe
			// Cet appel affiche votre numéro sur le plateau de jeu
            client.Connect("127.0.0.1", "test1");

			// Tant que la partie est en cours
            while (client.GetStatus() == InfluenceClient.Status.ONGOING)
            {
				// On attend notre tour, ce qui permet de récupérer le nouveau plateau de jeu
                InfluenceField field = client.NextRound();

				 // Après l'appel à nextRound, il se peut que le statut ait été modifié
                if (client.GetStatus() != InfluenceClient.Status.ONGOING)
                {
                    break;
                }

                List<InfluenceCell> myCells;
                Console.WriteLine(DateTime.Now + " - Attacking");
				
				// On va essayer de lancer 10 attaques
                for (int i = 0; i < 10; i++)
                {
					// On récupère toutes les cellules qui nous appartiennent
                    myCells = client.GetMyCells();
					
					// On en choisit une aléatoirement parmi celles-ci
                    InfluenceCell c = myCells.ElementAt(r.Next(myCells.Count));
					
					// Si la cellule choisie possède la puissance nécessaire pour attaquer
                    if (c.GetUnitsCount() >= 2)
                    {
						// On définit une cellule cible dans son entourage
                        int dx = c.GetX() + r.Next(3) - 1;
                        int dy = c.GetY() + r.Next(3) - 1;
						
						// Si la cible est une position valide (ses coordonnées sont bien sur le plateau)
                        if (dx >= 0 && dx < field.GetWidth() && dy >= 0 && dy < field.GetHeight())
                        {
                            InfluenceCell cellToAttack = field.GetCell(dx, dy);
							// Si la cellule cible appartient à un adversaire
                            if (cellToAttack != null && cellToAttack.GetOwner() != client.GetNumber())
                            {
								// On attaque la cible et on récupère le plateau de jeu après l'attaque
                                field = client.Attack(c.GetX(), c.GetY(), cellToAttack.GetX(), cellToAttack.GetY());
                            }
                        }
                    }
                }

				// On prévient le serveur qu'on a fini les attaques, il nous renvoie le nombre d'unités à placer
                int unitsToAdd = client.EndAttacks();

				// Phase de placement des unités
				
				// On récupère les cellules qui nous appartiennent
                myCells = client.GetMyCells();
				// Tant que l'on peut placer des unités
                for (int i = 0; i < unitsToAdd; i++)
                {
					// On choisit une cellule aléatoirement parmi celles qui nous appartiennent
                    InfluenceCell c = myCells.ElementAt(r.Next(myCells.Count));
					// On ajoute une unité à la cellule choisie
                    client.AddUnits(c, 1);
                }
				
				// On indique au serveur que l'on a fini notre tour
                client.EndAddingUnits();
            }
            
			// On est sorti de la boucle donc la partie est terminée
            switch (client.GetStatus())
            {
                case InfluenceClient.Status.VICTORY:
                    Console.WriteLine("YOU WON!");
                    break;

                case InfluenceClient.Status.DEFEAT:
                    Console.WriteLine("YOU LOST!");
                    break;

                case InfluenceClient.Status.CONNECTION_LOST:
                    Console.WriteLine("YOU LOST BECAUSE OF YOUR CONNECTION");
                    break;

                default:
                    Console.WriteLine("NOT REACHABLE");
                    break;
            }
			
			// Pour éviter que la console se ferme direct
            Console.ReadKey();
        }
    }
}
