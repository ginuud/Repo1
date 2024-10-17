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
  await navigateTo("/teams");
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
