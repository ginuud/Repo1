import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Team } from "~/types/team";
import type { Player } from '~/types/player';

export const useTeamStore = defineStore('team', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }
    const teams = ref<Team[]>([])

    const addTeam = async (team: Team) => {
      const res = await $fetch('http://localhost:5181/api/Teams', {
        method: 'POST',
        body: team,
      });
      teams.value.push(res)
    }

    const deleteTeam = async (teamId: number) => {
      try {
        await $fetch(`http://localhost:5181/api/Teams/${teamId}`, {
          method: 'DELETE',
        }); 
        const index = teams.value.findIndex((team) => team.id === teamId);
        if (index !== -1) {
          teams.value.splice(index, 1);
        }
        await loadTeams();
        console.log(`Team with ID ${teamId} deleted and list refreshed.`);
      } catch (error) {
        console.error('Error deleting team:', error);
      }
    }    

    const loadTeams = async () => {
      teams.value = await $fetch<Team[]>('http://localhost:5181/api/Teams')
    }

    const generateTeams = async (selectedPlayers: Player[], numberOfTeams: number, teamNames: string[]): Promise<Team[]> => {
      const requestData = {
        members: selectedPlayers,  
        teamsCount: numberOfTeams,
        teamNames: teamNames
      };
      const generatedTeams = await $fetch<Team[]>('http://localhost:5181/api/Teams/generate', {
        method: 'POST',
        body: requestData,
      });
      console.log("Generated Teams Response:", generatedTeams);
      return generatedTeams;
    };

    const playerstore = usePlayerStore()

    const addPointsToTeam = async (team: Team) => {
      try {
        if (team) {
          const teamPlayers = playerstore.players.filter(player => player.teamId === team.id);
          console.log("teamPlayers", teamPlayers)
          
          for (const player of teamPlayers) {
            await playerstore.updatePlayer(
              player.id,
              player.name,
              Number(player.points) + 1,
              player.teamId
            );
          }
        } else {
          console.error(`Team ${team} not found`);
        }
      } catch (error) {
        console.error('Error updating winning team points:', error);
      }
    };

    return { teams, generateId, addTeam, deleteTeam, generateTeams, loadTeams, addPointsToTeam };
  })