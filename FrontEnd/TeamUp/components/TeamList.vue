<template>
  <div class="mb-4 table-container flex items-center justify-between"> 
    <input
      v-model="searchQuery"
      type="text"
      placeholder="Search teams or members..."
      class="search-input"
    />
    <AddTeam />
  </div>
  
  <div v-if="teams.length === 0" class="text-center text-red-500">
    No teams have been added
  </div>

  <div v-if="filteredTeams.length === 0" class="text-center text-red-500">
    No games match your search.
  </div>

  <div v-else>
    <div
      v-for="team in filteredTeams"
      :key="team.id"
      class="team-accordion-item ml-20 mr-20 mb-20"
    >
      <div class="accordion-header">
        <strong>Team: {{ team.name }}</strong>
        <button @click="deleteTeam(team.id)" class="delete-button">
          Delete
        </button>
      </div>
      <div class="accordion-content">
        Members:
        {{
          team.members?.map((member) => member.name).join(", ") ||
          "No members"
        }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useTeamStore } from "~/stores/teamStore";
import { storeToRefs } from "pinia";

const teamStore = useTeamStore();
const { teams } = storeToRefs(teamStore);
const searchQuery = ref("");

const filteredTeams = computed(() => {
  if (!searchQuery.value.trim()) {
    return teams.value;
  }
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return teams.value.filter((team) => {
    const teamNameMatch = team.name.toLowerCase().includes(lowerCaseQuery);
    const memberMatch = (team.members || []).some((member) =>
      member.name.toLowerCase().includes(lowerCaseQuery)
    );
    return teamNameMatch || memberMatch;
  });
});


onMounted(async () => {
  await teamStore.loadTeams();
});

const deleteTeam = async (teamId: number) => {
  await teamStore.deleteTeam(teamId);
};
</script>

<style scoped>
@import "@/assets/css/tableStyle.css";
@import "@/assets/css/accordionStyle.css";
</style>
