import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Player } from "~/types/player";

export const usePlayerStore = defineStore('tiim', () => {
    let currentId: number = 0;

    function generateId(): number {
      currentId++;
      return currentId;
    }

    const addPlayer = (student: Player) => {
        players.value.push(student)
    }

    const deletePlayer = async (playerId: number) => {
        const index = players.value.findIndex((player) => player.id === playerId);
        if (index !== -1) {
          players.value.splice(index, 1);
        }
      }

    const players = ref<Player[]>([
        {id: generateId(), name: 'Ivo Linna', team: 'A', points: 0 },
        {id: generateId(), name: 'Arvo Pärt', team: 'A', points: 15 },
        {id: generateId(), name: 'Kristjan Jõekalda', team: 'A', points: 0 },
        {id: generateId(), name: 'Ott Sepp', team: 'A', points: 0 },
        {id: generateId(), name: 'Jüri Ratas', team: 'A', points: 6 },
        {id: generateId(), name: 'Koit Toome', team: 'B', points: 0 },
        {id: generateId(), name: 'Anu Saagim', team: 'B', points: 23 },
        {id: generateId(), name: 'Evelin Ilves', team: 'B', points: 0 },
        {id: generateId(), name: 'Marko Reikop', team: 'B', points: 20 },
        {id: generateId(), name: 'Mihkel Raud', team: 'B', points: 19 },
      ]);

      return { players, generateId, addPlayer, deletePlayer };
  })