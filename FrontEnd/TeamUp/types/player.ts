import type { Team } from "./team"

export type Player = {
    id: number
   name: string
   points: number
   rank: number
   team?: Team
}
