<template>
  <div>
	<h1 class="text-xl text-center">{{ title }}</h1>

	<div v-if="games.length === 0" class="text-center text-red-500">
  	No games have been played   	 
	</div>

	<div v-else>
  	<UTable :columns="columns" :rows="games">
    	<template #status-data="{ row }">
      	<UBadge
        	v-if="row.status === 'in progress'" color="green" variant="subtle">
        	In progress
      	</UBadge>
      	<UBadge
        	v-else-if="row.status === 'inactive'" color="red" variant="subtle">
        	Inactive
      	</UBadge>
    	</template>

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
        	<option :value="team1">{{ team1 }}</option>
        	<option :value="team2">{{ team2 }}</option>
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
import { ref, computed } from 'vue';
import { useGameStore } from '#imports';
import { useRouter } from 'vue-router';
import { usePlayerStore } from '~/stores/playerStore';

defineProps<{ title: String }>();

const gameStore = useGameStore();
const playerStore = usePlayerStore();

const columns = [
  { key: "name", label: "Game" },
  { key: "team1name", label: "Team 1" },
  { key: "team2name", label: "Team 2" },
  { key: "status", label: "Status" },
  { key: "actions", label: "End game" },
];

const games = computed(() => gameStore.games.map(game => ({ ...game })));

// Modal state
const isModalOpen = ref(false);
const selectedGameId = ref<number | null>(null);
const selectedTeam = ref<'A' | 'B' | null>(null);
const team1 = ref('');
const team2 = ref('');

// Open modal and set game data
const openModal = (gameId: number) => {
  const game = gameStore.games.find(g => g.id === gameId);
  if (game) {
	selectedGameId.value = gameId;
	team1.value = game.team1name || '';
	team2.value = game.team2name || '';
	isModalOpen.value = true;
  }
};

// Handle submitting the winner
const submitWinner = () => {
  if (selectedTeam.value && selectedGameId.value) {
	playerStore.addPointsToWinningTeam(selectedTeam.value);
	gameStore.makeStatusInactive(selectedGameId.value, "in progress");
	isModalOpen.value = false;
  navigateTo("/players");
  }
};

// Cancel and close modal
const cancelSelection = () => {
  isModalOpen.value = false;
};
</script>


