import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Game } from "~/types/game";

export const useGameStore = defineStore('game', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const games = ref<Game[]>([])

    const loadGames = async () => {
      games.value = await $fetch<Game[]>('http://localhost:5181/api/Games')
    }
    
    const addGame = async (game: Game) => {
      const res = await $fetch('http://localhost:5181/api/Games', {
        method: 'POST',
        body: game,
      });
      games.value.push(res)
    }

    const deleteGame = async (gameId: number) => {
      const res = await $fetch('http://localhost:5181/api/Games/' + gameId, {
        method: 'DELETE',
      })
      games.value = games.value.filter(game => game.id !== gameId);
    }

    const makeStatusInactive = (id: number, status: "in progress") => {
      const game = games.value.find(game => game.id === id);
      if (game) {
        game.status = "inactive";
      } 
      else {
        console.error(`Game with id ${id} not found`);
      }
    }

    return { games, generateId, addGame, makeStatusInactive, loadGames, deleteGame };
  })