<template>
  <div class="hero">
    <h1 class="text-xl text-center">{{ title }}</h1>

    <div v-if="games.length === 0" class="text-center text-red-500">
      No games have been played
    </div>

    <div v-else>
      <UTable :columns="columns" :rows="games">
        <template #actions-data="{ row }">
          <UButton
            type="button"
            color="red"
            variant="ghost"
            icon="i-heroicons-stop-circle-20-solid"
            @click="openEndGameModal(row.id)"
          >
          </UButton>
        </template>
      </UTable>
    </div>

    <UModal v-model="isEndGameModalOpen" prevent-close>
      <UCard
        :ui="{
          ring: '',
          divide: 'divide-y divide-gray-100 dark:divide-gray-800',
        }"
      >
        <template #header>
          <div class="flex items-center justify-between">
            <h3
              class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
            >
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
          <p>Select the winner of the game:</p>
          <div class="p-4">
            <UFormGroup name="selectedTeam">
              <USelectMenu
                v-model="selectedTeam"
                :options="teamOptions"
                placeholder="Select winning team"
              />
            </UFormGroup>
          </div>
        </div>

        <template #footer>
          <div class="flex justify-end space-x-2">
            <UButton color="green" @click="submitWinner">Submit Winner</UButton>
            <UButton color="red" @click="cancelSelection">Cancel</UButton>
          </div>
        </template>
      </UCard>
    </UModal>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useGameStore, type Player } from "#imports";
import { usePlayerStore } from "~/stores/playerStore";

defineProps<{ title: String }>();

const gameStore = useGameStore();
const playerStore = usePlayerStore();
const teamStore = useTeamStore();

const { players } = storeToRefs(playerStore);
const { games } = storeToRefs(gameStore);

const columns = [
  { key: "name", label: "Game" },
  { key: "teams.0.name", label: "Team 1" },
  { key: "teams.1.name", label: "Team 2" },
  { key: "actions", label: "End game" },
];

const isEndGameModalOpen = ref(false);
const selectedGameId = ref<number | null>(null);
const selectedTeam = ref<Team>();

let teamOptions = ref<{ id: number; name: string; members: Player[] }[]>([]);

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

const submitWinner = async () => {
  if (selectedTeam.value && selectedGameId.value) {
    await teamStore.addPointsToTeam(selectedTeam.value.value);
    isEndGameModalOpen.value = false;

    await gameStore.deleteGame(selectedGameId.value);

    navigateTo("/players");
  }
};

const cancelSelection = () => {
  isEndGameModalOpen.value = false;
};

onMounted(() => {
  playerStore.loadPlayers();
  gameStore.loadGames();
});
</script>

<style scoped>
.hero {
  background-color: #e8e8d9;
}
</style>
