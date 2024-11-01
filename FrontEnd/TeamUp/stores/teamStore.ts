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

    const teams = ref<Team[]>([])

    const loadTeams = async () => {
      teams.value = await $fetch<Team[]>('http://localhost:5181/api/Teams')
    }

    const generateTeams = (selectedPlayers: Player[], teamCount: number, teamNames: string[]): Team[] => {
      const sortedPlayers = selectedPlayers.slice().sort((a, b) => b.Points - a.Points);

      const balancedTeams: { team: Team; points: number }[] = Array.from({ length: teamCount }, (_, i) => ({
        team: { id: generateId(), teamname: teamNames[i], members: [] },
        points: 0, // Temporary tracking of team points for balancing
      }));
  
      sortedPlayers.forEach((player) => {
        const teamWithLeastPlayersAndPoints = balancedTeams.reduce((prev, curr) => 
          prev.team.Members.length < curr.team.Members.length ||
          (prev.team.Members.length === curr.team.Members.length && prev.points < curr.points)
            ? prev
            : curr
        );
        teamWithLeastPlayersAndPoints.team.Members.push(player);
        teamWithLeastPlayersAndPoints.points += player.Points;
      });

      return balancedTeams.map(({ team }) => team);
    };

    return { teams, generateId, addTeam, deleteTeam, generateTeams, loadTeams };
  })