import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Team } from "~/types/team";
import type { Player } from '~/types/player';
import { useAuth } from '~/composables/useAuth';

export const useTeamStore = defineStore("team", () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }
    const teams = ref<Team[]>([])
    const auth = useAuth();

    const addTeam = async (team: Team) => {
      const res = await auth.fetchWithToken("Teams", {
        method: 'POST',
        body: team,
      });
      teams.value.push(res)
    }

    const deleteTeam = async (teamId: number) => {
      try {
        await auth.fetchWithToken(`Teams/${teamId}`, {
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
      teams.value = await auth.fetchWithToken<Team[]>("Teams")
    }

    const generateTeams = async (selectedPlayers: Player[], numberOfTeams: number, teamNames: string[]): Promise<Team[]> => {
      const requestData = {
        members: selectedPlayers,  
        teamsCount: numberOfTeams,
        teamNames: teamNames
      };
      const generatedTeams = await auth.fetchWithToken<Team[]>("Teams/generate", {
        method: 'POST',
        body: requestData,
      });
      return generatedTeams;
    };

    const updateTeam = async (team: Team) => { 
      try {
        console.log("team", team)
        await auth.fetchWithToken(`Teams/${team.id}`, {
          method: 'PUT',
          body: team
          // body: {
          //   name: name,
          //   members: members
          // },
        });
        await loadTeams(); 
      } 
      catch (error) {
        console.error('Error updating player:', error);
      }
    }; 

    const playerstore = usePlayerStore()

    const addPointsToTeam = async (team: Team) => {
      if (!team) {
        console.error('Invalid team:', team);
        return;
      }
    
      try {
        const teamPlayers = playerstore.players.filter(player => player.teamId === team.id);
    
        await Promise.all(
          teamPlayers.map(player =>
            playerstore.updatePlayer(
              player.id,
              player.name,
              player.points + 1,
              player.teamId
            )
          )
        );
      } catch (error) {
        console.error('Error updating team points:', error);
      }
    };
    

    return { teams, generateId, addTeam, deleteTeam, generateTeams, loadTeams, addPointsToTeam, updateTeam };
  })