# Hackathon Ares : Le client C-sharp

## Partie 1: Installation

### Installation sur Windows

#### Visual Studio Professional 2015

C'est l'IDE le plus complet pour faire du C# et du .NET (utile aussi pour les cours de 2A donc), mais sa version d'évaluation est limitée à 90 jours. Sinon, vous devrez soit le payer soit trouver une belle version crackée.    
Pour le télécharger, rendez-vous ici : [https://go.microsoft.com/fwlink/?LinkId=691980&clcid=0x40c](https://go.microsoft.com/fwlink/?LinkId=691980&clcid=0x40c).    
Lors de l'installation, ne cochez pas toutes les options, sinon ça va prendre beaucoup trop de temps.

#### Visual Studio Community 2015

IDE moins complet mais suffisant pour le Hackathon, il est entièrement gratuit.   
Téléchargez-le ici : [https://go.microsoft.com/fwlink/?LinkId=691978&clcid=0x40c](https://go.microsoft.com/fwlink/?LinkId=691978&clcid=0x40c)

#### Visual Studio Code 2015

Editeur de texte à la manière de Sublime Text, vous pouvez lui ajouter des plugins (Le plugin C# est indispensable).   
Visual Studio Code 2015 est disponible ici : [https://code.visualstudio.com/download](https://code.visualstudio.com/download)

### Installation sur Linux

#### MonoDevelop

Un IDE standard sur Linux, utile pour faire du C#, du F#, du Visual Basic et même du .NET.
```
sudo apt install monodevelop
```
Quand vous lancez MonoDevelop, choisissez un projet console, dans la catégorie .NET.

#### Visual Studio Code 2015

Editeur de texte à la manière de Sublime Text, vous pouvez lui ajouter des plugins (Le plugin C# est indispensable).   
Visual Studio Code 2015 est disponible ici : [https://code.visualstudio.com/download](https://code.visualstudio.com/download)

## Partie 2: Utilisation

### Utilisation de la librairie

#### Visual Studio

Sous Visual Studio (peu importe lequel), cliquez sur `Project` puis `Add Reference`. Cliquez sur `Browse...` et chosissez le fichier DLL que vous venez de télécharger.   
Voilà, vous pouvez utiliser la librairie !

#### MonoDevelop

Allez dans le menu `Projet` puis `Modifier les références`. Cliquez sur l'onglet `.Net Assembly`, ensuite cliquez sur `Parcourir`. Choisissez le fichier DLL que vous venez de télécharger et enfin cliquez sur `Valider`.   
Voilà, vous pouvez utiliser la librairie !

### Clonage de la librairie

Si vous voulez avoir tout le code, vous pouvez cloner le repository suivant (vous pouvez l'importer sur Visual Studio ou sur MonoDevelop) :
```
git clone https://git.ares-ensiie.eu/hackathon/csharp-client
```

## Partie 3: Exemple

Le fichier `Exemple.cs` donne un exemple commenté du client C#. Ce client va jouer des coups aléatoires jusqu'à ce qu'il gagne ou qu'il perde.   
Vous pouvez le lancer depuis n'importe quel IDE après avoir importé la librairie.  

**Attention sous MonoDevelop, par défaut, la console d'exécution n'est pas affichée. Pour l'afficher, faites un clic droit sur votre projet puis cliquez sur `Options`. Choisissez le menu `Exécuter` puis le sous-menu `Général` et cochez `Exécuter sur une console externe`. Pensez à le faire en Debug et en Release.**


## Partie 4: Code source et documentation

### Code source

Le code source du client C# peut être trouvé sur le Gitlab dans le projet [hackathon/csharp-client](https://git.ares-ensiie.eu/hackathon/csharp-client)

### Documentation

La documentation est disponible au format HTML dans le dossier doc, vous avez juste à lancer le fichier `index.html`.  
Vous pouvez la retrouver aussi ici : [doc-csharp](https://perso.ares-ensiie.eu/miclo2018/doc_csharp)
