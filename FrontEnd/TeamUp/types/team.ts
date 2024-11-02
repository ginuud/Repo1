import type { Game } from "./game";
import type { Player } from "./player";
export type Team = {
    Id: number
    Name: string
    Members?: Player[]
    Game?: Game
  };