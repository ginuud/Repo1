import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useAuth } from '~/composables/useAuth';
import type { Player } from "~/types/player";

export const usePlayerStore = defineStore("player", () => {
  let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const auth = useAuth();
  const players = ref<Player[]>([]);

  const loadPlayers = async () => {
    try {
      players.value = await auth.fetchWithToken<Player[]>("Players");
      generateRanks(); 
      players.value.sort((a, b) => a.rank - b.rank);
    } catch (error) {
      console.error('Error loading players:', error);
      players.value = [];
    }
  };
  
  const generateRanks = () => {
    players.value
      .slice()
      .sort((a, b) => {
        return b.points - a.points;
      })
      .forEach((player, index) => {
        player.rank = index + 1;
      });
  };  
  
  const addPlayer = async (player: Player) => {
    const res = await auth.fetchWithToken("Players", {
      method: 'POST',
      body: player,
    });
    players.value.push(res);
    loadPlayers();
  };

  const deletePlayer = async (playerId: number) => {
    try {
      await auth.fetchWithToken(`Players/${playerId}`, {
        method: 'DELETE'
      });
      await loadPlayers(); 
    } catch (error) {
      console.error('Error deleting player:', error);
    }
    loadPlayers();
  };

  const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number, teamId: number | null) => { 
    try {
      console.log('teamId:', teamId);
      await auth.fetchWithToken(`Players/${selectedPlayerId}`, {
        method: 'PUT',
        body: {
          name: newName,
          points: newScore,
          teamId: teamId,
        },
      });
      await loadPlayers(); 
    } 
    catch (error) {
      console.error('Error updating player:', error);
    }
  }; 

  return { players, generateId, addPlayer, deletePlayer, loadPlayers, updatePlayer, generateRanks};
});