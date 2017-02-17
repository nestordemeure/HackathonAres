
# Hackathon client C/Python/Javascript

## Avec le paquet .deb

C'est la méthode la plus simple. Téléchargez le paquet et double-cliquez dessus.
Le paquet contient aussi les dépendances qui permettent de faire fonctionner
les bindings Python et Javascript.

L'exemple en C se compile de la manière suivante :

    gcc exemple.c -o exemple `pkg-config --libs --cflags gobject-2.0` -linfluence
    
Si vous voulez utiliser les bindings Python ou Javascript, lisez ensuite les sections
«Binding Python» ou «Binding Javascript».

## Installation manuelle

GLib doit être installée. Pour avoir les bindings python, il faut ausi installer
gobject-instrospection.

    sudo apt install libglib2.0-dev libgirepository1.0-dev
    
Ensuite, pour compiler et installer le client dans ~/.local :

```bash    
PREFIX=$HOME/.local
./configure --prefix=$PREFIX
make
make install
```

Vous pouvez changer le dossier d'installation en changeant la valeur de `$PREFIX`.

## Client C
    
Afin d'utiliser le client, le système doit savoir ou se trouve
la bibliothèque :

```bash
export LD_LIBRARY_PATH=$PREFIX/lib:$LD_LIBRARY_PATH
export LD_RUN_PATH=$PREFIX/lib:$LD_RUN_PATH
export LIBRARY_PATH=$PREFIX/lib:$LIBRARY_PATH
export CPATH=$PREFIX/include:$CPATH
```

L'exemple se compile de la manière suivante :

    gcc exemple.c -o exemple `pkg-config --libs --cflags gobject-2.0` -linfluence
    
## Binding python

### Si vous avez installé le client manuellement

Pour pouvoir utiliser le binding Python, il faut installer PyGObject (qui est
souvent déjà installé quand on a gnome) :

    sudo apt install python3-gi
    
Puis donner le chemin des bindings

```bash
export GI_TYPELIB_PATH=$PREFIX/lib/girepository-1.0
export LD_LIBRARY_PATH=$PREFIX/lib:$LD_LIBRARY_PATH
```
    
### Utilisation du binding

Vous aurez simplement besoin de faire l'import suivant :

```python
from gi.repository import Infl
```

## Binding Js

### Si vous avez installé le client manuellement

Installez gjs pour utiliser le binding Javascript.

    sudo apt install gjs

Donnez le chemin des bindings :

```bash
export GI_TYPELIB_PATH=$PREFIX/lib/girepository-1.0
export LD_LIBRARY_PATH=$PREFIX/lib:$LD_LIBRARY_PATH
```

### Utilisation du binding

Pour exécuter l'exemple :

    gjs exemple.js

