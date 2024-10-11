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
  import { useTeamStore } from "~/stores/stores" // Adjust the path based on your store's location
  
  const teamStore = useTeamStore();
  
  // Dynamically create the items for the accordion using the team names
  const accordionItems = teamStore.teams.map((team, index) => ({
    label: `Team: ${team.teamname}`,
    content: `Members: ${team.members.join(', ')}`
  }));
  </script>
  
  