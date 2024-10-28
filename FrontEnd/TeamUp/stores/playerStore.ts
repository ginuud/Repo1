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

    const players = ref<Player[]>([
        {id: generateId(), name: 'Ivo Linna', points: 0, rank: 10 },
        {id: generateId(), name: 'Arvo Pärt', points: 15, rank: 4 },
        {id: generateId(), name: 'Kristjan Jõekalda', points: 0, rank: 9 },
        {id: generateId(), name: 'Ott Sepp', points: 0, rank: 8 },
        {id: generateId(), name: 'Jüri Ratas', points: 6, rank: 5 },
        {id: generateId(), name: 'Koit Toome', points: 0, rank: 7 },
        {id: generateId(), name: 'Anu Saagim', points: 23, rank: 1 },
        {id: generateId(), name: 'Evelin Ilves', points: 0, rank: 6 },
        {id: generateId(), name: 'Marko Reikop', points: 20, rank: 2 },
        {id: generateId(), name: 'Mihkel Raud', points: 19, rank: 3 },
      ]);

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

    return { players, generateId, addPlayer, deletePlayer, generateRanks, addPointsToWinningTeam };
  })