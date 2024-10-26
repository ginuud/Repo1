import type { Player } from "./player";
export type Team = {
    id: number;
    teamname: string;
    members: Player[];
  };