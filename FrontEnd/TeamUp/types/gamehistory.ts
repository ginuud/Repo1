import type { TeamHistory } from "./teamhistory"

export type GameHistory = {
    id: number;
   name: string;
   teamsHistory: TeamHistory[];
   winner: TeamHistory;
};