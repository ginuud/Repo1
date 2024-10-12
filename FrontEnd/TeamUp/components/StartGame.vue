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
      <UFormGroup label="Team 1 name" name="team1name">
        <USelect v-model="state.team1name" :options="teamOptions"/>
      </UFormGroup>
      <UFormGroup label="Team 2 name" name="team2name">
        <USelect v-model="state.team2name" :options="teamOptions"/>
      </UFormGroup>
  
      <UButton type="submit"> Start game </UButton>
    </UForm>
  </template>

  
<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Game } from "~/types/game";
import { computed } from 'vue';

const { addGame, generateId, } = useGameStore();
const teamsStore = useTeamStore();

const state = reactive<Game>({
        id: generateId(),
        name: undefined,
        team1name: undefined,
        team2name: undefined,
        status: 'in progress',
    });

const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.name) 
    errors.push({ path: "name", message: "Required" });
    if (!state.team1name)
    errors.push({ path: "team1name", message: "Required" });
    if (!state.team2name)
    errors.push({ path: "team2name", message: "Required" });
    return errors;
};

async function onSubmit(event: FormSubmitEvent<any>) {
    addGame({...state})
    await navigateTo("/games");
}

async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
}

const teamOptions = computed(() => 
  teamsStore.teams.map(team => ({
    value: team.teamname,
    label: team.teamname 
  }))
);

</script>