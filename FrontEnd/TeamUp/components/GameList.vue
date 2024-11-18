<template>
  <div>
	<h1 class="text-xl text-center">{{ title }}</h1>

	<div v-if="games.length === 0" class="text-center text-red-500">
  	No games have been played   	 
	</div>

	<div v-else>
  	<UTable :columns="columns" :rows="games">
    	<template #actions-data="{ row }">
      	<!-- Open modal instead of redirecting -->
      	<UButton
        	type="button" color="red" variant="ghost" icon="i-heroicons-stop-circle-20-solid"
        	@click="openModal(row.id)">
      	</UButton>
    	</template>
  	</UTable>
	</div>

	<!-- Modal for selecting winner -->
	<UModal v-model="isModalOpen" prevent-close>
  	<UCard :ui="{ ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }">
    	<template #header>
      	<div class="flex items-center justify-between">
        	<h3 class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
          	Select Winner
        	</h3>
        	<UButton color="gray" variant="ghost" icon="i-heroicons-x-mark-20-solid" class="-my-1" @click="isModalOpen = false" />
      	</div>
    	</template>

    	<div class="p-4">
      	<p>Select the winner of the game:</p>
      	<select v-model="selectedTeam" class="border p-2 rounded w-full">
        	<option :value="team1">{{ team1.name }}</option>
        	<option :value="team2">{{ team2.name }}</option>
      	</select>
    	</div>
    
    	<template #footer>
      	<div class="flex justify-end space-x-2">
        	<UButton @click="submitWinner">Submit Winner</UButton>
        	<UButton color="red" @click="cancelSelection">Cancel</UButton>
      	</div>
    	</template>
  	</UCard>
	</UModal>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useGameStore } from '#imports';
import { usePlayerStore } from '~/stores/playerStore';

defineProps<{ title: String }>();

const gameStore = useGameStore();
const playerStore = usePlayerStore();
const teamStore = useTeamStore();

const {players} = storeToRefs(playerStore)
const {games} = storeToRefs(gameStore)

onMounted(() => {
    playerStore.loadPlayers();
    console.log("Players loaded:", players.value);
	gameStore.loadGames();
	console.log("Games loaded:", games.value)
  })

const columns = [
  { key: "name", label: "Game" },
  { key: "teams.0.name", label: "Team 1" },
  { key: "teams.1.name", label: "Team 2" },
  { key: "actions", label: "End game" },
];

const isModalOpen = ref(false);
const selectedGameId = ref<number | null>(null);
const selectedTeam = ref<'A' | 'B' | null>(null);
const team1 = ref('');
const team2 = ref('');

const openModal = (gameId: number) => {
  const game = gameStore.games.find(g => g.id === gameId);
  if (game) {
	selectedGameId.value = gameId;
	team1.value = game.teams[0] || '';
	team2.value = game.teams[1] || '';
	isModalOpen.value = true;
  }
};

const submitWinner = async () => {
  if (selectedTeam.value && selectedGameId.value) {
	await teamStore.addPointsToTeam(selectedTeam.value);
	isModalOpen.value = false;
	await gameStore.deleteGame(selectedGameId.value)
	selectedGameId.value = null;
	selectedTeam.value = null;
  	navigateTo("/players");
  }
};

const cancelSelection = () => {
  isModalOpen.value = false;
};
</script>


