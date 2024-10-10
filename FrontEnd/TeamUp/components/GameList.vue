<template> 
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
      <div v-if="games.length === 0" class="text-center text-red-500">
        No games have been played        
      </div>
      <div v-else>
        <UTable :columns="columns" :rows="games">
            <template #actions-data="{ row }" >
                <UButton 
                    type="button" color="red" variant="ghost" icon="i-heroicons-stop-circle-20-solid" @click="endGame(row.id)">
                </UButton>
            </template>
        </UTable>
      </div>
    </div>
</template>

<script setup lang="ts">

import { useGameStore } from '#imports';  
  
defineProps<{ title: String }>();
  
const columns = [
    {
      key: "name", 
      label: "Game",
    },
    {
      key: "team1name",
      label: "Team 1",
    },
    {
      key: "team2name",
      label: "Team 2",
    },
    {
      key: "status",
      label: "Status",
    },
    {
    key: "actions",
    label: "End game",
  },
  
];
  
const gameStore = useGameStore();
const games = computed(() =>gameStore.games.map(game => ({
    ...game})));

import { useRouter } from 'vue-router';
const router = useRouter();

const endGame = async (id: number) => {
 await router.push("/players");
};

</script>