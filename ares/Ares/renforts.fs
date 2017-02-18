module Ares.Renforts

open System
open Influence
open Ares.Explo

let renforce (field:InfluenceField) (client:InfluenceClient) unitsToAdd =
      (let myCells = client.GetMyCells()
      for renfortNum = 1 to unitsToAdd do
         let cell = myCells.[rng.Next(myCells.Count)]
         client.AddUnits(cell, 1))