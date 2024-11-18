<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Name" name="name">
        <UInput v-model="state.name" type="name"/>
      </UFormGroup>

      <UFormGroup label="Points" name="points">
        <UInput v-model="state.points" type="number" placeholder="Enter points" />
      </UFormGroup>
  
      <UButton type="submit"> Lisa </UButton>
    </UForm>
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import type { Player } from "~/types/player";
  
    const playerStore = usePlayerStore();

    const state = reactive<Player>({
        id: playerStore.generateId(),
        name: '',
        points: 0,
        rank: 0
    });

    const validate = (state: any): FormError[] => {
        const errors = [];
        if (!state.name) 
        errors.push({ path: "name", message: "Required" });
        if (!state.points === undefined || state.points === null)
        errors.push({ path: "points", message: "Required" });
        return errors;
    };
    
    async function onSubmit(event: FormSubmitEvent<any>) {
      console.log("State before adding player:", state);
      const transformedData = { 
        id: state.id,
        name: state.name,
        points: state.points,
        rank: state.rank,
        team: state.team
      };
      try {
          await playerStore.addPlayer(transformedData);
          console.log("Player successfully added");
          await navigateTo("/players");
      } 
      catch (error) {
          console.error("Error in addPlayer:", error);
      }
    }

    async function onError(event: FormErrorEvent) {
        const element = document.getElementById(event.errors[0].id);
        element?.focus();
        element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>