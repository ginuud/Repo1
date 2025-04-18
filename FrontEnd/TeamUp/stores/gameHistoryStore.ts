import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useAuth } from '~/composables/useAuth';
import type { GameHistory } from "~/types/gamehistory";

export const useGameHistoryStore = defineStore("gameHistory", () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const auth = useAuth();
    const gamesHistory = ref<GameHistory[]>([])

    const loadGamesHistory = async () => {
      gamesHistory.value = await auth.fetchWithToken<GameHistory[]>("GamesHistory")
    }
    
    const addGameHistory = async (game: Game, winner: string) => {
      const res = await auth.fetchWithToken("GamesHistory", {
        method: 'POST',
        body: {
          name: game.name,
          teams: game.teams,
          winner: winner
        },
      });
      gamesHistory.value.push(res);
      loadGamesHistory();
    };

    const deleteGameHistory = async (gameId: number) => {
      const res = await auth.fetchWithToken(`GamesHistory/${gameId}`, {
        method: 'DELETE',
      })
      gamesHistory.value = gamesHistory.value.filter(gameHistory => gameHistory.id !== gameId);
    }

    return { gamesHistory, generateId, addGameHistory, loadGamesHistory, deleteGameHistory };
  })