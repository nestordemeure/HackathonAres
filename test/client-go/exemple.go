package main

import (
	"log"
	"math/rand"
	"strconv"
	"time"

	"git.ares-ensiie.eu/hackathon/hackathon-go-client/client"
)

func ce(e error) {
	if e != nil {
		panic(e)
	}
}

func main() {
	// Initialisation du rng
	rng := rand.New(rand.NewSource(time.Now().UnixNano()))

	// On créé un nouveau client
	c := client.NewClient()

	// On se connecte: Le premier champ est l'IP du serveur, le second est le nom
	// de votre équipe
	ce(c.Connect("127.0.0.1:1337", "Exemple: Go"))

	// Sur le plateau, votre équipe est représentée par un identifiant numérique
	// récupéré après la phase de connection.
	log.Println("Connected ! Name: " + c.Name + ", ID: " + strconv.Itoa(c.ID))

	// Tant que le jeu est en cours
	for c.Status() == client.ONGOING {
		log.Println("Waiting for our turn")
		ce(c.NextTurn())

		// Il se peut que le status du jeu change après l'appel à la
		// fonction NextTurn.
		if c.Status() != client.ONGOING {
			break
		}

		log.Println("Attacking")

		// On va essayer de lancer 10 attaques
		for i := 0; i < 10; i++ {
			// On choisi une cellule parmis celles qui nous appartiennent
			mycell := c.MyCells()[rng.Int()%len(c.MyCells())]

			// Si cette céllule à la puissance necessaire pour attaquer
			if mycell.Power >= 2 {
				// On définit une cible dans son entourage
				dx := mycell.X + rng.Int()%3 - 1
				dy := mycell.Y + rng.Int()%3 - 1

				// Si la cible est une position valide
				if dx >= 0 && dx < c.GetField().SizeX && dy >= 0 && dy < c.GetField().SizeY {
					dest := c.Get(dx, dy)
					// Si la cible ne nous appartient pas
					if dest != nil && dest.Owner != c.ID {
						// On attaque la cible
						ce(c.Attack(mycell.X, mycell.Y, dx, dy))
					}
				}
			}
		}
		// Fin du tour d'attaque
		log.Println("End attacks")
		// On previens le serveur que l'on a finit le tour d'attaque
		_, err := c.EndAttacks()
		ce(err)

		// Début de la phase de placement des unitées
		log.Println("Adding units")
		// Tant que l'on peut placer des unitées
		for c.RemainingUnits() > 0 {

			// On choisi une céllule par hasard dans notre liste de céllules
			mycell := c.MyCells()[rng.Int()%len(c.MyCells())]
			// Et on lui ajoute une unité
			c.AddUnit(mycell)
		}
		// On indique au serveur que l'on a finit notre tour
		c.EndAddingUnits()
	}

	// On est sorti de la boucle donc la partie est terminée.

	switch c.Status() {
	case client.DEFEAT:
		log.Println("WE LOST :(")
		break
	case client.VICTORY:
		log.Println("WE WON !")
		break
	case client.CONNECTION_LOST:
		log.Println("CONNECTION LOST : WE PROBABLY LOST :(")
		break
	case client.ONGOING:
		log.Println("WTF !!")
		break
	}
}
