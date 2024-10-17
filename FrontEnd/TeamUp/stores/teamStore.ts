import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Team } from "~/types/team";

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

    const generateTeams = (teamCount: number, teamNames: string[]) => {
      const playerStore = usePlayerStore(); // Access players from the player store
      const players = playerStore.players; 

      // Sort players by points in descending order
      const sortedPlayers = players.slice().sort((a, b) => b.points - a.points);

      // Initialize teams with equal distribution
      const balancedTeams: Team[] = Array.from({ length: teamCount }, (_, i) => ({
        id: generateId(),
        teamname: teamNames[i] || `Team ${i + 1}`, // Use form-provided team names or default names
        members: [],
      }));

      // Distribute players evenly among teams
      sortedPlayers.forEach((player, index) => {
        balancedTeams[index % teamCount].members.push(player.name); // Distribute players by index
      });

      // Clear the current teams and add the newly generated balanced teams
      teams.value = balancedTeams;
    };

    return { teams, generateId, addTeam, deleteTeam, generateTeams };
  })