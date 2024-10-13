import type { Team } from "./team"

export type Game = {
    id: number
   name: string
   team1name: Team
   team2name: Team
   status: 'in progress' | 'inactive'
}