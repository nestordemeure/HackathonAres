# Hackathon Ares 2017

## Le jeu

Le jeu que nous vous proposons est basé sur l'application `Influence` disponible uniquement sur Android.   

### Principe

Le but du jeu est de prendre toutes les cellules de vos adversaires afin que vous soyez seul sur le plateau.   
Pour le tournoi principal, vous serez seulement 2 sur le plateau.

### Début de la partie

Vous commencez sur une case choisie aléatoirement, de même que vos adversaires.

### Déroulement d'un tour

#### Attaques

Lors de votre tour de jeu, vous pouvez tout d'abord attaquer d'autres cellules du plateau. Une attaque est valide si la cellule attaquée existe bel et bien sur le plateau, si la cellule d'origine de l'attaque est adjacente à la cellule attaquée, et si la cellule attaquée ne vous appartient pas.

    ┼───┼───┼───┼
    │ A │ A │ A │      
    ┼───┼───┼───┼
    │ A │ O │ A │ 
    ┼───┼───┼───┼
    │ A │ A │ A │
    ┼───┼───┼───┼

    La cellule O est la cellule d'origine de l'attaque, les cellules A sont les cellules adjacentes.

**Vous ne pouvez pas attaquer avec moins de 2 unités sur la cellule d'origine de l'attaque**   
Il vous est possible de réaliser au maximum 20 attaques par tour. Si vous lancez une attaque invalide, celle-ci sera quand même comptée dans les 20.

#### Placement des unités

Une fois les attaques terminées, vous pouvez placer des unités sur vos cellules (le nombre d'unités à placer vous est envoyé par le serveur).  
Vous pouvez placer plusieurs unités sur une même cellule, dans la limite de 20 unités par cellule.

### Fin de la partie

La partie se termine lorsqu'il ne reste plus qu'un joueur sur le plateau.   
Vous perdez dès que vous n'avez plus aucune cellule sur le plateau. A l'inverse, vous gagnez dès qu'il n'y a plus aucun adversaire sur le plateau.
Vous perdez si la connexion avec le serveur se coupe. Vous perdez si le protocole n'est pas respecté.

## Les clients

Nous vous proposons des implémentations du protocole dans les langages suivants :

* Client go: [client-go/](https://git.ares-ensiie.eu/hackathon/hackathon-sujet/tree/master/client-go)
* Client C# : [client-c#/](https://git.ares-ensiie.eu/hackathon/hackathon-sujet/tree/master/client-c%23)
* Client Java : [client-java/](https://git.ares-ensiie.eu/hackathon/hackathon-sujet/tree/master/client-java)
* Client C, Python, JS : [client-c-python-js/](https://git.ares-ensiie.eu/hackathon/hackathon-sujet/tree/master/client-C-python-js)

Les README de chaque client indiquent où se trouve la documentation du client.

## Le serveur

Vous pouvez télécharger le serveur et l'interface web pour visualiser vos matchs : [server](https://mryawe.github.io/hackathon-server.zip)  
Pour pouvoir lancer le serveur, passez le fichier en executable :  
`chmod 755 hackathon-server`  
Ensuite lancez-le en faisant `./hackathon-server`


## Le protocole

Si vous voulez faire votre propre client, vous pouvez suivre le protocole décrit sur la page suivante : [protocol](https://git.ares-ensiie.eu/hackathon/hackathon-protocol/wikis/home)   
Les méthodes que nous fournissons dans nos clients sont sur cette page : [client-api](https://git.ares-ensiie.eu/hackathon/hackathon-protocol/wikis/client-api)   
Cela peut vous donner une base mais vous n'êtes pas obligés de suivre exactement ces recommandations.   

**Attention, toute erreur dans le protocole entraînera une perte de la partie**

