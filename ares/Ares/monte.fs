module Ares.Monte

open Influence
open Ares.Explo

let rayon = 2
let profondeur = 2
let temps = 3000.
let newField = Array.init (2*rayon+1) (fun i -> Array.zeroCreate (2*rayon+1))
let mutable notrecamp = []
let mutable adversecamp = []

//-------------------------------------------------------------------------------------------------

let reproduitField (field:InfluenceField) (client:InfluenceClient) x y =
   adversecamp <- []
   notrecamp <- []
   for h = 0 to newField.Length do
      for l = 0 to (newField.[0]).Length do
         let cell = field.GetCell(x-rayon+h,y-rayon+l)
         if (isNull cell) then
            newField.[h].[l] <- System.Int32.MinValue
         elif cell.GetOwner() <> client.GetNumber() then
            newField.[h].[l] <- - cell.GetUnitsCount()
            adversecamp <- (h,l)::adversecamp
         else
            newField.[h].[l] <- cell.GetUnitsCount()
            notrecamp <- (h,l)::notrecamp

let rec retire (x,y) liste =
   match liste with
   |[] -> liste
   |(a,b)::q -> if a = x && b = y then q else (a,b)::(retire (x,y) q)

let simuleAttaque x y =
   let dx = x + rng.Next(3) - 1
   let dy = y + rng.Next(3) - 1
   if dx >= 0 && dx < newField.Length && dy >= 0 && dy < newField.[0].Length then
      let cell = newField.[x].[y] - 1
      let cellToAttack = newField.[dx].[dy]
      //Si la cellule est valide
      if cellToAttack <> System.Int32.MinValue then
         //Si on a des attaquants et que la cellule attaquée est adverse
         if cell > 0 && cellToAttack <= 0 then
            newField.[x].[y] <- 1
            newField.[dx].[dy] <- cell + cellToAttack
            //Si on gagne, notre camp a gagné une cellule, l'autre camp en a perdu une
            if newField.[dx].[dy] > 0 then
               notrecamp <- (dx,dy)::notrecamp
               adversecamp <- retire (dx,dy) adversecamp

let simuleAttaqueMechant x y =
   let dx = x + rng.Next(3) - 1
   let dy = y + rng.Next(3) - 1
   if dx >= 0 && dx < newField.Length && dy >= 0 && dy < newField.[0].Length then
      let cell = newField.[x].[y] + 1
      let cellToAttack = newField.[dx].[dy]
      if cellToAttack <> System.Int32.MinValue then
         //Si on a des attaquants et que la cellule attaquée est adverse
         if cell < 0 && cellToAttack >= 0 then
            newField.[x].[y] <- -1
            newField.[dx].[dy] <- cell + cellToAttack
            //Si le méchant gagne, le méchant a gagné une cellule, notre camp en a perdu une
            if newField.[dx].[dy] < 0 then
               adversecamp <- (dx,dy)::adversecamp
               notrecamp <- retire (dx,dy) notrecamp
let campJoue nomCamp =
   for joueur in nomCamp do
      let x, y = joueur
      simuleAttaque x y

let campJoueMechant advCamp =
   for joueur in advCamp do
      let x, y = joueur
      simuleAttaqueMechant x y

let premierCoup x y dx dy =
   let cell = newField.[x].[y] - 1
   let cellToAttack = newField.[dx].[dy]
   newField.[x].[y] <- 1
   newField.[dx].[dy] <- cell + cellToAttack
   //Si on gagne, notre camp a gagné une cellule, l'autre camp en a perdu une
   if newField.[dx].[dy] > 0 then
      notrecamp <- (dx,dy)::notrecamp
      adversecamp <- retire (dx,dy) adversecamp
let calculeScore =
   let nbrCases = notrecamp.Length
   let diffNbrUnites = List.sumBy (fun (i,j) -> newField.[i].[j]) notrecamp + List.sumBy (fun (i,j) -> newField.[i].[j]) adversecamp
   (nbrCases,diffNbrUnites)

let simulePartie x y coup profondeur =
   let dx,dy = coup
   premierCoup x y dx dy
   for numCoup = 1 to profondeur do
      campJoue notrecamp
      campJoueMechant adversecamp
   calculeScore

let somme (a,b) (x,y) = (a+x,b+y)
let division (a,b) x = (a/x,b/x)

//-------------------------------------------------------------------------------------------------
let monte x y (field:InfluenceField) (client:InfluenceClient) =
   let stopWatch = System.Diagnostics.Stopwatch.StartNew()

   reproduitField field client x y

   let mutable newX = rayon
   let mutable newY = rayon

   let coupsPossibles =
      [|
         for i=(-1) to 1 do
            for j=(-1) to 1 do
               if not(i = 0 && j = 0) then
                  let coupX = newX+i
                  let coupY = newY+j
                  if newField.[coupX].[coupY] <> System.Int32.MinValue then
                     if newField.[coupX].[coupY] <= 0 then 
                        yield(coupX, coupY)
      |]
   let nbrCoups = coupsPossibles.Length
   let mutable scores = Array.create nbrCoups (0,0)
   let mutable coupsPasses = Array.zeroCreate nbrCoups

   let mutable coupN = -1

   while stopWatch.Elapsed.TotalMilliseconds < temps do
      coupN <- coupN + 1
      let indiceCoup = coupN%nbrCoups
      let coup = coupsPossibles.[indiceCoup]
      let score = simulePartie newX newY coup profondeur
      scores.[indiceCoup] <- somme score scores.[indiceCoup]
      coupsPasses.[indiceCoup] <- coupsPasses.[indiceCoup] + 1
      reproduitField field client x y

   let mutable maxX,maxY = (0,0)
   let mutable indice = 0
   for i=0 to nbrCoups do
      if coupsPasses.[i] = 0 then
         scores.[i] <- (System.Int32.MinValue,System.Int32.MinValue)
      else
         scores.[i] <- division scores.[i] coupsPasses.[i]
         let a,b = scores.[i]
         if a > maxX then
            maxX <- a
            maxY <- b
            indice <- i
         elif a = maxX && b > maxY then
            maxX <- a
            maxY <- b
            indice <- i

   coupsPossibles.[indice] |> Some

