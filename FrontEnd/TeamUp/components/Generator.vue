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
    import { computed, reactive, onMounted } from 'vue';
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useTeamStore } from "~/stores/teamStore";
    import { usePlayerStore } from "~/stores/playerStore";
    import type { Player } from "~/types/player";

    const playerStore = usePlayerStore();
    const teamStore = useTeamStore();

    onMounted(() => {
      playerStore.loadPlayers();
    });

    const playerOptions = computed(() =>
      playerStore.players.map((player) => ({
        value: player,
        label: player.name,
      }))
    );

    const state = reactive({
      Name: "",
      selectedPlayers: [],  
      numberOfTeams: 2,  
    });

    const teamNames = reactive(Array.from({ length: state.numberOfTeams }, () => ''));

    const validate = (state: any): FormError[] => {
      const errors = [];
      if (state.selectedPlayers.length < 4) {
        errors.push({ path: "selectedPlayers", message: "Choose at least 4 players" });
      }
      if (state.numberOfTeams < 2) {
        errors.push({ path: "numberOfTeams", message: "At least 2 teams required" });
      }
      return errors;
    };

    async function onSubmit(event: FormSubmitEvent<any>) {
      event.preventDefault();

      console.log("Selected Players:", state.selectedPlayers);

      const transformedData = state.selectedPlayers
        .map(player => ({
          id: player.value.id,
          name: player.value.name,
          points: player.value.points,
          rank: player.value.rank,
          teamId: player.value.teamId,
        }));

      console.log("Transformed Player Data:", transformedData);
      
      try {
          await teamStore.generateTeams(transformedData, state.numberOfTeams, teamNames);
          console.log("Teams successfully added");
          await navigateTo("/teams");
      } 
      catch (error) {
          console.error("Error in generateTeams:", error);
      }
    }

    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

</script>

    