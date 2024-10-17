import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Team } from "~/types/team";
import type { Player } from "~/types/player";

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
      {id: generateId(), teamname: 'A', members:['Ivo Linna', 'Arvo Pärt','Kristjan Jõekalda']},
      {id: generateId(), teamname: 'B', members:['Koit Toome', 'Evelin Ilves']},
    ]);

    const generateTeams = (teamCount: number) => {
      const playerStore = usePlayerStore(); // Get players from the player store
      const players = playerStore.players; // Access players
  
      // Sort players by points in descending order
      const sortedPlayers = [...players].sort((a, b) => b.points - a.points);
  
      // Initialize generated teams
      const generatedTeams: Team[] = Array.from({ length: teamCount }, (_, i) => ({
        id: generateId(),
        teamname: `Team ${i + 1}`,
        members: [],
      }));
  
      // Distribute players between teams
      sortedPlayers.forEach((player, index) => {
        generatedTeams[index % teamCount].members.push(player.name);
      });
  
      // Clear current teams and add newly generated teams
      teams.value = generatedTeams;
    };

    return { teams, generateId, addTeam, deleteTeam, generateTeams };
  })