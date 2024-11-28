<template>
  <div class="hero">
    <h1 class="text-xl text-center">{{ title }}</h1>

    <div class="mb-4 table-container">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search games or teams..."
        class="search-input focus:ring-cyan-300"
      />
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
          <TableRow v-for="game in filteredGames" :key="game.id">
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
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useGameStore, type Player } from "#imports";
import { usePlayerStore } from "~/stores/playerStore";

defineProps<{ title: String }>();

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
  background-color: #cee7f8;
}

.search-input {
  background-color: white; /* Valge taust */
  color: #333; /* Teksti värv */
  width: 300px; /* Laius */
  padding: 8px 12px; /* Sisu ruum */
  border: 1px solid #9f57e2; /* Piirjoon */
  border-radius: 4px; /* Ümarad nurgad */
  box-shadow: 0 4px 4px rgb(142, 69, 238); /* Kerge vari */
  font-size: 15px; 
}
.search-input:focus {
  outline: none; /* Eemaldab brauseri vaikimisi fookuse piirjoone */
  border-color: #9f57e2; /* Tsüaani värvi piirjoon */
  box-shadow: 0 0 5px rgba(142, 69, 238, 0.5); /* Tsüaani värvi varjuefekt */
}

.table-container {
  max-width: 90%; 
  margin: 0 auto; 
  padding: 20px; 
}

.Table {
  width: 100%;
  border-spacing: 0;
  border-collapse: collapse; 
}
.Table tr:hover{
  background: none
}
.header-row {
  box-shadow: 0 5px 15px rgb(142, 69, 238);
}

.header-cell {
  font-weight: bold;
  color: #06011a; /* Tumehall tekst */
}

.Table th,
.Table td {
  padding: 10px;
  text-align: left;
}

</style> 