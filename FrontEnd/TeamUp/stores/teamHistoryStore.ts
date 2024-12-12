import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { TeamHistory } from "~/types/teamhistory";
import { useAuth } from '~/composables/useAuth';

export const useTeamHistoryStore = defineStore("teamHistory", () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }
    const teamsHistory = ref<TeamHistory[]>([])
    const auth = useAuth();

    const addTeamHistory = async (teamHistory: TeamHistory) => {
      const res = await auth.fetchWithToken("TeamsHistory", {
        method: 'POST',
        body: teamHistory,
      });
      teamsHistory.value.push(res);
    }

    const deleteTeamHistory = async (teamHistoryId: number) => {
      try {
        await auth.fetchWithToken(`TeamsHistory/${teamHistoryId}`, {
          method: 'DELETE',
        }); 
        const index = teamsHistory.value.findIndex((teamHistory) => teamHistory.id === teamHistoryId);
        if (index !== -1) {
          teamsHistory.value.splice(index, 1);
        }
        await loadTeamsHistory();
        console.log(`Team with ID ${teamHistoryId} deleted and list refreshed.`);
      } catch (error) {
        console.error('Error deleting team history:', error);
      }
    }    

    const loadTeamsHistory = async () => {
      teamsHistory.value = await auth.fetchWithToken<TeamHistory[]>("TeamsHistory")
    }

    
    return { teamsHistory, generateId, addTeamHistory, deleteTeamHistory, loadTeamsHistory };
  })