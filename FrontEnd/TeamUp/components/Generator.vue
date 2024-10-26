<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Select Players" name="selectedPlayers">
        <USelectMenu v-model="state.selectedPlayers" 
                     :options="playerOptions"
                     multiple placeholder="Select players" />
      </UFormGroup>

      <UFormGroup label="Number of Teams" name="numberOfTeams">
        <UInput v-model.number="state.numberOfTeams" type="number" />
      </UFormGroup>

      <UFormGroup label="Team Names" name="teamNames">
        <div v-for="index in state.numberOfTeams" :key="index">
            <UInput v-model="teamNames[index - 1]" type="text" placeholder="Enter team name" />
        </div>
      </UFormGroup>
  
      <UButton type="submit"> Generate Teams </UButton>

    </UForm>
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useTeamStore } from "~/stores/teamStore";
    import { usePlayerStore } from "~/stores/playerStore";
    import type { Player } from "~/types/player";

    const playerStore = usePlayerStore();
    const { addTeam, generateId, generateTeams, } = useTeamStore();

    interface SelectedPlayer {
      value: number;
      label: string;
    }

    const state = reactive({
        teamname: '',
        selectedPlayers: [] as SelectedPlayer[],
        numberOfTeams: 2,
    });

    const teamNames = reactive(Array.from({ length: state.numberOfTeams }, () => ''));

    const playerOptions = computed(() =>
        playerStore.players.map(player => ({
            value: player.id,
            label: player.name,
        }))
    );

    const validate = (state: any): FormError[] => {
        const errors = [];
        if (state.selectedPlayers.length < 4)
        errors.push({ path: "selectedPlayers", message: "Choose at least 4 players" });
        if (state.numberOfTeams < 2)
        errors.push({ path: "numberOfTeams", message: "At least 2 teams required" });
        return errors;
    };

    async function onSubmit(event: FormSubmitEvent<any>) {
      console.log('Selected Players IDs:', state.selectedPlayers); //
        const selectedPlayers = state.selectedPlayers
          .map(selected => selected.value) 
          .map(id => playerStore.players.find(player => player.id === id))
          .filter((player): player is Player => player !== undefined);
            console.log('Selected Players:', selectedPlayers);
    
    // Check number of teams and prepare team names
    console.log('Number of Teams:', state.numberOfTeams); //
    console.log('Team Names:', teamNames); //
        const teams = generateTeams(selectedPlayers, state.numberOfTeams, teamNames);
    console.log('Generated Teams:', teams); //
        teams.forEach((team, index) => {
            addTeam({
                id: generateId(),
                teamname: teamNames[index],
                members: team.members,
            });
        });

        await navigateTo("/teams");
    }
    
    async function onError(event: FormErrorEvent) {
        const element = document.getElementById(event.errors[0].id);
        element?.focus();
        element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>