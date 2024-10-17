<template>
    <div>
      <h2>Generate Balanced Teams</h2>
      <UForm
        :state="formState"
        @submit="onGenerateTeams"
        class="space-y-4"
      >
        <UFormGroup label="Number of Teams" name="numberOfTeams">
          <USelect
            v-model="formState.numberOfTeams"
            :options="[{ value: 2, label: '2' }, { value: 3, label: '3' }, { value: 4, label: '4' }]"
          />
        </UFormGroup>
  
        <UButton type="submit">Generate Teams</UButton>
      </UForm>
  
      <div v-if="generatedTeams.length > 0">
        <h2>Generated Teams</h2>
        <div v-for="(team, index) in generatedTeams" :key="index">
          <h3>Team {{ index + 1 }}</h3>
          <ul>
            <li v-for="player in team.players" :key="player.id">
              {{ player.name }} - {{ player.points }} points
            </li>
          </ul>
          <p><strong>Total Points:</strong> {{ team.totalPoints }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, reactive } from 'vue';
  import { usePlayerStore } from '~/stores/playerStore'; // Player store
  import { useTeamStore } from '~/stores/teamStore'; // Team store
  import type { Player } from '~/types/player'; // Player type
  import type { Team } from '~/types/team'; // Team type
  
  const playerStore = usePlayerStore();
  const teamStore = useTeamStore();
  
  // Create a reactive state for the form
  const formState = reactive({
    numberOfTeams: 2,
  });
  
  const generatedTeams = ref<{ players: Player[]; totalPoints: number }[]>([]);
  
  // Function to handle team generation and store it in the team store
  function onGenerateTeams() {
    const teams = generateBalancedTeams(playerStore.players, formState.numberOfTeams);
    generatedTeams.value = teams;
  
    // Add generated teams to the team store
    teams.forEach((team, index) => {
      teamStore.addTeam({
        id: teamStore.generateId(),
        teamname: `Team ${index + 1}`,
        members: team.players.map(player => player.name)
      });
    });
  }
  
  // Function to divide players into balanced teams based on their points
  function generateBalancedTeams(players: Player[], numberOfTeams: number): { players: Player[], totalPoints: number }[] {
    const sortedPlayers = players.slice().sort((a, b) => b.points - a.points);
  
    const teams = Array.from({ length: numberOfTeams }, () => ({
      players: [] as Player[],
      totalPoints: 0
    }));
  
    sortedPlayers.forEach((player) => {
      const teamWithLeastPoints = teams.reduce((prev, curr) =>
        prev.totalPoints < curr.totalPoints ? prev : curr
      );
      teamWithLeastPoints.players.push(player);
      teamWithLeastPoints.totalPoints += player.points;
    });
  
    return teams;
  }
  </script>
  
  
  