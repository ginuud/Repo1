 
<template>
  <div>
    <UButton 
      :ui="{ rounded: 'rounded-full' }"
      icon="i-heroicons-plus"
      size="md"
      color="primary"
      variant="solid"
      label="Add team"
      @click="navigateToAddTeam"
      class="add-team-button"
    >
    </UButton>

    <div class="mt-4">
      <div v-if="teams.length === 0" class="text-center text-red-500">
        No teams have been added
      </div>
      <div v-else>
        <div v-for="team in teams" :key="team.id" class="team-accordion-item">
          <div class="accordion-header">
            <strong>Team: {{ team.name }}</strong>
            <button @click="deleteTeam(team.id)" class="delete-button">Delete</button>
          </div>
          <div class="accordion-content">
            Members: {{ team.members?.map(member => member.name).join(', ') || 'No members' }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { useTeamStore } from "~/stores/teamStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
//import { AddTeam } from "#build/components";
//import AddTeam from "~/pages/add-team.vue";

const teamStore = useTeamStore();
const { teams } = storeToRefs(teamStore);
const router = useRouter();

const navigateToAddTeam = () => {
  router.push('/add-team'); // Replace with your actual "Add Team" route
};

onMounted(() => {
  teamStore.loadTeams();
});

const deleteTeam = async (teamId: number) => {
  await teamStore.deleteTeam(teamId);
};
</script>

<style scoped>
.team-accordion-item {
  border: 1px solid #ccc;
  margin-bottom: 1em;
  padding: 1em;
  border-radius: 8px;
}

.accordion-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.delete-button {
  background-color: red;
  color: white;
  border: none;
  padding: 0.5em 1em;
  cursor: pointer;
}
</style>