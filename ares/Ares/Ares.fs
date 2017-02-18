module Ares.Main

open System
open Influence
open Ares.Explo
open Ares.Monte
open Ares.Renforts

/// the id for this 
let mutable playerId = 0
let champDeVision = 2

//-------------------------------------------------------------------------------------------------

/// print a status
let printStatus (client:InfluenceClient) =
   match client.GetStatus() with 
   | InfluenceClient.Status.VICTORY -> printfn "YOU WON!"
   | InfluenceClient.Status.DEFEAT -> printfn "YOU LOST!"
   | InfluenceClient.Status.CONNECTION_LOST -> printfn "YOU LOST BECAUSE OF YOUR CONNECTION"
   | _ -> printfn "NOT REACHABLE"

/// put x at the end of the list
let rec insertLast l x =
   match l with 
   | [] -> [x]
   | t::q -> t::(insertLast q x)

//-------------------------------------------------------------------------------------------------

type CellType = Fighter | Scout | Dead

let isEnemy (cell:InfluenceCell) = 
   let owner = cell.GetOwner()
   (owner <> 0) && (owner <> playerId) && (cell.GetUnitsCount() > 1)

// forbid mine
let voisins (field:InfluenceField) rayon x y =
   seq {
      for x2 = max 0 (x-rayon) to min (x+rayon) (field.GetWidth()-1) do 
         for y2 = max 0 (y-rayon) to min (y+rayon) (field.GetHeight()-1) do 
            if x<>x2 || y<>y2 then yield (x2,y2)
   }

let circle (field:InfluenceField) rayon x y =
   seq {
      if x-rayon >= 0 then 
         let x2 = x-rayon
         for y2 = max 0 (y-rayon) to min (y+rayon) (field.GetHeight()-1) do 
            if x<>x2 || y<>y2 then yield (x2,y2)
      if x+rayon < field.GetWidth() then 
         let x2 = x+rayon
         for y2 = max 0 (y-rayon) to min (y+rayon) (field.GetHeight()-1) do 
            if x<>x2 || y<>y2 then yield (x2,y2)
      if y-rayon >= 0 then 
         let y2 = y-rayon
         for x2 = max 0 (x-rayon) to min (x+rayon) (field.GetWidth()-1) do
            if x<>x2 || y<>y2 then yield (x2,y2)
      if y+rayon < field.GetHeight() then 
         let y2 = y+rayon
         for x2 = max 0 (x-rayon) to min (x+rayon) (field.GetWidth()-1) do
            if x<>x2 || y<>y2 then yield (x2,y2)
   }

//-----

let evaluate x y (field:InfluenceField) = 
   let cell = field.GetCell(x,y)
   let mutable result = Dead
   if cell.GetUnitsCount() > 1 then
      let voisinProches = circle field 1 x y 
      if Seq.exists (fun (x2,y2) -> field.GetCell(x2,y2).GetOwner() <> playerId) voisinProches then
         let voisinsRayon = voisins field champDeVision x y
         if Seq.exists (fun (x2,y2) -> isEnemy <| field.GetCell(x2,y2)) voisinsRayon 
         then result <- Fighter
         else result <- Scout
   result

//-------------------------------------------------------------------------------------------------
// MAIN

[<EntryPoint>]
let main argv =
   let client = InfluenceClient.GetInstance()
   client.Connect("127.0.0.1", "les DauF#ins Surfeurs")
   playerId <- client.GetNumber()
   //client.Connect("10.3.4.37", "les DauF#ins Surfeurs") // test Ares
   let mutable field = client.NextRound()
   while client.GetStatus() = InfluenceClient.Status.ONGOING do
      printfn "%A - Attacking" DateTime.Now
      // attaques
      // collecte les cellules
      let mutable fighters = []
      let mutable scouts = []
      for x = 0 to field.GetWidth() - 1 do 
         for y = 0 to field.GetHeight() - 1 do 
            let cell = field.GetCell(x,y)
            if cell.GetOwner() = playerId then 
               match evaluate x y field with 
               | Fighter -> fighters <- (x,y)::fighters
               | Scout -> scouts <- (x,y)::scouts
               | Dead -> ()
      // rédiger des fonctions qui consomme un scout ou un fighter pour plus de finesse
      // gère les attaques 
      let mutable attackNumber = 0
      let mutable stop = false
      // scouts 
      while attackNumber < 20 && not stop do 
         match scouts with 
         | [] -> stop <- true
         | (x,y)::q when evaluate x y field = Dead -> scouts <- q // utile uniquement si on utilie insertLast
         | (x,y)::q -> 
            match explo x y field client with 
            | None -> scouts <- q 
            | Some (x2,y2) ->
               field <- client.Attack(x,y,x2,y2)
               attackNumber <- attackNumber + 1
               if field.GetCell(x2,y2).GetOwner() = playerId then 
                  scouts <- insertLast q (x2,y2)
               else 
                  scouts <- q
      // fighters
      stop <- false
      let stopwatch = System.Diagnostics.Stopwatch.StartNew()
      let mutable listSize = List.length fighters |> float
      let mutable timeLeft = 4500.
      while attackNumber < 20 && not stop do 
         match fighters with 
         | [] -> stop <- true
         | (x,y)::q -> 
            let temps = timeLeft / listSize
            stopwatch.Restart()
            match monte stopwatch temps x y field client with 
            | None -> 
               stop <- true // TODO : les attaquant DOIVENT attaquer
               fighters <- q 
            | Some (x2,y2) ->
               printfn "??? xy %d %d | x2y2 %d %d" x y x2 y2
               field <- client.Attack(x,y,x2,y2)
               attackNumber <- attackNumber + 1
               if (field.GetCell(x2,y2).GetOwner() = playerId) && (field.GetCell(x2,y2).GetUnitsCount() > 1) then 
                  listSize <- listSize + 1.
                  fighters <- (x2,y2)::q
                  printfn "attack okay"
               else 
                  fighters <- q
            timeLeft <- timeLeft - stopwatch.Elapsed.TotalMilliseconds
      let unitsToAdd = client.EndAttacks()
      // renforts
      renforce field client unitsToAdd
      field <- client.EndAddingUnits()
      // end-turn
      field <- client.NextRound()
   printStatus client ; 0
