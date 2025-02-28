<template>
  <div
    v-if="filteredGamesHistory.length === 0"
    class="text-center text-red-500"
  >
    No games match your search.
  </div>

  <div v-else>
    <!-- Add the "Game history" title here -->
    <h2 class="game-history-title">Game History</h2>
    <div class="mb-4 table-container flex items-center justify-between">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search games or teams..."
        class="search-input"
      />
    </div>
    <div class="table-container">
      <Table class="Table">
        <TableHeader>
          <TableRow class="header-row">
            <TableCell class="header-cell">Game</TableCell>
            <TableCell class="header-cell">Teams</TableCell>
            <TableCell class="header-cell">Winner</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow
            v-for="gameHistory in filteredGamesHistory"
            :key="gameHistory.id"
            class="border-b border-black"
          >
            <TableCell>{{ gameHistory.name }}</TableCell>
            <TableCell>{{ gameHistory.teams}} </TableCell>
            <TableCell>{{ gameHistory.winner}}</TableCell>
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

const { gamesHistory } = storeToRefs(gameHistoryStore);
const searchQuery = ref("");

const filteredGamesHistory = computed(() => {
  if (!searchQuery.value.trim()) {
    return gamesHistory.value;
  }
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return gamesHistory.value.filter(
    (gameHistory) =>
      gameHistory.name.toLowerCase().includes(lowerCaseQuery) ||
      gameHistory.teams.toLowerCase().includes(lowerCaseQuery)
  );
});


onMounted(() => {
  playerStore.loadPlayers();
  gameHistoryStore.loadGamesHistory();
});
</script>

<style scoped>
@import "@/assets/css/tableStyle.css";
.game-history-title {
  margin-left: 87px;
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 16px;
  margin-top: 60px;
}
</style>
