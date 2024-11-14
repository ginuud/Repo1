<template>
<div>
    <h1 class="text-xl text-center">{{ title }}</h1>
    <template v-if="players.length > 0">
      <UTable v-model:sort="sort" :columns="columns" :rows="players">
        <template #actions-data="{ row }">
        <UButton
          type="button" color="cyan" variant="ghost" icon="i-heroicons-pencil-20-solid"
          @click="openEditModal(row.id)">
        </UButton>
        <UButton
          type="button" color="red" variant="ghost" icon="i-heroicons-trash-20-solid"
          @click="openDeleteModal(row.id)">
        </UButton>
        </template>
      </UTable>
    </template>
    <template v-else>
      <p class="text-center">Mängijate andmed puuduvad</p>
    </template>
  </div>

  <UModal v-model="isDeleteModalOpen" prevent-close>
  <UCard :ui="{ ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }">
    <template #header>
      <div class="flex items-center justify-between">
        <h3 class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
          Delete player
        </h3>
        <UButton color="gray" variant="ghost" icon="i-heroicons-x-mark-20-solid" class="-my-1" @click="isDeleteModalOpen = false" />
      </div>
    </template>

    <div class="p-4">
      <p>Are you sure you want to delete player: {{ selectedPlayerName }}</p>
    </div>
  
    <template #footer>
      <div class="flex justify-end space-x-2">
        <UButton color="red" @click="submitDelete">Delete Player</UButton>
      </div>
    </template>
  </UCard>
  </UModal>

  <UModal v-model="isEditModalOpen" prevent-close>
  <UCard :ui="{ ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }">
    <template #header>
      <div class="flex items-center justify-between">
        <h3 class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
          Edit player
        </h3>
        <UButton color="gray" variant="ghost" icon="i-heroicons-x-mark-20-solid" class="-my-1" @click="isEditModalOpen = false" />
      </div>
    </template>

    <div class="p-4">
        <UInput v-model="newName" color="cyan" variant="outline" placeholder="Player name" />
        <UInput v-model="newScore" type="number" color="cyan" variant="outline" placeholder="Player score" />
    </div>
    <template #footer>
      <div class="flex justify-end space-x-2">
        <UButton color="green" @click="submitPlayer">Save Changes</UButton>
      </div>
    </template>
  </UCard>
  </UModal>
</template>

<script setup lang ="ts">
import { usePlayerStore } from '~/stores/playerStore';
import { ref, onMounted } from 'vue';

defineProps<{ title: String }>();
const playerStore = usePlayerStore();
const {players} = storeToRefs(playerStore);
const columns = [
{
  key: "Rank",
  label: "Rank",
},
{
  key: "points",
  label: "Points",
  sortable: true
},
{
  key: "name",
  label: "Name",
},
{
  key: "actions",
  label: "Tegevused"
}
];

const isDeleteModalOpen = ref(false);
const selectedPlayerId = ref<number | null>(null);
const selectedPlayerName = ref<string | null>(null);
const currentTeamId = ref<number | null>(null);
const isEditModalOpen = ref(false);
const newName = ref('');
const newScore = ref<number>(0);


const openDeleteModal = (playerId: number) => {
  const player = playerStore.players.find(p => p.id === playerId);
  if (player) {
  isDeleteModalOpen.value = true;
  selectedPlayerId.value = playerId;
  selectedPlayerName.value = player.name;	  
  }
};

const submitDelete = async() => {
  if (selectedPlayerId.value !== null) {
  await playerStore.deletePlayer(selectedPlayerId.value);
  isDeleteModalOpen.value = false;
  selectedPlayerId.value = null;
  navigateTo("/players");
}
};

const openEditModal = (playerId: number) => {
  const player = playerStore.players.find(p => p.id === playerId); 
    if (player) {
	  selectedPlayerId.value = playerId;
    newName.value = player.name;
    newScore.value = player.points;
    if (player.Team) {
      currentTeamId.value = player.team.id;
    }
	  isEditModalOpen.value = true;
  }  
};
// const openEditModal = (playerId: number) => {
//   const player = playerStore.players.find(p => p.id === playerId); 
//     if (player) {
// 	  selectedPlayerId.value = playerId;
//     newName.value = player.name;
//     newScore.value = player.points;
// 	  isEditModalOpen.value = true;
//   }  
// };
const submitPlayer = () => {
  if (selectedPlayerId.value !== null && newName.value && newScore.value) {
	  playerStore.updatePlayer(selectedPlayerId.value, newName.value, newScore.value, currentTeamId.value);
	  isEditModalOpen.value = false;
    selectedPlayerId.value = null;
  navigateTo("/players");
  }
};

// const submitPlayer = () => {
//   if (selectedPlayerId.value !== null && newName.value && newScore.value) {
// 	  playerStore.updatePlayer(selectedPlayerId.value, newName.value, newScore.value);
// 	  isEditModalOpen.value = false;
//     selectedPlayerId.value = null;
//   navigateTo("/players");
//   }
// };

onMounted(async () => {
  await playerStore.loadPlayers();
});
</script>

