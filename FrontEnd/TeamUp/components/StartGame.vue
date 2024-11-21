<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
    <UFormGroup label="Game name" name="name">
        <UInput v-model="state.name" type="name"/>
      </UFormGroup>
      <UFormGroup label="Teams" name="teams">
        <USelectMenu
          v-model="state.teams"
          :options="teamOptions"
          multiple
          searchable
          searchable-placeholder="Search team..."
          placeholder="Select 2 teams" />
      </UFormGroup>
  
      <UButton type="submit"> Start game </UButton>
    </UForm>
  </template>

  
<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Game } from "~/types/game";
import { computed } from 'vue';

const gameStore = useGameStore();
const teamStore = useTeamStore();

onMounted(() => {
    teamStore.loadTeams();
  })

const teamOptions = computed(() => 
  teamStore.teams
  .filter((team) => team.gameId === null)
  .map(team => ({
    value: team,
    label: team.name
  }))
);

const state = reactive<Game>({
        id: gameStore.generateId(),
        name: "",
        teams: [],
        status: 'in progress',
    });

const validate = (state: Game): FormError[] => {
    const errors = [];
    if (!state.name) 
    errors.push({ path: "name", message: "Required" });
    if (state.teams.length != 2 )
    errors.push({ path: "teams", message: "Choose 2 teams" });
    return errors;
};

async function onSubmit(event: Event) {
  event.preventDefault();

  const transformedData = {
    id: state.id,
    name: state.name,
    teams: state.teams.map(team => ({
        id: team.value.id,
        name: team.value.name,
        members: team.value.members.map(member => ({
            id: member.id,
            name: member.name,
            points: member.points,
            rank: member.rank,
            teamId: member.teamId
        }))
    })),
    status: state.status
};

  console.log("Data before starting game:", transformedData)
  try {
    await gameStore.addGame(transformedData)
    console.log("Game successfully started")
    await navigateTo("/games");
  }
  catch (error){
    console.error("Error in startGame:", error)
  }
}

async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
}

</script>