module Ares

open System
open Influence
open Ares.Explo
open Ares.Monte

[<EntryPoint>]
let main argv =
   let rng = new System.Random()
   let client = InfluenceClient.GetInstance()
   client.Connect("127.0.0.1", "les DauF#ins Surfeurs")
   //client.Connect("10.3.4.37", "les DauF#ins Surfeurs")
   let mutable field = client.NextRound()
   let mutable myCells = client.GetMyCells()

   while client.GetStatus() = InfluenceClient.Status.ONGOING do

      printfn "%A - Attacking" DateTime.Now
      for attackNumber = 1 to 20 do
         myCells <- client.GetMyCells()
         let cell = myCells.[rng.Next(myCells.Count)]
         if cell.GetUnitsCount() >= 2 then 
            let dx = cell.GetX() + rng.Next(3) - 1
            let dy = cell.GetY() + rng.Next(3) - 1
            if dx >= 0 && dx < field.GetWidth() && dy >= 0 && dy < field.GetHeight() then 
               let cellToAttack = field.GetCell(dx, dy)
               if (isNull cellToAttack |> not) && (cellToAttack.GetOwner() <> client.GetNumber()) then 
                  field <- client.Attack(cell.GetX(), cell.GetY(), cellToAttack.GetX(), cellToAttack.GetY())
      printfn "out"
      let unitsToAdd = client.EndAttacks()
      myCells <- client.GetMyCells()
      for renfortNum = 1 to unitsToAdd do
         let cell = myCells.[rng.Next(myCells.Count)]
         client.AddUnits(cell, 1)
      field <- client.EndAddingUnits()

      match client.GetStatus() with 
      | InfluenceClient.Status.VICTORY -> printfn "YOU WON!"
      | InfluenceClient.Status.DEFEAT -> printfn "YOU LOST!"
      | InfluenceClient.Status.CONNECTION_LOST -> printfn "YOU LOST BECAUSE OF YOUR CONNECTION"
      | _ -> printfn "NOT REACHABLE"
      field <- client.NextRound()
   0 // return an integer exit code
