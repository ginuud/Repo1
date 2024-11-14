import type { Team } from "./team"

export type Game = {
    id: number
   name: string
   teams: Team[]
   status: 'in progress' | 'inactive'
}