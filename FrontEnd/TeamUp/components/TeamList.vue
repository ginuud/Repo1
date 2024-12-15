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
      class="team-accordion-item ml-40 mr-40 mb-20"
    >
      <div class="accordion-header">
        <strong>Team: {{ team.name }}</strong>
        <div style="display: inline-block">
          <button @click="openEditModal(team.id)" class="edit-button">
            Edit
          </button>
          <button @click="deleteTeam(team.id)" class="delete-button">
            Delete
          </button>
        </div>
      </div>
      <div class="accordion-content">
        Members:
        {{
          team.members?.map((member) => member.name).join(", ") || "No members"
        }}
      </div>
    </div>
  </div>

  <UModal v-model="isEditModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-900',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <h3 class="text-base font-semibold leading-6 text-white">
            Edit player
          </h3>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            class="-my-1"
            @click="isEditModalOpen = false"
          />
        </div>
      </template>
      <UForm
        :validate="validateTeam"
        :state="editTeamForm"
        @submit="submitTeam"
        @error="handleError"
      >
        <UFormGroup label="Team Name" name="name">
          <UInput
            v-model="editTeamForm.name"
            color="purple"
            variant="outline"
          />
        </UFormGroup>

        <UFormGroup label="Members" name="members">
          <USelectMenu
            v-model="editTeamForm.members"
            :options="playerEditOptions"
            multiple
            searchable
            searchable-placeholder="Search a player..."
            placeholder="Select players"
            color="purple"
            variant="outline"
          />
        </UFormGroup>
        <div style="margin-bottom: 0.5rem">
          <p class="text-gray-500">
            Only players who are not already in a team are shown
          </p>
        </div>

        <UButton class="flex justify-end space-x-2" type="submit"
          >Add Team</UButton
        >
      </UForm>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { useTeamStore } from "~/stores/teamStore";
import { storeToRefs } from "pinia";

const playerStore = usePlayerStore();
const teamStore = useTeamStore();
const { teams } = storeToRefs(teamStore);
const searchQuery = ref("");
const isLoading = ref(true);
const selectedTeamid = ref<number | null>(null);
const playerEditOptions = ref([]);
const preSelectedMembers = ref([]);

const isEditModalOpen = ref(false);

const editTeamForm = reactive({
  name: "",
  members: [],
});

watch(
  preSelectedMembers,
  (newOptions) => {
    editTeamForm.members = newOptions.map(
      (item) =>
        playerEditOptions.value.find(
          (option) => option.value.id === item.value.id
        ) || item
    );
  },
  { immediate: true }
);

const openEditModal = async (teamId: number) => {
  const team = teamStore.teams.find((t) => t.id === teamId);

  const availablePlayers = [
    ...team.members.map((player) => ({
      value: player,
      label: player.name,
    })),
    ...playerStore.players
      .filter((player) => player.teamId === null)
      .map((player) => ({
        value: player,
        label: player.name,
      })),
  ];

  playerEditOptions.value = availablePlayers;

  preSelectedMembers.value = team.members.map((player) => ({
    value: player,
    label: player.name,
  }));

  editTeamForm.name = team?.name;

  console.log("editTeamForm", editTeamForm);

  selectedTeamid.value = teamId;
  isEditModalOpen.value = true;
};

function resetEditTeamForm() {
  editTeamForm.name = "";
  editTeamForm.members = [];
}

const validateTeam = (state: typeof editTeamForm): FormError[] => {
  const errors: FormError[] = [];
  console.log("editTeamForm", editTeamForm);
  if (!state.name) {
    errors.push({ path: "name", message: "Team name is required" });
  }
  if (state.members.length < 2) {
    errors.push({ path: "members", message: "Choose at least 2 players" });
  }
  return errors;
};

async function submitTeam(event: FormSubmitEvent) {
  event.preventDefault();
  console.log("editTeamForm", editTeamForm);
  try {
    const teamData = {
      id: selectedTeamid.value,
      name: editTeamForm.name,
      members: editTeamForm.members.map(({ value }) => ({
        id: value.id,
        name: value.name,
        points: value.points,
        rank: value.rank,
        teamId: value.teamId,
      })),
    };
    console.log("teamdata", teamData);
    await teamStore.updateTeam(teamData);
    console.log("Team successfully added:", teamData);
    isEditModalOpen.value = false;
    selectedTeamid.value = null;
    resetEditTeamForm();
  } catch (error) {
    console.error("Error adding team:", error);
  }
}

function handleError(event: FormErrorEvent) {
  const firstErrorElement = document.getElementById(event.errors[0]?.id);
  firstErrorElement?.focus();
  firstErrorElement?.scrollIntoView({ behavior: "smooth", block: "center" });
}

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
