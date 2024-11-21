import type { Game } from "./game";
import type { Player } from "./player";
export type Team = {
    id: number
    name: string
    members?: Player[]
    gameId?: number | null;
    game?: Game
  };