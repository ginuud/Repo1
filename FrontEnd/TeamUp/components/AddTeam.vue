<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Team name" name="name">
        <UInput v-model="state.name" type="name"/>
      </UFormGroup>
  
      <UFormGroup label="Members" name="members">
        <USelectMenu v-model="state.members" :options="playerOptions" multiple placeholder="Select players" />
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
        id: teamStore.generateId(),
        name: '',
        members: [],
    });

    const validate = (state: any): FormError[] => {
        const errors = [];
        if (!state.name) 
        errors.push({ path: "Name", message: "Required" });
        if (state.members.length < 2)
        errors.push({ path: "Members", message: "Choose at least 2 players" });
        return errors;
    };
    
    async function onSubmit(event: FormSubmitEvent<any>) {

      console.log("State before adding team:", state);
      const transformedData = { //state sisaldab ka value ja label key'sid, seega ei saa otse state'i kasutada
        id: state.id,
        name: state.name,
        members: state.members.map(member => ({
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