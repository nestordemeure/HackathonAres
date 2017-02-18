module Ares.Explo

open Influence

let rng = new System.Random()

//-------------------------------------------------------------------------------------------------


//-------------------------------------------------------------------------------------------------

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