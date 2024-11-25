<template>
  <div class="hero">
    <h1 class="text-xl text-center">{{ title }}</h1>

    <div v-if="games.length === 0" class="text-center text-red-500">
      No games have been played
    </div>

    <div v-else>
      <UTable
        :ui="{
          base: 'font-medium',
          th: {
            color: 'text-black-900',
            size: 'text-la',
          },
          td: {
            color: 'text-gray-600',
          },
        }"
        :columns="columns"
        :rows="games"
        class="ml-40 mr-40 mb-20"
      >
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
          divide: 'divide-y divide-gray-900',
        }"
		class="hero"
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
			<h3
          	class="text-base font-semibold leading-6 text-gray-900 dark:text-white"
          	>Select the winner of the game:
        	</h3>
            <UFormGroup name="selectedTeam">
              <USelectMenu
                v-model="selectedTeam"
                :options="teamOptions"
                placeholder="Select winning team"
				color="cyan"
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

const deleteTeamsWhenChecked= async (gameId: number) => {
	console.log("deleteTeamsWhenChecked called with gameId:", gameId);
	const game = gameStore.games.find(g => g.id === gameId);
	if (game) {
    const gameTeams = game.teams;
	console.log("Teams associated with game:", gameTeams);

    for (const team of gameTeams) {
      console.log("Deleting team:", team.id);
      await teamStore.deleteTeam(team.id); 
    }
  }
}

const submitWinner = async () => {
  	if (selectedTeam.value && selectedGameId.value) {
	await teamStore.addPointsToTeam(selectedTeam.value.value);
	
	const game = gameStore.games.find(g => g.id === selectedGameId.value);
    if (!game) {
      console.error("Game not found with ID:", selectedGameId.value);
      return;
    }

	if (game.deleteTeams)
  	{
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
