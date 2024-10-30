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
          v-if="row.status === 'in progress'"
        	type="button" color="red" variant="ghost" icon="i-heroicons-stop-circle-20-solid"
        	@click="openModal(row.id)">
      	</UButton>

        <!-- Show "Details" button if game is inactive -->
        <UButton
            v-else-if="row.status === 'inactive'"
            type="button" color="blue" variant="ghost" icon="i-heroicons-information-circle-20-solid"
            @click="openDetails(row.id)">
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

  <!-- Slideover for game details -->
  <USlideover v-model="isOpen">
    <UCard
      class="flex flex-col flex-1"
      :ui="{ body: { base: 'flex-1' }, ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }"
    >
      <template #header>
        <UButton
          color="gray"
          variant="ghost"
          size="sm"
          icon="i-heroicons-x-mark-20-solid"
          class="flex sm:hidden absolute end-5 top-5 z-10"
          square
          padded
          @click="isOpen = false"
        />
        <h3 class="text-lg font-semibold leading-6 text-gray-900 dark:text-white">
          Game Details
        </h3>
      </template>

      <div class="p-4">
        <div v-if="gameDetails">
          <!-- Display game details here -->
          <p><strong>Game:</strong> {{ gameDetails.name }}</p>
          <p><strong>Team 1:</strong> {{ gameDetails.team1name }}</p>
          <p><strong>Team 2:</strong> {{ gameDetails.team2name }}</p>
          <p><strong>Status:</strong> {{ gameDetails.status }}</p>
          <p><strong>Winner:</strong> {{ gameDetails.winner || 'Game is still in progress' }}</p>
        </div>
        <div v-else>
          <p>Loading game details...</p>
        </div>
      </div>

      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="gray" @click="isOpen = false">Close</UButton>
        </div>
      </template>
    </UCard>
  </USlideover>
  </div>
</template>


<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { useGameStore } from '~/stores/gameStore';
import { useRouter } from 'vue-router';
import { usePlayerStore } from '~/stores/playerStore';


defineProps<{ title: String }>();

const gameStore = useGameStore();
const playerStore = usePlayerStore();

// Computed property to get game details based on selectedGameId
const gameDetails = computed(() => {
  return selectedGameId.value !== null
    ? gameStore.games.find(game => game.id === selectedGameId.value)
    : null;
});



const columns = [
  { key: "name", label: "Game" },
  { key: "team1name", label: "Team 1" },
  { key: "team2name", label: "Team 2" },
  { key: "status", label: "Status" },
  { key: "actions", label: "Actions" },
];

const games = computed(() => gameStore.games.map(game => ({ ...game })));

// Modal state
const isModalOpen = ref(false);
const selectedGameId = ref<number | null>(null);
const selectedTeam = ref<'A' | 'B' | null>(null);
const team1 = ref('');
const team2 = ref('');

const isOpen = ref(false);

// Open modal and set game data

//watch(selectedGameId, (newId) => {
 // if (newId !== null) {
   // isOpen.value = true;
  //}
//});

// Open modal and set game data
const openModal = (gameId: number) => {
  const game = gameStore.games.find(g => g.id === gameId);
  if (game && game.status === 'in progress') {
	selectedGameId.value = gameId;
	team1.value = game.team1name || '';
	team2.value = game.team2name || '';
	isModalOpen.value = true;
  }
};

const openDetails = (gameId: number) => {
  const game = gameStore.games.find(g => g.id === gameId);
  if(game && game.status === 'inactive'){
    selectedGameId.value = gameId;
    isOpen.value = true;
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


