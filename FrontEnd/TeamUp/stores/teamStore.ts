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

    return { teams, generateId, addTeam, deleteTeam };
  })