import { defineStore } from 'pinia';
import { ref } from 'vue';
import SelectWinner from '~/pages/select-winner.vue';
import type { Game } from "~/types/game";

export const useGameStore = defineStore('game', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const games = ref<Game[]>([
      {id: generateId(), name: 'Jalgpall', team1name: 'A', team2name: 'B', status: 'in progress', winner: null },
      {id: generateId(), name: 'Korvpall', team1name: 'A', team2name: 'B', status: 'inactive', winner: 'A'},
    ]);
    const addGame = (game: Game) => {
      games.value.push(game)
    }

    const makeStatusInactive = (id: number, //status: "in progress"

    ) => {
    const game = games.value.find(game => game.id === id);
    if (game) {
      Object.assign(game, { status: 'inactive' });
      //game.status = "inactive";
    } 
    else {
      console.error(`Game with id ${id} not found`);
    }
    }

    const setWinner = (id: number, winner: string) => {
      const game = games.value.find(game => game.id === id);
      if (game) {
        Object.assign(game, { winner });
        game.winner = winner;
      } else {
        console.error(`Game with id ${id} not found`);
      }
    };

    return { games, generateId, addGame, makeStatusInactive, setWinner };
  })