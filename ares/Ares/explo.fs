module Ares.Explo

open Influence

let rng = new System.Random()
let distanceDeSec = 2
let valueNeutral = 1
let valueUnique = 3
let valueDistant = 1
let valueDangerousDistant = -6

//-------------------------------------------------------------------------------------------------


//-------------------------------------------------------------------------------------------------  
let valueCell x y (field:InfluenceField) (client:InfluenceClient) =
   if field.GetCell(x, y).GetOwner() = client.GetNumber() then -1 
   else
      let mutable value = 0
         (* let mutable value = if field.GetCell(x, y).GetUnitsCount() <> 0 then 1 else 0*)
      for ix = max 0 (x-distanceDeSec) to min (x+distanceDeSec) (field.GetWidth()-1) do
         for iy = max 0 (y-distanceDeSec) to min (y+distanceDeSec) (field.GetHeight()-1) do
            if ix<>x || iy<>y then
                  let presentUnit = field.GetCell(x, y).GetUnitsCount()
                  if presentUnit = 0 then
                     value <- value + valueNeutral
                  elif presentUnit = 1 then 
                     value <- value + valueUnique
                  elif (presentUnit - max (abs (ix - x)) (abs (iy - y))) < 0 then
                     value <- value + valueDistant
                  elif max (abs (ix - x)) (abs (iy - y)) = 2 then
                     value <- valueDangerousDistant
                  else
                     value <- -1000
      value

let getVoisin x y (field:InfluenceField) =
   [
      for ix = (x-1) to (x+1) do
         for iy = (y-1) to (y+1) do
            if (ix > -1) && (iy > -1) &&
               (ix <> field.GetWidth()) && (iy <> field.GetHeight()) &&
               (ix <> x && iy <> y) then
               yield (ix,iy)
   ]

let explo x y (field:InfluenceField) (client:InfluenceClient) =
   let mutable bestCell = (0, 0)
   let mutable bestValue = -1
   for ix = max 0 (x-1) to min (x+1) (field.GetWidth()-1) do
      for iy = (y-1) to min (y+1) (field.GetHeight()-1) do
         if ix<>x || iy<>y then
            let value = valueCell ix iy field client
            if value > bestValue then
               bestValue <- value
               bestCell <- (ix, iy)
   if bestValue > -1 then Some bestCell else None

(*let GetEnemyCamp (field:InfluenceField) clientNumber
   let mutable enemyCells = (0, 0)
   for ix = 0 to (field.GetWidth()-1) do
      for iy = 0 to min (field.GetHeight()-1) do

   *)
let valueCell2 x y (field:InfluenceField) (client:InfluenceClient) =
   if field.GetCell(x, y).GetOwner() = client.GetNumber() then -1 
   else
      let mutable value = 
         if field.GetCell(x, y).GetUnitsCount() <> 0 then 1 else 0
      for ix = max 0 (x-distanceDeSec) to min (x+distanceDeSec) (field.GetWidth()-1) do
         for iy = max 0 (y-distanceDeSec) to min (y+distanceDeSec) (field.GetHeight()-1) do
            if ix<>x || iy<>y then
                  let presentUnit = field.GetCell(x, y).GetUnitsCount()
                  if presentUnit = 0 then 
                     value <- value + 1
                  elif presentUnit = 1 then 
                     value <- value + 3
                  elif (presentUnit - max (abs (ix - x)) (abs (iy - y))) < 0 then
                     value <- value + 2
                  else
                     value <- -1000
      value

<<<<<<< HEAD
let explo2 x y (field:InfluenceField) (client:InfluenceClient) =
   let mutable bestCell = (0, 0)
   let mutable bestValue = -1
   for ix = max 0 (x-1) to min (x+1) (field.GetWidth()-1) do
      for iy = (y-1) to min (y+1) (field.GetHeight()-1) do
         if ix<>x || iy<>y then
            let value = valueCell2 ix iy field client
            if value > bestValue then
               bestValue <- value
               bestCell <- (ix, iy)
   if bestValue > -1 then Some bestCell else None
=======
let explo x y (field:InfluenceField) (client:InfluenceClient) =
   let cell = field.GetCell(x, y)
   let dx = cell.GetX() + rng.Next(3) - 1
   let dy = cell.GetY() + rng.Next(3) - 1
   if dx >= 0 && dx < field.GetWidth() && dy >= 0 && dy < field.GetHeight() then 
      let cellToAttack = field.GetCell(dx, dy)
      if (isNull cellToAttack |> not) && (cellToAttack.GetOwner() <> client.GetNumber()) then 
         Some (dx,dy)
      else None
   else None
>>>>>>> 795541d81ad7698676b0bbedea8c03c0848734e4
