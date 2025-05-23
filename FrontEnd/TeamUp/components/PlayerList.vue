<template>
  <div class="mb-4 table-container flex items-center justify-between">
    <input
      v-model="searchQuery"
      type="text"
      placeholder="Search players..."
      class="search-input"
    />
    <AddPlayer />
  </div>

  <div v-if="isLoading" class="flex justify-center items-center h-64">
    <div class="loader"></div>
  </div>

  <div v-else-if="players.length === 0" class="text-center text-red-500">
    No players have been added
  </div>

  <div v-else-if="filteredPlayers.length === 0" class="text-center text-red-500">
    No players match your search.
  </div>

  <div v-else>
    <div class="table-container">
      <Table class="Table">
        <TableHeader>
          <TableRow class="header-row">
            <TableCell class="header-cell">Rank</TableCell>
            <TableCell class="header-cell">Points</TableCell>
            <TableCell class="header-cell">Name</TableCell>
            <TableCell class="header-cell">Actions</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow
            v-for="player in filteredPlayers"
            :key="player.id"
            class="border-b border-black"
          >
            <TableCell>{{ player.rank }}</TableCell>
            <TableCell>{{ player.points }}</TableCell>
            <TableCell>{{ player.name }}</TableCell>
            <TableCell>
              <UButton
                type="button"
                color="black"
                variant="ghost"
                icon="i-heroicons-pencil-20-solid"
                class="edit-button"
                @click="openEditModal(player.id)"
              ></UButton>
              <UButton
                type="button"
                color="red"
                variant="ghost"
                icon="i-heroicons-trash-20-solid"
                @click="openDeleteModal(player.id)"
              ></UButton>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>

  <UModal v-model="isDeleteModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-900',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <UFormGroup label="Delete player" name="deletePlayer"></UFormGroup>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            @click="isDeleteModalOpen = false"
          />
        </div>
      </template>

      <div class="p-4">
        <UKbd class="ring-0 bg-transparent font-semibold text-base px-3 py-4">
            Are you sure you want to delete player: {{ selectedPlayerName }}
          </UKbd>
      </div>

      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="red" @click="submitDelete">Delete player</UButton>
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
          <UFormGroup label="Edit player" name="editPlayer"></UFormGroup>
            <UButton
              color="gray"
              variant="ghost"
              icon="i-heroicons-x-mark-20-solid"
              @click="isEditModalOpen = false"
            />
        </div>
      </template>

      <UFormGroup label="Name" name="playerName" >
        <UInput
          v-model="newName"
          color="purple"
          variant="outline"
          placeholder="Player name"
        />
        <p v-if="errors.newName" class="text-red-500 text-sm mt-1">
          {{ errors.newName }}
        </p>
      </UFormGroup>
      <UFormGroup label="Points" name="points">
        <UInput
          v-model="newScore"
          type="number"
          color="purple"
          variant="outline"
          placeholder="Player score"
          :errors="errors.newScore"
        />
      </UFormGroup>
      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="green" @click="submitPlayer">Save changes</UButton>
        </div>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { usePlayerStore } from "~/stores/playerStore";
import { ref, onMounted, computed } from "vue";
import AddPlayer from "./AddPlayer.vue";

const playerStore = usePlayerStore();
const { players } = storeToRefs(playerStore);

const isDeleteModalOpen = ref(false);
const selectedPlayerId = ref<number | null>(null);
const selectedPlayerName = ref<string | null>(null);
const currentTeamId = ref<number | null>(null);
const isEditModalOpen = ref(false);
const newName = ref("");
const newScore = ref<number>(0);
const searchQuery = ref("");
const isLoading = ref(true);

const filteredPlayers = computed(() => {
  if (!searchQuery.value.trim()) {
    return players.value;
  }
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return players.value.filter((player) =>
    player.name.toLowerCase().includes(lowerCaseQuery)
  );
});

const openDeleteModal = (playerId: number) => {
  const player = playerStore.players.find((p) => p.id === playerId);
  if (player) {
    isDeleteModalOpen.value = true;
    selectedPlayerId.value = playerId;
    selectedPlayerName.value = player.name;
  }
};

const submitDelete = async () => {
  if (selectedPlayerId.value !== null) {
    await playerStore.deletePlayer(selectedPlayerId.value);
    isDeleteModalOpen.value = false;
    selectedPlayerId.value = null;
    navigateTo("/players");
  }
};

const errors = reactive<{ newName: string | null }>({
  newName: null,
});

const validateEditForm = () => {
  errors.newName = newName.value.trim() ? null : "Required";
  return !errors.newName;
};

const openEditModal = (playerId: number) => {
  const player = playerStore.players.find((p) => p.id === playerId);
  if (player) {
    selectedPlayerId.value = playerId;
    newName.value = player.name;
    newScore.value = player.points;
    console.log("Player teamid:", player.teamId);
    currentTeamId.value = player.teamId || null;

    console.log("Current Team ID after assignment:", currentTeamId.value);
    isEditModalOpen.value = true;
  }
};

const submitPlayer = () => {
  if (validateEditForm()) {
    if (selectedPlayerId.value !== null) {
      playerStore.updatePlayer(
        selectedPlayerId.value,
        newName.value,
        newScore.value,
        currentTeamId.value
      );
      isEditModalOpen.value = false;
      selectedPlayerId.value = null;
      navigateTo("/players");
    }
  }
};

onMounted(async () => {
  try {
    isLoading.value = true;
    await playerStore.loadPlayers();
  } catch (error) {
    console.error("Error loading players:", error);
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

  button.edit-button:hover {
  background-color: #333; /* Tumedam värv */
  color: white; /* Teksti värv kontrastiks */
}
</style>
