import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player, RankedPlayer } from "~/types/player";

export const usePlayerStore = defineStore('player', () => {
  let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const players = ref<Player[]>([]);

    const loadPlayers = async () => {
      players.value = await $fetch<Player[]>('http://localhost:5181/api/Players');
    };
  
    const rankedPlayers = computed<RankedPlayer[]>(() => {
      const sortedPlayers = [...players.value].sort((a, b) => b.Points - a.Points);
  
      let currentRank = 1;
      let previousPoints: number | null = null;
  
      return sortedPlayers.map((player, index) => {
        if (previousPoints !== player.Points) {
          currentRank = index + 1;
        }
        previousPoints = player.Points;
  
        return {
          ...player,
          rank: currentRank
        };
      });
    });

  

  const addPlayer = async (Player: Player) => {
    const res = await $fetch('http://localhost:5181/api/Players', {
      method: 'POST',
      body: Player,
    });
    players.value.push(res);

    //generateRanks();
  };
  // const addPlayer = async (player: Player) => {
  //   try {
  //     const response = await $fetch('http://localhost:5181/api/Players', {
  //       method: 'POST',
  //       body: {
  //         Name: player.Name,
  //         Points: player.Points,
  //         TeamId: player.Team
  //       }
  //     });
      
  //     await loadPlayers(); // Reload all players to ensure correct order
  //   } catch (error) {
  //     console.error('Error adding player:', error);
  //   }
  // };


  // const deletePlayer = async (playerId: number) => {
  //   await $fetch(`http://localhost:5181/api/Players/${playerId}`, {
  //     method: 'DELETE',
  //   });
  //     const index = players.value.findIndex((player) => player.Id === playerId);
  //     if (index !== -1) {
  //       players.value.splice(index, 1);
  //   }
  // };
  const deletePlayer = async (playerId: number) => {
    try {
      await $fetch(`http://localhost:5181/api/Players/${playerId}`, {
        method: 'DELETE'
      });
      await loadPlayers(); // Reload players after deletion
    } catch (error) {
      console.error('Error deleting player:', error);
    }
  };

  // const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number) => {
  //   const updateData = {
  //     name: newName,
  //     points: newScore
  //   };
    
  //   await $fetch(`http://localhost:5181/api/Players/${selectedPlayerId}`, {
  //     method: 'PUT',
  //     body: updateData,
  //   });

  //   const player = players.value.find(p => p.Id === selectedPlayerId);
  //   if(player){
  //     player.Name = newName;
  //     player.Points = newScore;
  //   }
  //   //generateRanks();
  // };

  const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number) => {
    try {
      await $fetch(`http://localhost:5181/api/Players/${selectedPlayerId}`, {
        method: 'PUT',
        body: {
          Name: newName,
          Points: newScore
        }
      });
      await loadPlayers(); // Reload players after update
    } catch (error) {
      console.error('Error updating player:', error);
    }
  };
  

  // const loadPlayers = async () => {
  //   players.value = await $fetch<Player[]>('http://localhost:5181/api/Players');
  //   //generateRanks();
  // };
  // const loadPlayers = async () => {
  //   try {
  //     const response = await $fetch<Player[]>('http://localhost:5181/api/Players');
  //     players.value = response;
  //   } catch (error) {
  //     console.error('Error loading players:', error);
  //     players.value = [];
  //   }
  // };

  // const generateRanks = () => {
  //   const sortedPlayers = players.value.slice().sort((a, b) => b.Points - a.Points);
  //   sortedPlayers.forEach((player, index) => {
  //     player.Rank = index + 1;
  //   });
  //   players.value = sortedPlayers;
  // };
  
  const teamStore = useTeamStore();

  // const addPointsToWinningTeam = (winningTeam: string) => {
  // const team = teamStore.teams.find(t => t.Name === winningTeam);
  // if (team) {
  //   players.value.forEach((player) => {
  //     if (team.Members.includes(player)) {
  //     player.Points = Number(player.Points) + 1;
  //     }
  //   });
  // //generateRanks();
  // }
  // else {
  //   console.error(`Team ${winningTeam} not found`);
  // }
  // };

  const addPointsToWinningTeam = async (winningTeam: string) => {
    try {
      const team = await useTeamStore().teams.find(t => t.Name === winningTeam);
      
      if (team) {
        // Find all players in the winning team
        const teamPlayers = players.value.filter(player => player.Team === team.Id);
        
        // Update points for each player in the winning team
        for (const player of teamPlayers) {
          await updatePlayer(
            player.Id,
            player.Name,
            Number(player.Points) + 1
          );
        }
        
        // Reload players to update rankings
        await loadPlayers();
      } else {
        console.error(`Team ${winningTeam} not found`);
      }
    } catch (error) {
      console.error('Error updating winning team points:', error);
    }
  };

  return { players: rankedPlayers, generateId, addPlayer, deletePlayer, addPointsToWinningTeam, loadPlayers, updatePlayer};
});