import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const usePlayerStore = defineStore('player', () => {
  let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const players = ref<Player[]>([]);

  const addPlayer = async (Player: Player) => {
    const res = await $fetch('http://localhost:5181/api/Players', {
      method: 'POST',
      body: Player,
    });
    players.value.push(res);

    generateRanks();
  }

  const deletePlayer = async (playerId: number) => {
      const index = players.value.findIndex((player) => player.Id === playerId);
      if (index !== -1) {
        players.value.splice(index, 1);
    }
  }
  const playerStore = usePlayerStore();

  const updatePlayer = (selectedPlayerId: number, newName: string, newScore: number) => {
    const player = playerStore.players.find(p => p.Id === selectedPlayerId);
    if(player){
      player.Name = newName;
      player.Points = newScore;
    }
  };

  const loadPlayers = async () => {
    players.value = await $fetch<Player[]>('http://localhost:5181/api/Players')
  }

  const generateRanks = () => {
    const sortedPlayers = players.value.slice().sort((a, b) => b.Points - a.Points);

    sortedPlayers.forEach((player, index) => {
      player.Rank = index + 1;
    });

    players.value = sortedPlayers;
  };
  
  const teamStore = useTeamStore();

  const addPointsToWinningTeam = (winningTeam: string) => {
  const team = teamStore.teams.find(t => t.Name === winningTeam);
  if (team) {
    players.value.forEach((player) => {
    if (team.Members.includes(player)) {
    player.Points = Number(player.Points) + 1;
    }
  });
  
  generateRanks();
  } else {
    console.error(`Team ${winningTeam} not found`);
  }
  };

  return { players, generateId, addPlayer, deletePlayer, generateRanks, addPointsToWinningTeam, loadPlayers, updatePlayer};
})