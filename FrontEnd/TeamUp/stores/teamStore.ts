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

    const addTeam = (team: Team) => {
        teams.value.push(team)
    }

    const deleteTeam = async (teamId: number) => {
      const index = teams.value.findIndex((team) => team.id === teamId);
      if (index !== -1) {
        teams.value.splice(index, 1);
      }
    }

    const teams = ref<Team[]>([
      {id: generateId(), teamname: 'A', members:[]},
      {id: generateId(), teamname: 'B', members:[]},
    ]);

    const generateTeams = (selectedPlayers: Player[], teamCount: number, teamNames: string[]): Team[] => {

      if (selectedPlayers.length < teamCount) {
          console.error('Not enough players to generate the requested number of teams.');
          return []; 
      }

      const sortedPlayers = selectedPlayers.slice().sort((a, b) => b.points - a.points);

      const balancedTeams: Team[] = Array.from({ length: teamCount }, (_, i) => ({
          id: generateId(),
          teamname: teamNames[i], 
          members: [],
      }));

      sortedPlayers.forEach((player, index) => {
          balancedTeams[index % teamCount].members.push(player); 
      });

      return balancedTeams; 
    };

    return { teams, generateId, addTeam, deleteTeam, generateTeams };
  })