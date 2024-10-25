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
          @change="updateTeamNames" 
        />
      </UFormGroup>
      <div v-for="(team, index) in formState.teamNames" :key="index">
        <UFormGroup :label="`Team ${index + 1} Name`" :name="`teamName${index + 1}`">
          <UInput v-model="formState.teamNames[index]" placeholder="Enter team name" />
        </UFormGroup>
      </div>
      <UButton type="submit">Generate Teams</UButton>
    </UForm>

    <div v-if="generatedTeams.length > 0">
      <h2>Generated Teams</h2>
      <div v-for="(team, index) in generatedTeams" :key="index">
        <h3>{{ formState.teamNames[index] }}</h3>
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
import { ref, reactive, watch } from 'vue';
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

function updateTeamNames() {
  const teamCount = formState.numberOfTeams;
  formState.teamNames = Array.from({ length: teamCount }, (_, i) => formState.teamNames[i]);
}

watch(() => formState.numberOfTeams, () => {
  updateTeamNames();
});

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

updateTeamNames();
</script>
