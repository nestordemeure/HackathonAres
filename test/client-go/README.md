# Hackathon Ares : Le client go

## Partie 1: Installation

### Installation de go (Linux)

```
cd /tmp
wget https://storage.googleapis.com/golang/go1.7.5.linux-amd64.tar.gz
tar -C /usr/local -xzf go1.7.5.linux-amd64.tar.gz
rm go1.7.5.linux-amd64.tar.gz
```

### Configuration du GOPATH
```
mkdir $HOME/gocode
cd $HOME/gocode
mkdir src pkg bin
echo "export GOPATH=$(pwd)" >> $HOME/.bashrc
export GOPATH=$(pwd)
```

### Clonage de la bibliothèque

```
go get git.ares-ensiie.eu/hackathon/hackathon-go-client/client
```
Ou alors
```
mkdir -p $GOPATH/src/git.ares-ensiie.eu/hackathon/
cd $GOPATH/src/git.ares-ensiie.eu/hackathon/
git clone http://git.ares-ensiie.eu/hackathon/hackathon-go-client.git
```

## Partie 2: Utilisation

Pour utiliser ce client, il suffit de l'inclure dans vos import:
```
import (
  ...
	"git.ares-ensiie.eu/hackathon/hackathon-go-client/client"
)
```

## Partie 3: Exemple

Le fichier exemple.go donne un exemple commenté du client go. Ce client va jouer des coups aléatoires jusqu'à ce qu'il gagne ou qu'il perde.

Pour lancer le client, utilisez la commande go run:
```
go run exemple.go
```

## Partie 4: Code Source et documentation

### Code source
Le code source du client go peut être trouvé sur le gitlab à cette url :
[hackathon/hackathon-go-client](https://git.ares-ensiie.eu/hackathon/hackathon-go-client)

### Documentation

#### Version texte
```
godoc git.ares-ensiie.eu/hackathon/hackathon-go-client
```

#### Version HTML
Une version html de la documentation est disponible dans le dossier doc.  
Vous pouvez aussi la retrouver ici : [doc-go](https://perso.ares-ensiie.eu/miclo2018/doc_go/doc.html)  

Cependant elle peut également être générée en local:
```
godoc -http=:6060
```
La documentation sera disponible à l'adresse suivante: [http://localhost:6060/pkg/git.ares-ensiie.eu/hackathon/hackathon-go-client/client/](http://localhost:6060/pkg/git.ares-ensiie.eu/hackathon/hackathon-go-client/client/)
