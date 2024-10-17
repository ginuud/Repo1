<template>
  <div>
    <h2>Generate Teams</h2>
    <UForm
      :state="formState"
      @submit="onGenerateTeams"
      class="space-y-4"
    >
      <div v-for="(team, index) in formState.teamNames" :key="index">
        <UFormGroup :label="`Team ${index + 1} Name`" :name="`teamName${index + 1}`">
          <UInput v-model="formState.teamNames[index]" placeholder="Enter team name" />
        </UFormGroup>
      </div>
      <UButton type="submit">Generate Teams</UButton>
    </UForm>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, watch } from 'vue';
import { usePlayerStore } from '~/stores/playerStore'; // Player store
import { useTeamStore } from '~/stores/teamStore'; // Team store
import type { Player } from '~/types/player'; // Player type

const playerStore = usePlayerStore();
const teamStore = useTeamStore();

// Create a reactive state for the form
const formState = reactive({
  numberOfTeams: 2,
  teamNames: Array(2).fill('') // Initialize with empty names for 2 teams
});

// Watch for changes to numberOfTeams to dynamically update teamNames array
watch(() => formState.numberOfTeams, (newCount) => {
  formState.teamNames = Array(newCount).fill(''); // Reset team names when number of teams changes
});

const generatedTeams = ref<{ players: Player[]; totalPoints: number }[]>([]);

// Function to handle team generation and store it in the team store
async function onGenerateTeams() {
  const teams = generateBalancedTeams(playerStore.players, formState.numberOfTeams);
  generatedTeams.value = teams;

  // Add generated teams to the team store with custom names
  teams.forEach((team, index) => {
    teamStore.addTeam({
      id: teamStore.generateId(),
      teamname: formState.teamNames[index] || `Team ${index + 1}`, // Use entered name or default if empty
      members: team.players.map(player => player.name)
    });
  });
  await navigateTo("/teams");
}

// Function to divide players into balanced teams based on their points
function generateBalancedTeams(players: Player[], numberOfTeams: number): { players: Player[], totalPoints: number }[] {
  // Sort players by points in descending order
  const sortedPlayers = players.slice().sort((a, b) => b.points - a.points);

  // Initialize teams with an equal number of players
  const teams = Array.from({ length: numberOfTeams }, () => ({
    players: [] as Player[],
    totalPoints: 0
  }));

  // Distribute players evenly among teams, trying to balance total points
  sortedPlayers.forEach((player, index) => {
    const teamIndex = index % numberOfTeams;
    teams[teamIndex].players.push(player);
    teams[teamIndex].totalPoints += player.points;
  });

  return teams;
}
</script>
