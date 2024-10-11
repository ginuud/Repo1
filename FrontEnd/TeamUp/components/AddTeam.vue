<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Team name" name="teamname">
        <UInput v-model="state.teamname" type="teamname"/>
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
    import { usePlayerStore } from "~/stores/stores";
    const playerStore = usePlayerStore();

    const playerOptions = computed(() =>
    playerStore.players.map((player) => ({
        value: player.id,
        label: player.name,
    })));
  
    const { addTeam, generateId, } = useTeamStore();

    const state = reactive<Team>({
        id: generateId(),
        teamname: undefined,
        members: [],
    });

    const validate = (state: any): FormError[] => {
        const errors = [];
        if (!state.teamname) 
        errors.push({ path: "teamname", message: "Required" });
        if (!state.members)
        if (!state.members || state.members.length === 1)
        errors.push({ path: "members", message: "Choose at least 2 players" });
        return errors;
    };
    
    async function onSubmit(event: FormSubmitEvent<any>) {
        addTeam({...state})
        await navigateTo("/players");
    }
    
    async function onError(event: FormErrorEvent) {
        const element = document.getElementById(event.errors[0].id);
        element?.focus();
        element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>