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
  
  <div v-if="isLoading" class="flex justify-center items-center h-64">
    <div class="loader"></div>
  </div>

  <div v-else-if="teams.length === 0" class="text-center text-red-500">
    No teams have been added
  </div>

  <div v-else-if="filteredTeams.length === 0" class="text-center text-red-500">
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
const isLoading = ref(true);

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
  try {
    isLoading.value = true;
    await teamStore.loadTeams();
  } catch (error) {
    console.error("Error loading data:", error);
  } finally {
    isLoading.value = false;
  }
});

const deleteTeam = async (teamId: number) => {
  await teamStore.deleteTeam(teamId);
};
</script>

<style scoped>
@import "@/assets/css/tableStyle.css";
@import "@/assets/css/accordionStyle.css";

.loader {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite;
}
</style>
