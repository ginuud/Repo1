import type { Team } from "./team"

export type Game = {
    Id: number
   Name: string
   Teams: Team[]
   Status: 'in progress' | 'inactive'
}