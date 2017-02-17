module Ares

open System
open Influence

[<EntryPoint>]
let main argv =
   let r = new System.Random()
   let client = InfluenceClient.GetInstance()
   //client.Connect("127.0.0.1", "les DauF#ins Surfeurs")
   client.Connect("10.3.4.37", "les DauF#ins Surfeurs");
   let mutable field = client.NextRound()
   let mutable myCells = client.GetMyCells()

   while client.GetStatus() = InfluenceClient.Status.ONGOING do
      printfn "%A - Attacking" DateTime.Now
      for i = 0 to 10 do
         myCells <- client.GetMyCells()
         let c = myCells.[r.Next(myCells.Count)]
         if c.GetUnitsCount() >= 2 then 
            let dx = c.GetX() + r.Next(3) - 1
            let dy = c.GetY() + r.Next(3) - 1
            if dx >= 0 && dx < field.GetWidth() && dy >= 0 && dy < field.GetHeight() then 
               let cellToAttack = field.GetCell(dx, dy)
               if (isNull cellToAttack) && (cellToAttack.GetOwner() <> client.GetNumber()) then 
                  field <- client.Attack(c.GetX(), c.GetY(), cellToAttack.GetX(), cellToAttack.GetY())

      let unitsToAdd = client.EndAttacks()
      myCells <- client.GetMyCells()
      for i = 0 to unitsToAdd do
         let c = myCells.[r.Next(myCells.Count)]
         client.AddUnits(c, 1)
         field <- client.EndAddingUnits()

      match client.GetStatus() with 
      | InfluenceClient.Status.VICTORY -> printfn "YOU WON!"
      | InfluenceClient.Status.DEFEAT -> printfn "YOU LOST!"
      | InfluenceClient.Status.CONNECTION_LOST -> printfn "YOU LOST BECAUSE OF YOUR CONNECTION"
      | _ -> printfn "NOT REACHABLE"
      System.Console.ReadKey() |> ignore
      field <- client.NextRound()
   0 // return an integer exit code
