<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Team name" name="Name">
        <UInput v-model="state.Name" type="Name"/>
      </UFormGroup>
  
      <UFormGroup label="Members" name="Members">
        <USelectMenu v-model="state.Members" :options="playerOptions" multiple placeholder="Select players" />
      </UFormGroup>
  
      <UButton type="submit"> Lisa </UButton>
    </UForm>
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import type { Team } from "~/types/team";
    import { useTeamStore } from "~/stores/teamStore";
    import { usePlayerStore } from "~/stores/playerStore";

    const playerStore = usePlayerStore();
    const teamStore = useTeamStore();

    onMounted(() => {
      playerStore.loadPlayers();
  })

    const playerOptions = computed(() =>
    playerStore.players.map((player) => ({
        value: player,
        label: player.name,
    })));
  

    const state = reactive<Team>({
        Id: teamStore.generateId(),
        Name: '',
        Members: [],
    });

    const validate = (state: any): FormError[] => {
        const errors = [];
        const allPlayersUnassigned = state.Members.every(member => member.value.teamId === null);
        if (!state.Name) 
        errors.push({ path: "Name", message: "Required" });
        if (state.Members.length < 2)
        errors.push({ path: "Members", message: "Choose at least 2 players" });
        if (!allPlayersUnassigned)
        errors.push({path: "Members", message: "Player is already in a team"})
        return errors;
    };
    
    async function onSubmit(event: FormSubmitEvent<any>) {

      console.log("State before adding team:", state);
      const transformedData = { //state sisaldab ka value ja label key'sid, seega ei saa otse state'i kasutada
        Id: state.Id,
        Name: state.Name,
        Members: state.Members.map(member => ({
            id: member.value.id,
            name: member.value.name,
            points: member.value.points,
            rank: member.value.rank,
            teamId: member.value.teamId
        }))
      };

      try {
          await teamStore.addTeam(transformedData);
          console.log("Team successfully added");
          await navigateTo("/teams");
      } 
      catch (error) {
          console.error("Error in addTeam:", error);
      }
    }
    
    async function onError(event: FormErrorEvent) {
        const element = document.getElementById(event.errors[0].id);
        element?.focus();
        element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>