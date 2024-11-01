import type { Team } from "./team"

export type Game = {
    Id: number
   Name: string
   teams: Team[]
   status: 'in progress' | 'inactive'
}