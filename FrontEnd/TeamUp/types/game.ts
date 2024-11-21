import type { Team } from "./team"

export type Game = {
    id: number;
   name: string;
   teams: Team[];
   deleteTeams?: boolean;
};