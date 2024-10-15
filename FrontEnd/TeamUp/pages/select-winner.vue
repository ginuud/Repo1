<template>
    <UCard>
      <template #header>
        <h2 class="text-xl font-semibold">Select winner</h2>
      </template>
  
      <div class="p-4">
        <p>Select the winner of the game:</p>
        <select v-model="selectedTeam" class="border p-2 rounded">
          <option :value="team1">{{ team1 }}</option>
          <option :value="team2">{{ team2 }}</option>
        </select>
      </div>
    
      <template #footer>
        <UButton @click="submitWinner">Submit Winner</UButton>
        <UButton color="red" @click="cancelSelection">Cancel</UButton>
      </template>
    </UCard>
  </template>
  
  <script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useGameStore } from '~/stores/stores'; 
  import { usePlayerStore } from '~/stores/stores'; 
  
  const route = useRoute();
  const router = useRouter();
  const gameStore = useGameStore();
  const playerStore = usePlayerStore();
  
  const gameId = ref(Number(route.query.gameId)); 
  const game = computed(() => gameStore.games.find(g => g.id === gameId.value));
  
  const team1 = computed(() => game.value?.team1name || '');
  const team2 = computed(() => game.value?.team2name || '');
  const selectedTeam = ref<'A' | 'B' | null>(null);
  
  const submitWinner = () => {
    if (selectedTeam.value) {
      playerStore.addPointsToWinningTeam(selectedTeam.value);
      gameStore.makeStatusInactive(gameId.value, "in progress"); 
      router.push('/players'); 
    }
  };
  
  const cancelSelection = () => {
    router.push('/games')
  };
  </script>
    