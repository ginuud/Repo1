<template>
  <UButton 
    :ui="{ rounded: 'rounded-full' }"
    size="md"
    color="primary"
    variant="solid"
    label="Start game"
    @click="openStartGameModal">
  </UButton>

  <UModal v-model="isStartGameModalOpen" prevent-close>
  <UCard :ui="{ ring: '', divide: 'divide-y divide-gray-100 dark:divide-gray-800' }">
    <template #header>
      <div class="flex items-center justify-between">
        <h3 class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
          Start game
        </h3>
        <UButton color="gray" variant="ghost" icon="i-heroicons-x-mark-20-solid" class="-my-1" @click="isStartGameModalOpen = false" />
      </div>
    </template>

    <div class="p-4 space-y-4">
      <UFormGroup label="Game name" name="name">
        <UInput v-model="name" color="cyan" variant="outline" placeholder="Game name" />
        <p v-if="errors.name" class="text-red-500 text-sm mt-1">{{ errors.name }}</p>
      </UFormGroup>

      <UFormGroup label="Teams" name="teams">
        <USelectMenu
          v-model="selectedTeams"
          :options="teamOptions"
          multiple
          searchable
          searchable-placeholder="Search teams..."
          placeholder="Select 2 teams" />
          <p v-if="errors.teams" class="text-red-500 text-sm mt-1">{{ errors.teams }}</p>
      </UFormGroup>

        <UCheckbox v-model="deleteTeams" name="deleteTeams" label="Delete teams when game ends" />
    </div>
  
    <template #footer>
      <div class="flex justify-end space-x-2">
        <UButton color="green" @click="startGame">Start game</UButton>
      </div>
    </template>
  </UCard>
  </UModal>
</template>
  
<script setup lang="ts">
import { ref, reactive, computed } from 'vue';
import { useGameStore } from '~/stores/gameStore';
import { useTeamStore } from '~/stores/teamStore';

const gameStore = useGameStore();
const teamStore = useTeamStore();

const isStartGameModalOpen = ref(false);
const name = ref('');
const selectedTeams  = ref([]);
const errors = reactive({ name: null as string | null, teams: null as string | null });
const deleteTeams = ref(false);

const teamOptions = computed(() => 
  teamStore.teams
  .filter(team => team.gameId === null)
  .map(team => ({ value: team, label: team.name }))
);

const validate = (): boolean => {
  errors.name = name.value.trim() ? null : 'Game name is required.';
  errors.teams = selectedTeams.value.length === 2 ? null : 'Please select exactly 2 teams.';
  return !errors.name && !errors.teams;
};

const openStartGameModal = () => {
  isStartGameModalOpen.value = true;
  name.value = '';
  selectedTeams.value = [];
  deleteTeams.value = false;  	  
};

const startGame = async() => {
  if (validate()) {
    await gameStore.addGame({
      id: gameStore.generateId(),
      name: name.value,
      teams: selectedTeams.value.map(option => option.value),
      deleteTeams: deleteTeams.value,
    });
    isStartGameModalOpen.value = false;
  }
};

onMounted(() => {
  gameStore.loadGames();
  teamStore.loadTeams();
})
</script>

