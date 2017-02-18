module Ares.Renforts

open System
open Influence
open Ares.Explo

let renforce (field:InfluenceField) (client:InfluenceClient) unitsToAdd =
<<<<<<< HEAD
      (let myCells = client.GetMyCells()
=======
      let myCells = client.GetMyCells()
>>>>>>> a9ea0b21700bbc72a6ef0861e338611d75ad6158
      for renfortNum = 1 to unitsToAdd do
         let cell = myCells.[rng.Next(myCells.Count)]
         client.AddUnits(cell, 1))
