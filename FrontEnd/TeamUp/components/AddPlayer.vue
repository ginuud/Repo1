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
        name: undefined,
        points: undefined,
    });

    const validate = (state: any): FormError[] => {
        const errors = [];
        if (!state.name) 
        errors.push({ path: "name", message: "Required" });
        if (!state.points)
        errors.push({ path: "points", message: "Required" });
        return errors;
    };
    
    async function onSubmit(event: Event) {
      event.preventDefault();

      console.log("State before adding player:", state);
      
      try {
          await playerStore.addPlayer({...state});
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