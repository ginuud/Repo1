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

    const addTeam = async (Team: Team) => {
      const res = await $fetch('http://localhost:5181/api/Teams', {
        method: 'POST',
        body: Team,
      });
      teams.value.push(res)
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

    const generateTeams = async (selectedPlayers: Player[], numberOfTeams: number, teamNames: string[]): Promise<Team[]> => {
      const requestData = {
        players: selectedPlayers,  
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

    return { teams, generateId, addTeam, deleteTeam, generateTeams, loadTeams };
  })