<template>
    <div v-if="gamesHistory.length === 0" class="text-center text-red-500">
      No games have been played
    </div>
  
    <div class="mb-4 table-container flex items-center justify-between">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search games or teams..."
        class="search-input"
      />
    </div>
  
    <div v-if="filteredGamesHistory.length === 0" class="text-center text-red-500">
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
          <TableRow v-for="gameHistory in filteredGamesHistory" :key="gameHistory.id" class="border-b border-black">
            <TableCell>{{ gameHistory.name }}</TableCell>
          </TableRow>
        </TableBody>
        </Table>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, computed } from "vue";
  import { useGameHistoryStore, type Player, type TeamHistory } from "#imports";
  import { usePlayerStore } from "~/stores/playerStore";
  
  const gameHistoryStore = useGameHistoryStore();
  const playerStore = usePlayerStore();

  
  const { players } = storeToRefs(playerStore);
  const { gamesHistory } = storeToRefs(gameHistoryStore);
  const searchQuery = ref("");
  
  const filteredGamesHistory = computed(() => {
    if (!searchQuery.value.trim()) {
      return gamesHistory.value;
    }
    const lowerCaseQuery = searchQuery.value.toLowerCase();
    return gamesHistory.value.filter((gameHistory) =>
      gameHistory.name.toLowerCase().includes(lowerCaseQuery) ||
      gameHistory.teamsHistory.some((teamHistory) =>
        teamHistory?.name?.toLowerCase().includes(lowerCaseQuery)
      )
    );
  });
  
  const isEndGameModalOpen = ref(false);
  const selectedGameId = ref<number | null>(null);
  const selectedTeamHistory = ref<TeamHistory>();
  
  let teamHistoryOptions = ref<{ id: number; name: string; members: Player[] }[]>([]);
  
  
  onMounted(() => {
    playerStore.loadPlayers();
    gameHistoryStore.loadGamesHistory();
  });
  </script>
  
  <style scoped>
    @import "@/assets/css/tableStyle.css"; 
  </style> 