<template>
  <div v-if="games.length === 0" class="text-center text-red-500">
    No games have been played
  </div>

  <div class="mb-4 table-container flex items-center justify-between">
    <input
      v-model="searchQuery"
      type="text"
      placeholder="Search games or teams..."
      class="search-input"
    />
    <StartGame />
  </div>

  <div v-if="filteredGames.length === 0" class="text-center text-red-500">
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
        <TableRow v-for="game in filteredGames" :key="game.id" class="border-b border-black">
          <TableCell>{{ game.name }}</TableCell>
          <TableCell>{{ game.teams[0]?.name || 'N/A' }}</TableCell>
          <TableCell>{{ game.teams[1]?.name || 'N/A' }}</TableCell>
          <TableCell>
            <UButton
              type="button"
              color="red"
              variant="ghost"
              icon="i-heroicons-stop-circle-20-solid"
              @click="openEndGameModal(game.id)"
            >
            </UButton>
          </TableCell>
        </TableRow>
      </TableBody>
      </Table>
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
            <h3 class="text-base font-semibold leading-6 text-white" >
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
			    <h3 class="text-base font-semibold leading-6 text-white"
          	>Select the winner of the game:
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
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useGameStore, type Player } from "#imports";
import { usePlayerStore } from "~/stores/playerStore";
import StartGame from "./StartGame.vue";

const gameStore = useGameStore();
const playerStore = usePlayerStore();
const teamStore = useTeamStore();

const { players } = storeToRefs(playerStore);
const { games } = storeToRefs(gameStore);
const searchQuery = ref("");

const filteredGames = computed(() => {
  if (!searchQuery.value.trim()) {
    return games.value;
  }
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return games.value.filter((game) =>
    game.name.toLowerCase().includes(lowerCaseQuery) ||
    game.teams.some((team) =>
      team?.name?.toLowerCase().includes(lowerCaseQuery)
    )
  );
});

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
  @import "@/assets/css/tableStyle.css"; 
</style> 