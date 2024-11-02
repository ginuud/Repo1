<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
    <UFormGroup label="Game name" name="Name">
        <UInput v-model="state.name" type="Name"/>
      </UFormGroup>
      <UFormGroup label="Team 1 name" name="Teams[0]">
        <USelect v-model="state.Teams[0]" :options="teamOptions"/>
      </UFormGroup>
      <UFormGroup label="Team 2 name" name="Teams[1]">
        <USelect v-model="state.Teams[1]" :options="teamOptions"/>
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

const state = reactive<Game>({
        Id: gameStore.generateId(),
        Name: undefined,
        Teams: [],
        Status: 'in progress',
    });

const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.Name) 
    errors.push({ path: "name", message: "Required" });
    if (!state.Teams[0])
    errors.push({ path: "Teams[0]", message: "Required" });
    if (!state.Teams[1])
    errors.push({ path: "Teams[1]", message: "Required" });
    return errors;
};

async function onSubmit(event: Event) {
  event.preventDefault();

  console.log("State before starting game:", state)

  try {
    await gameStore.addGame({...state})
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

const teamOptions = computed(() => 
teamStore.teams.map(team => ({
    value: team.name,
    label: team.name 
  }))
);

</script>