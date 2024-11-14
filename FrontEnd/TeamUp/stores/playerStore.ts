import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const usePlayerStore = defineStore('player', () => {
  let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const players = ref<Player[]>([]);

  const loadPlayers = async () => {
    try {
      players.value = await $fetch<Player[]>('http://localhost:5181/api/Players');
      console.log("Players loaded:", players.value); 
      generateRanks(); 
      players.value.sort((a, b) => a.Rank - b.Rank);
    } catch (error) {
      console.error('Error loading players:', error);
      players.value = [];
    }
  };
  
  const generateRanks = () => {
    console.log("Players before sorting:", players.value.map(player => ({
      name: player.name,
      points: player.points,
      rank: player.rank
    })));
  
    players.value
      .slice()
      .sort((a, b) => {
        console.log(`Comparing ${a.name} (points: ${a.points}) with ${b.name} (points: ${b.points})`);
        return b.points - a.points;
      })
      .forEach((player, index) => {
        player.Rank = index + 1;
      });
  };  
  
  const addPlayer = async (Player: Player) => {
    const res = await $fetch('http://localhost:5181/api/Players', {
      method: 'POST',
      body: Player,
    });
    players.value.push(res);
    loadPlayers();
  };

  const deletePlayer = async (playerId: number) => {
    try {
      await $fetch(`http://localhost:5181/api/Players/${playerId}`, {
        method: 'DELETE'
      });
      await loadPlayers(); 
    } catch (error) {
      console.error('Error deleting player:', error);
    }
    loadPlayers();
  };

  // const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number, teamId: number | null) => { 
  //   try {
  //     const body: any = {
  //       name: newName,
  //       points: newScore
  //     };
  
  //     if (teamId !== null) {
  //       body.teamId = teamId;
  //     }
  
  //     const response = await $fetch(`http://localhost:5181/api/Players/${selectedPlayerId}`, {
  //       method: 'PUT',
  //       body: body
  //     });
  
  //     await loadPlayers(); 
  //   } catch (error) {
  //     console.error('Error updating player:', error);
  //   }
  // }; 
  const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number, teamId: number | null) => { 
    try {
      const body: any = {
        name: newName,
        points: newScore,
        teamId: teamId
      };
  
      const response = await $fetch(`http://localhost:5181/api/Players/${selectedPlayerId}`, {
        method: 'PUT',
        body: body
      });
  
      await loadPlayers(); 
    } catch (error) {
      console.error('Error updating player:', error);
    }
  }; 
  // const updatePlayer = async (selectedPlayerId: number, newName: string, newScore: number) => { 
  //   try {
  //     await $fetch(`http://localhost:5181/api/Players/${selectedPlayerId}`, {
  //       method: 'PUT',
  //       body: {
  //         name: newName,
  //         points: newScore
  //       }
  //     });
  //     await loadPlayers(); 
  //   } catch (error) {
  //     console.error('Error updating player:', error);
  //   }
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
      const team = await useTeamStore().teams.find(t => t.name === winningTeam);
      
      if (team) {
        const teamPlayers = players.value.filter(player => player.team === team.id);
        
        for (const player of teamPlayers) {
          await updatePlayer(
            player.id,
            player.name,
            Number(player.points) + 1
          );
        }
        await loadPlayers();
      } else {
        console.error(`Team ${winningTeam} not found`);
      }
    } catch (error) {
      console.error('Error updating winning team points:', error);
    }
  };

  return { players, generateId, addPlayer, deletePlayer, addPointsToWinningTeam, loadPlayers, updatePlayer, generateRanks};
});