<template>
    <UAccordion
    multiple
      color="green"
      variant="ghost"
      size="lg"
      :items="accordionItems"
      
    />
  </template>
  <script setup lang="ts">
  import { useTeamStore } from "~/stores/playerStore" 
  
  const teamStore = useTeamStore();
  
  // Tuleb ilusamaks teha
  const accordionItems = teamStore.teams.map((team, index) => ({
    label: `Team: ${team.teamname}`,
    content: `Members: ${team.members.join(', ')}`
  }));
  </script>
  
  