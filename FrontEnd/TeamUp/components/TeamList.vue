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
  import { useTeamStore } from "~/stores/teamStore" 
  
  const teamStore = useTeamStore();
  const {teams} = storeToRefs(teamStore)

  onMounted(() => {
    teamStore.loadTeams();
  })
  
  // Tuleb ilusamaks teha
  const accordionItems = computed(() => {
    console.log("Team values:", teams.value)
  return teams.value.map((team) => ({
    label: `Team: ${team.name}`,
    content: `Members: ${team.members?.map(member => member.name).join(', ') || 'No members'}`
  }));
});
  </script>
  
  