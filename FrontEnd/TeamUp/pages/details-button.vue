<template>
    <div class="p-4">
      <h2 class="text-xl font-bold mb-4">Game Details</h2>
  
      <div v-if="gameDetails">
        <p><strong>Team 1:</strong> {{ gameDetails.team1name }}</p>
        <p><strong>Team 2:</strong> {{ gameDetails.team2name }}</p>
        <p><strong>Winner:</strong> {{ gameDetails.winner || 'Game is still in progress' }}</p>
      </div>
      <div v-else class="text-gray-500">Loading game details...</div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import { useGameStore } from '~/stores/gameStore';
  
  const route = useRoute();
  const gameStore = useGameStore();
  const gameDetails = ref<null | { team1name: string; team2name: string; winner: string | null }>(null);
  
  onMounted(() => {
    // Retrieve the game ID from route parameters
    const gameId = Number(route.params.id);
    // Find the game details in the game store
    const game = gameStore.games.find(g => g.id === gameId);
    if (game) {
      gameDetails.value = {
        team1name: game.team1name,
        team2name: game.team2name,
        winner: game.winner || null,  // Retrieve the winner from gameStore
      };
    }
  });
  </script>
  
  