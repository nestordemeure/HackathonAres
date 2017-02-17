module Ares.Monte

open Influence
open Ares.Explo

//-------------------------------------------------------------------------------------------------


//-------------------------------------------------------------------------------------------------

let monte x y (field:InfluenceField) (client:InfluenceClient) profondeur temps rayon =
   let cell = field.GetCell(x, y)
   let dx = cell.GetX() + rng.Next(3) - 1
   let dy = cell.GetY() + rng.Next(3) - 1
   if dx >= 0 && dx < field.GetWidth() && dy >= 0 && dy < field.GetHeight() then 
      let cellToAttack = field.GetCell(dx, dy)
      if (isNull cellToAttack |> not) && (cellToAttack.GetOwner() <> client.GetNumber()) then 
         client.Attack(cell.GetX(), cell.GetY(), cellToAttack.GetX(), cellToAttack.GetY())
      else field
   else field
