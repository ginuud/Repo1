<template>
  <div>
    <h2>Generate Teams</h2>
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
      <div v-for="(team, index) in formState.teamNames" :key="index">
        <UFormGroup :label="`Team ${index + 1} Name`" :name="`teamName${index + 1}`">
          <UInput v-model="formState.teamNames[index]" placeholder="Enter team name" />
        </UFormGroup>
      </div>
      <UButton type="submit">Generate Teams</UButton>
    </UForm>
    <!-- KUSTUTA SIIT -->
    <div v-if="generatedTeams.length > 0">
      <h2>Generated Teams</h2>
      <div v-for="(team, index) in generatedTeams" :key="index">
        <h3>{{ formState.teamNames[index] }}</h3> <!-- Display the team name from the form -->
        <ul>
          <li v-for="player in team.players" :key="player.id">
            {{ player.name }} - {{ player.points }} points
          </li>
        </ul>
        <p><strong>Total Points:</strong> {{ team.totalPoints }}</p>
      </div>
    </div>
    <!-- KUSTUTA SIIANI -->
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { usePlayerStore } from '~/stores/playerStore';
import { useTeamStore } from '~/stores/teamStore'; 
import type { Player } from '~/types/player'; 

const playerStore = usePlayerStore();
const teamStore = useTeamStore();

const formState = reactive({
  numberOfTeams: 2,
  teamNames: Array(2).fill('')
});

const generatedTeams = ref<{ players: Player[]; totalPoints: number }[]>([]);

async function onGenerateTeams() {
  const teams = generateBalancedTeams(playerStore.players, formState.numberOfTeams);
  generatedTeams.value = teams;

  teams.forEach((team, index) => {
    teamStore.addTeam({
      id: teamStore.generateId(),
      teamname: formState.teamNames[index], 
      members: team.players.map(player => player.name)
    });
  });
  // LISA TAGASI KUI ÜLEVALT ON DIVID KUSTUTATUD await navigateTo("/teams");
}

function generateBalancedTeams(players: Player[], numberOfTeams: number): { players: Player[], totalPoints: number }[] {
  const sortedPlayers = players.slice().sort((a, b) => b.points - a.points);

  const teams = Array.from({ length: numberOfTeams }, () => ({
    players: [] as Player[],
    totalPoints: 0
  }));

  sortedPlayers.forEach((player, index) => {
    const teamIndex = index % numberOfTeams;
    teams[teamIndex].players.push(player);
    teams[teamIndex].totalPoints += player.points;
  });

  return teams;
}
</script>
