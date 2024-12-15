<template>
  <div class="mb-4 table-container flex items-center justify-between">
    <input
      v-model="searchQuery"
      type="text"
      placeholder="Search games or teams..."
      class="search-input"
    />
    <StartGame />
  </div>

  <div v-if="isLoading" class="flex justify-center items-center h-64">
    <div class="loader"></div>
  </div>

  <div v-else-if="games.length === 0" class="text-center text-red-500">
    No games have been played
  </div>

  <div v-else-if="filteredGames.length === 0" class="text-center text-red-500">
    No games match your search.
  </div>

  <div v-else>
    <div class="table-container">
      <Table class="Table">
        <TableHeader>
          <TableRow class="header-row">
            <TableCell class="header-cell">Game</TableCell>
            <TableCell class="header-cell">Team 1</TableCell>
            <TableCell class="header-cell">Team 2</TableCell>
            <TableCell class="header-cell">Actions</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow
            v-for="game in filteredGames"
            :key="game.id"
            class="border-b border-black"
          >
            <TableCell>{{ game.name }}</TableCell>
            <TableCell>{{ game.teams[0]?.name || "N/A" }}</TableCell>
            <TableCell>{{ game.teams[1]?.name || "N/A" }}</TableCell>
            <TableCell>
              <UButton
                type="button"
                color="red"
                variant="ghost"
                icon="i-heroicons-stop-circle-20-solid"
                @click="openEndGameModal(game.id)"
              >
              </UButton>
              <UButton
                type="button"
                color="black"
                variant="ghost"
                icon="i-heroicons-pencil-20-solid"
                @click="openEditModal(game.id)"
              ></UButton>
              <UButton
                type="button"
                color="red"
                variant="ghost"
                icon="i-heroicons-trash-20-solid"
                @click="openDeleteModal(game.id)"
              ></UButton>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>

  <UModal v-model="isEndGameModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-900',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <h3 class="text-base font-semibold leading-6 text-white">
            Select Winner
          </h3>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            class="-my-1"
            @click="isEndGameModalOpen = false"
          />
        </div>
      </template>

      <div class="p-4">
        <h3 class="text-base font-semibold leading-6 text-white">
          Select the winner of the game:
        </h3>
        <UFormGroup name="selectedTeam">
          <USelectMenu
            v-model="selectedTeam"
            :options="teamOptions"
            placeholder="Select winning team"
            color="purple"
            variant="outline"
          />
        </UFormGroup>
      </div>

      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="green" @click="submitWinner">Submit Winner</UButton>
          <UButton color="red" @click="cancelSelection">Cancel</UButton>
        </div>
      </template>
    </UCard>
  </UModal>

  <UModal v-model="isDeleteModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-900',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <h3 class="text-base font-semibold leading-6 text-white">
            Delete game
          </h3>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            class="-my-1"
            @click="isDeleteModalOpen = false"
          />
        </div>
      </template>

      <div class="p-4">
        <h3 class="text-base font-semibold leading-6 text-white">
          Are you sure you want to delete game: {{ selectedGameName }}
        </h3>
      </div>

      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="red" @click="submitDelete">Delete Game</UButton>
        </div>
      </template>
    </UCard>
  </UModal>

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
            Edit game
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

      <div class="p-4">
        <h3 class="text-base font-semibold leading-6 text-white">Name</h3>
        <UInput
          v-model="name"
          color="purple"
          variant="outline"
          placeholder="Game name"
        />
        <p v-if="errors.name" class="text-red-500 text-sm mt-1">
          {{ errors.name }}
        </p>
        <h3 class="text-base font-semibold leading-6 text-white">Teams</h3>

        <UFormGroup name="teams">
          <div style="margin-bottom: 0.5rem">
            <USelectMenu
              v-model="selectedTeams"
              color="purple"
              :options="teamEditOptions"
              multiple
              searchable
              searchable-placeholder="Search teams..."
              placeholder="Select 2 teams"
            />
            <p v-if="errors.teams" class="text-red-500 text-sm mt-1">
              {{ errors.teams }}
            </p>
          </div>
        </UFormGroup>
      </div>
      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="green" @click="submitGame">Save Changes</UButton>
        </div>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useGameStore, type Player } from "#imports";
import { usePlayerStore } from "~/stores/playerStore";
import StartGame from "./StartGame.vue";

const gameStore = useGameStore();
const playerStore = usePlayerStore();
const teamStore = useTeamStore();

const { games } = storeToRefs(gameStore);
const searchQuery = ref("");
const isLoading = ref(true);

const isDeleteModalOpen = ref(false);
const selectedGameName = ref<string | null>(null);

let teamEditOptions = [];
let teamOptions = ref<{ id: number; name: string; members: Player[] }[]>([]);

const isEditModalOpen = ref(false);
const name = ref("");
const selectedTeams = ref([]);
const errors = reactive({
  name: null as string | null,
  teams: null as string | null,
});

const openDeleteModal = (gameId: number) => {
  const game = gameStore.games.find((p) => p.id === gameId);
  if (game) {
    isDeleteModalOpen.value = true;
    selectedGameId.value = gameId;
    selectedGameName.value = game.name;
  }
};

const submitDelete = async () => {
  if (selectedGameId.value !== null) {
    await gameStore.deleteGame(selectedGameId.value);
    isDeleteModalOpen.value = false;
    selectedGameId.value = null;
    navigateTo("/games");
  }
};

const openEditModal = async (gameId: number) => {
  await teamStore.loadTeams();
  const game = gameStore.games.find((p) => p.id === gameId);

  teamEditOptions = [
    ...game.teams.map((team) => ({
      value: team,
      label: team.name,
    })),
    ...teamStore.teams
      .filter((team) => team.gameId === null)
      .map((team) => ({ value: team, label: team.name })),
  ];

  console.log("teamEditOptions", teamEditOptions);
  if (game) {
    selectedGameId.value = gameId;
    name.value = game.name;
    selectedTeams.value = [];
    isEditModalOpen.value = true;
  }
};

const submitGame = async () => {
  if (validate()) {
    const teamIds = selectedTeams.value.map((team) => team.value.id);
    console.log(teamIds);
    await gameStore.updateGame(selectedGameId.value, name.value, teamIds);
    isEditModalOpen.value = false;
    selectedGameId.value = null;
  }
};

const validate = (): boolean => {
  errors.name = name.value.trim() ? null : "Game name is required.";
  errors.teams =
    selectedTeams.value.length === 2 ? null : "Please select exactly 2 teams.";
  return !errors.name && !errors.teams;
};

const filteredGames = computed(() => {
  if (!searchQuery.value.trim()) {
    return games.value;
  }
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return games.value.filter(
    (game) =>
      game.name.toLowerCase().includes(lowerCaseQuery) ||
      game.teams.some((team) =>
        team?.name?.toLowerCase().includes(lowerCaseQuery)
      )
  );
});

const isEndGameModalOpen = ref(false);
const selectedGameId = ref<number | null>(null);
const selectedTeam = ref<Team>();

const openEndGameModal = (gameId: number) => {
  const game = gameStore.games.find((g) => g.id === gameId);

  if (game) {
    selectedGameId.value = gameId;
    teamOptions = game.teams.map((team) => ({
      value: team,
      label: team.name,
    }));

    isEndGameModalOpen.value = true;
  }
};

const deleteTeamsWhenChecked = async (gameId: number) => {
  console.log("deleteTeamsWhenChecked called with gameId:", gameId);
  const game = gameStore.games.find((g) => g.id === gameId);
  if (game) {
    const gameTeams = game.teams;
    console.log("Teams associated with game:", gameTeams);

    for (const team of gameTeams) {
      console.log("Deleting team:", team.id);
      await teamStore.deleteTeam(team.id);
    }
  }
};

const submitWinner = async () => {
  if (selectedTeam.value && selectedGameId.value) {
    await teamStore.addPointsToTeam(selectedTeam.value.value);

    const game = gameStore.games.find((g) => g.id === selectedGameId.value);
    if (!game) {
      console.error("Game not found with ID:", selectedGameId.value);
      return;
    }

    if (game.deleteTeams) {
      await deleteTeamsWhenChecked(selectedGameId.value);
    } else {
      console.log("deleteTeams is false, skipping team deletion.");
    }

    try {
      console.log("Deleting game with id:", selectedGameId.value);
      await gameStore.deleteGame(selectedGameId.value);
    } catch (error) {
      console.error("Failed to delete game:", selectedGameId.value, error);
    }
    isEndGameModalOpen.value = false;
    navigateTo("/players");
  }
};

const cancelSelection = () => {
  isEndGameModalOpen.value = false;
};

onMounted(async () => {
  try {
    isLoading.value = true;
    await playerStore.loadPlayers();
    await gameStore.loadGames();
  } catch (error) {
    console.error("Error loading data:", error);
  } finally {
    isLoading.value = false;
  }
});
</script>

<style scoped>
@import "@/assets/css/tableStyle.css";

.loader {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 1s linear infinite;
}
</style>
