import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const usePlayerStore = defineStore('player', () => {
    let currentId: number = 0;

    function generateId(): number {
      return ++currentId;
    }

    const addPlayer = (player: Player) => {
        players.value.push(player)
    }

    const deletePlayer = async (playerId: number) => {
        const index = players.value.findIndex((player) => player.id === playerId);
        if (index !== -1) {
          players.value.splice(index, 1);
      }
    }

    const players = ref<Player[]>([]);

    const loadPlayers = async () => {
      players.value = await $fetch<Player[]>('http://localhost:5181/api/Players')
    }



      const generateRanks = () => {
        const sortedPlayers = players.value.slice().sort((a, b) => b.points - a.points);
    
        sortedPlayers.forEach((player, index) => {
          player.rank = index + 1;
        });

        players.value = sortedPlayers;
      };
      
      const teamStore = useTeamStore();

      const addPointsToWinningTeam = (winningTeam: string) => {
      const team = teamStore.teams.find(t => t.teamname === winningTeam);
      if (team) {
        players.value.forEach((player) => {
        if (team.members.includes(player)) {
        player.points = Number(player.points) + 1;
        }
      });
    
    generateRanks();
    } else {
      console.error(`Team ${winningTeam} not found`);
    }
    };

    return { players, generateId, addPlayer, deletePlayer, generateRanks, addPointsToWinningTeam, loadPlayers };
  })