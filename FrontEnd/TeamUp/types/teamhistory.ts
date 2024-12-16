import type { GameHistory } from "./gamehistory";
import type { Player } from "./player";
export type TeamHistory = {
    id: number
    name: string
    members?: Player[]
    gameHistoryId?: number | null;
    gameHistory?: GameHistory
  };