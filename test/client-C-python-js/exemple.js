const Infl = gi.imports.Infl;

let client = new Infl.Client();

console.log('Connexion...');
client.connect("localhost:1337", "ares");

let f = client.next_round();
while (client.status == Infl.ClientStatus.ONGOING) {
    let cells = client.get_my_cells();
        for (let c in cells) {
                if (c.unit_count > 1) {
                        let a_x = c.x + Math.floor(Math.random() * 3.0) - 1;
                        let a_y = c.y + Math.floor(Math.random() * 3.0) - 1;
                        
                        if (!(a_x == x && a_y = y)) {
                                client.attack(x, y, a_x, a_y);
                                console.log("Attaque (" + c.x + ", " + c.y + ") -> (" + a_x + ", " + a_y + ")");
                        } 
                }
        }

        let n_units = client.end_attacks();
        
        console.log("Ajout cellules");
        for (var i = 0; i < n_units; i++) {
                var cell = cells[Math.floor(Math.random() * n_units)];
                
                if (cell.unit_count < 20)
                        client.add_units(cell, 1);
        }

        client.end_adding_units();

        f = client.next_round();
}

switch (client.status) {
        case Infl.ClientStatus.VICTORY:
                console.log("Victoire !!!\n");
                break;
        case Infl.ClientStatus.DEFEAT:
                console.log("Défaite !!!\n");
                break;
        case Infl.ClientStatus.CONNECTION_LOST:
                console.log("Problème réseau");
                break;
        case Infl.ClientStatus.ONGOING:
                console.log("Ongoing ?!?");
                break;
}

