
#include <influence-client.h>

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main(int argc, char **argv) {
    InflClient *client;
    InflField *field;
    int w, h;
    int succ;
    
    srand(time(NULL));
    
    // Création d'un nouveau client
    client = infl_client_new();
    
    printf("Connexion...\n");
    // Connexion du client
    succ = infl_client_connect(client, "localhost:1337", (argc == 2) ? argv[1] : "ares");
    
    if (succ) {
        // Tour suivant, doit être appelé à chaque tour
        field = infl_client_next_round(client);
        infl_field_get_size(field, &w, &h);
        // Boucle de jeu
        while (infl_client_get_status(client) == INFL_CLIENT_ONGOING) {
            InflCell **my_cells;
            int n_cells, i, count;
            int n_units;
            
            // On récupère les cellules qui nous appartiennent
            infl_client_get_my_cells(client, &my_cells, &n_cells);
            
            for (i = 0; i < n_cells; i++) {
                int x, y, a_x, a_y;
                
                // On récupère les informations sur la cellule
                count = infl_cell_get_unit_count(my_cells[i]);
                x = infl_cell_get_x(my_cells[i]);
                y = infl_cell_get_y(my_cells[i]);
                
                if (count > 1) {
                    // Position d'attaque aléatoire
                    a_x = x + rand() % 3 - 1;
                    a_y = y + rand() % 3 - 1;
                    
                    if (!(a_x == x && a_y == y)) {
                        // Attaque
                        infl_client_attack(client, x, y, a_x, a_y);
                        printf("Attaque (%d, %d) -> (%d, %d)\n", x, y, a_x, a_y);
                    }
                }
            }
            
            // Fin des attaques, début de la phase de placement
            n_units = infl_client_end_attacks(client);
            
            for (i = 0; i < n_units; i++) {
                // Selection d'une cellule au hasard
                InflCell *cell = my_cells[rand() % n_cells];
                
                printf("Tentative ajout cellule (%d, %d)\n", infl_cell_get_x(cell), infl_cell_get_y(cell));
                
                if (infl_cell_get_unit_count(cell) <= 20)
                    // Ajout d'une cellule
                    infl_client_add_units(client, cell, 1);
            }
            
            // Fin phase placement
            infl_client_end_adding_units(client);
            
            // Il faut libérer les tableaux retournés par
            // infl_client_get_my_cells
            free(my_cells);
            
            // Passage au tour suivant
            field = infl_client_next_round(client);
        }
        
        // On vérifie la raison de la fin de la partie
        switch (infl_client_get_status(client)) {
            case INFL_CLIENT_VICTORY:
                printf("Victoire !!!\n");
                break;
            case INFL_CLIENT_DEFEAT:
                printf("Défaite !!!\n");
                break;
            case INFL_CLIENT_CONNECTION_LOST:
                printf("Problème réseau\n");
                break;
            case INFL_CLIENT_ONGOING:
                printf("Ongoing ???\n");
                break;
        }
    }
    
    // On ferme la connexion et on libère la mémoire allouée au client
    // il n'y a pas besoin de libérer la mémoire de la carte et des cellules
    g_object_unref(client);
    
    return 0;
}
