module Ares.Renforts

open System
open Influence
open Ares.Explo

/// the id for this 
let mutable playerId = 0
let champDeVision = 2

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

/// put x at the end of the list
let rec insertLast l x =
   match l with 
   | [] -> [x]
   | t::q -> t::(insertLast q x)

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

let renforce (field:InfluenceField) (client:InfluenceClient) unitsToAdd =
      let mutable scouts = []
      let mutable fighters = []
      for x = 0 to field.GetWidth() - 1 do 
         for y = 0 to field.GetHeight() - 1 do 
            let cell = field.GetCell(x,y)
            if cell.GetOwner() = playerId then 
               match evaluate x y field with 
               | Fighter -> fighters <- (x,y)::fighters
               | Scout -> scouts <- (x,y)::scouts
               | Dead -> ()
      let mutable renfortNum = 1
      let mutable stop = false
      let mutable renforceThis = true
      // scouts (1 sur 2)
      while renfortNum < unitsToAdd && not stop do 
         match scouts with 
         | [] -> stop <- true
         | (x,y)::q ->
            if renforceThis then
                  client.AddUnits(field.GetCell(x,y), 1)
            renfortNum <- renfortNum + 1
            renforceThis <- not renforceThis
            scouts <- q
      // fighters
      stop <- false
      while renfortNum < unitsToAdd && not stop do 
         match fighters with 
         | [] -> stop <- true
         | (x,y)::q -> 
            client.AddUnits(field.GetCell(x,y), 1)
            renfortNum <- renfortNum + 1
            fighters <- insertLast q (x,y)