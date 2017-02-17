from gi.repository import Infl
import sys
from random import randint, choice

# Création du client
client = Infl.Client()

print('Connexion...')
# Connexion du client
client.connect("localhost:1337", sys.argv[1] if len(sys.argv) == 2 else "python")
print("Connecté")

# Attente du tour suivant. f contient la carte du jeu.
f = client.next_round()
# Boucle du jeu
while client.get_status() == Infl.ClientStatus.ONGOING:
    # cell contient la liste des cellules appartenant au joueur
    cells = client.get_my_cells()
    for c in cells:
        if c.get_unit_count() > 1:
            # On peut aussi utiliser c.get_x() et c.get_y()
            x, y = c.props.x, c.props.y
            a_x = x + randint(0 if x == 0 else -1, 0 if x == 255 else 1)  
            a_y = y + randint(0 if y == 0 else -1, 0 if y == 255 else 1)  

            if not (a_x == x and a_y == y):
                # Attaque
                client.attack(x, y, a_x, a_y)
                print("Attaque ({}, {}) -> ({}, {})".format(x, y, a_x, a_y))
    
    # Fin de l'attaque, début du placement
    n_units = client.end_attacks()
    for i in range(n_units):
        client.add_units(choice(cells), 1)

    print('Placement')
    
    # Fin du placement
    client.end_adding_units()
    
    # Attente du tour suivant
    f = client.next_round()

# Affichage de la raison d'arrêt de la boucle (victoire, défaite, ou erreur réseau)
print(client.get_status())

# La déconnexion est automatique

