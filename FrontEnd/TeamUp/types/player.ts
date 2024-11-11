import type { Team } from "./team"

export type Player = {
    Id: number
   Name: string
   Points: number
   //Rank: number
   Team?: Team
}

export type RankedPlayer = Player & {
    rank: number
}