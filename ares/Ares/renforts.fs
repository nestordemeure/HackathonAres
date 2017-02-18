module Ares.Renforts

open System
open Influence

let renforce field client unitsToAdd =
      let myCells = client.GetMyCells()
      for renfortNum = 1 to unitsToAdd do
         let cell = myCells.[rng.Next(myCells.Count)]
         client.AddUnits(cell, 1)